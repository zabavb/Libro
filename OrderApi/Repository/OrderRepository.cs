using BookAPI.Models;
using BookAPI.Repositories.Interfaces;
using Library.Common;
using Library.DTOs.Order;
using Library.DTOs.UserRelated.User;
using Microsoft.EntityFrameworkCore;
using OrderApi.Data;
using OrderAPI;
using StackExchange.Redis;
using System.Text.Json;
using Library.Interfaces;
using Library.Sorts;
using Order = OrderApi.Models.Order;

namespace OrderApi.Repository
{
    public class OrderRepository(
        OrderDbContext context,
        IConnectionMultiplexer redis,
        ILogger<IOrderRepository> logger
        ) : IOrderRepository
    {
        private readonly OrderDbContext _context = context;
        private readonly IDatabase _redisDatabase = redis.GetDatabase();
        public readonly string _cacheKeyPrefix = "Order_";
        public readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(10);
        private readonly ILogger<IOrderRepository> _logger = logger;
        

        public async Task<PaginatedResult<Order>> GetAllPaginatedAsync(int pageNumber, int pageSize, string? searchTerm,
            Filter? filter, Sort? sort)
        {
            IEnumerable<Order> orders;

            string cacheKey = $"{_cacheKeyPrefix}All";
            var cachedOrders = await _redisDatabase.HashGetAllAsync(cacheKey);
            if (cachedOrders.Length > 0)
            {
                orders = cachedOrders.Select(entry => JsonSerializer.Deserialize<Order>(entry.Value!)!);
                _logger.LogInformation("Fetched from CACHE.");
            }
            else
            {
                orders = _context.Orders.AsNoTracking();
                _logger.LogInformation("Fetched from DB.");

                var hashEntries = orders.ToDictionary(
                    order => order.OrderId.ToString(),
                    order => JsonSerializer.Serialize(order)
                );
                await _redisDatabase.HashSetAsync(
                    cacheKey,
                    [.. hashEntries.Select(kvp => new HashEntry(kvp.Key, kvp.Value))]
                );
                await _redisDatabase.KeyExpireAsync(cacheKey, _cacheExpiration);
                _logger.LogInformation("Set to CACHE.");
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
                orders = await SearchEntitiesAsync(searchTerm, orders);
            if (orders.Any() && filter != null)
                orders = await FilterEntitiesAsync(orders, filter);
            if (orders.Any() && sort != null)
                orders = await SortAsync(orders, sort);

            var totalOrders = await Task.FromResult(orders.Count());
            orders = await Task.FromResult(orders.Skip((pageNumber - 1) * pageSize).Take(pageSize));
            ICollection<Order> result = [.. orders];

            return new PaginatedResult<Order>
            {
                Items = result,
                TotalCount = totalOrders,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<IEnumerable<Order>> SearchEntitiesAsync(string searchTerm, IEnumerable<Order> data)
        {
            if (data == null)
            {
                return await _context.Orders
                    .AsNoTracking()
                    .Where(o => o.Address.Contains(searchTerm) ||
                                o.Region.Contains(searchTerm) ||
                                o.City.Contains(searchTerm))
                    .ToListAsync();
            }

            return await Task.FromResult(
                data.Where(o => o.Address.Contains(searchTerm) ||
                                o.Region.Contains(searchTerm) ||
                                o.City.Contains(searchTerm)));
        }

        public async Task<IEnumerable<Order>> FilterEntitiesAsync(IEnumerable<Order> orders, Filter filter)
        {
            if (filter.OrderDateStart.HasValue)
                orders = orders.Where(o => o.OrderDate >= filter.OrderDateStart.Value);

            if (filter.OrderDateEnd.HasValue)
                orders = orders.Where(o => o.OrderDate <= filter.OrderDateEnd.Value);

            if (filter.DeliveryDateStart.HasValue)
                orders = orders.Where(o => o.DeliveryDate >= filter.DeliveryDateStart);

            if (filter.DeliveryDateEnd.HasValue)
                orders = orders.Where(o => o.DeliveryDate <= filter.DeliveryDateEnd);

            if (filter.Status.HasValue)
                orders = orders.Where(o => o.Status == filter.Status);

            if (filter.DeliveryId.HasValue)
                orders = orders.Where(o => o.DeliveryTypeId == filter.DeliveryId);

            if (filter.UserId.HasValue)
                orders = orders.Where(o => o.UserId == filter.UserId);

            return await Task.FromResult(orders.ToList());
        }

        public async Task<IEnumerable<Order>> SortAsync(IEnumerable<Order> orders, Sort sort)
        {
            var query = orders.AsQueryable();

            if (sort.OrderDate != Bool.NULL)
                query = sort.OrderDate == Bool.ASCENDING
                    ? query.OrderBy(o => o.OrderDate)
                    : query.OrderByDescending(o => o.OrderDate);

            if (sort.BooksAmount != Bool.NULL)
                query = sort.BooksAmount == Bool.ASCENDING
                    ? query.OrderBy(o => o.Books.Values.Sum())
                    : query.OrderByDescending(o => o.Books.Values.Sum());

            if (sort.OrderPrice != Bool.NULL)
                query = sort.OrderPrice == Bool.ASCENDING
                    ? query.OrderBy(o => o.Price)
                    : query.OrderByDescending(o => o.Price);

            if (sort.DeliveryPrice != Bool.NULL)
                query = sort.DeliveryPrice == Bool.ASCENDING
                    ? query.OrderBy(o => o.DeliveryPrice)
                    : query.OrderByDescending(o => o.DeliveryPrice);

            if (sort.DeliveryDate != Bool.NULL)
                query = sort.DeliveryDate == Bool.ASCENDING
                    ? query.OrderBy(o => o.DeliveryDate)
                    : query.OrderByDescending(o => o.DeliveryDate);

            if (sort.StatusSort != Bool.NULL)
                query = sort.StatusSort == Bool.ASCENDING
                    ? query.OrderBy(o => o.Status)
                    : query.OrderByDescending(o => o.Status);

            return await Task.FromResult(query.ToList());
        }

        public async Task<Order?> GetByIdAsync(Guid id)
        {
            string cacheKey = $"{_cacheKeyPrefix}{id}";
            string fieldKey = id.ToString();

            var cachedOrder = await _redisDatabase.HashGetAsync(cacheKey, fieldKey);

            if (!cachedOrder.IsNullOrEmpty)
            {
                _logger.LogInformation("Fetched from CACHE.");
                return JsonSerializer.Deserialize<Order>(cachedOrder!);
            }

            _logger.LogInformation("Fetched from DB.");

            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _logger.LogInformation("Set to CACHE.");
                await _redisDatabase.HashSetAsync(
                    cacheKey,
                    fieldKey,
                    JsonSerializer.Serialize(order)
                );

                await _redisDatabase.KeyExpireAsync(cacheKey, _cacheExpiration);
            }

            return order;
        }

        public async Task CreateAsync(Order entity)
        {
            await _context.Orders.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Order entity)
        {
            if (!await _context.Orders.AnyAsync(o => o.OrderId == entity.OrderId))
                throw new InvalidOperationException();

            _context.Orders.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var order = await _context.Orders.FindAsync(id) ?? throw new KeyNotFoundException();

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }

        // =============== MERGE FUNCTIONS ====================



    }
}