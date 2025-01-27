using Library.Extensions;
using Microsoft.EntityFrameworkCore;
using OrderApi.Data;
using StackExchange.Redis;
using System.Text.Json;
using Order = OrderApi.Models.Order;
namespace OrderApi.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _context;
        private readonly IDatabase _redisDatabase;
        private readonly string _cacheKeyPrefix;
        private readonly TimeSpan _cacheExpiration;
        private readonly ILogger<IOrderRepository> _logger;
        public OrderRepository(OrderDbContext context, IConnectionMultiplexer redis, ILogger<IOrderRepository> logger)
        {
            _context = context;
            _redisDatabase = redis.GetDatabase();

            _cacheKeyPrefix = "Order_";
            _cacheExpiration = TimeSpan.FromMinutes(10);

            _logger = logger;
        }

        public async Task<PaginatedResult<Order>> GetAllPaginatedAsync(int pageNumber, int pageSize, string searchTerm, Filter? filter)
        {
            IEnumerable<Order> orders;

            string cacheKey = $"{_cacheKeyPrefix}All";
            var cachedOrders = await _redisDatabase.StringGetAsync(cacheKey);
            if (!cachedOrders.IsNullOrEmpty)
            {
                orders = JsonSerializer.Deserialize<ICollection<Order>>(cachedOrders!)!;
                _logger.LogInformation("Fetched from CACHE.");
            }
            else
            {
                orders = _context.Orders.AsNoTracking();
                _logger.LogInformation("Fetched from DB.");

                await _redisDatabase.StringSetAsync(
                    cacheKey,
                    JsonSerializer.Serialize(orders),
                    _cacheExpiration);
                _logger.LogInformation("Set to CACHE.");
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
                orders = await SearchEntitiesAsync(searchTerm, orders);

            if (orders.Any() && filter != null)
                orders = await FilterEntitiesAsync(orders, filter);

            var totalOrders = await Task.FromResult(orders.Count());

            orders = await Task.FromResult(orders.Skip((pageNumber - 1) * pageSize).Take(pageSize));
            ICollection<Order> result = new List<Order>(orders);

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

            return await Task.FromResult(orders.ToList());
        }


        public async Task<Order?> GetByIdAsync(Guid id)
        {
            string cacheKey = $"{_cacheKeyPrefix}{id}";
            var cachedOrder = await _redisDatabase.StringGetAsync(cacheKey);

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
                await _redisDatabase.StringSetAsync(
                    cacheKey,
                    JsonSerializer.Serialize(order),
                    _cacheExpiration
                    );
            }

            return order;
        }

        public async Task CreateAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Order order)
        {
            if (!await _context.Orders.AnyAsync(o => o.OrderId == order.OrderId))
                throw new InvalidOperationException();

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var order = await _context.Orders.FindAsync(id) ?? throw new KeyNotFoundException();

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }

}
