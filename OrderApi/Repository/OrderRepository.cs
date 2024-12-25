using Library.Extensions;
using Microsoft.EntityFrameworkCore;
using OrderApi.Data;
using OrderApi.Models;

namespace OrderApi.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _context;
        private readonly ILogger<IOrderRepository> _logger;
        private string _message;
        public OrderRepository(OrderDbContext context, ILogger<IOrderRepository> logger)
        {
            _context = context;
            _logger = logger;
            _message = string.Empty;
        }

        public async Task<PaginatedResult<Order>> GetAllPaginatedAsync(int pageNumber, int pageSize, string searchTerm, Filter? filter)
        {
            IEnumerable<Order> orders;
            if (!string.IsNullOrWhiteSpace(searchTerm))
                orders = await SearchEntitiesAsync(searchTerm);
            else
                orders = await _context.Orders.AsNoTracking().ToListAsync();

            if (orders.Any() && filter != null)
                orders = await FilterEntitiesAsync(orders, filter);

            var totalOrders = await Task.FromResult(orders.Count());

            orders = await Task.FromResult(orders.Skip((pageNumber - 1) * pageSize).Take(pageSize));

            if (orders == null)
            {
                _message = "Failed to fetch delivery types";
                _logger.LogError(_message);
                throw new InvalidOperationException(_message);
            }
            else
                _logger.LogInformation("Successfully fetched delivery types");

            return new PaginatedResult<Order>
            {
                Items = (ICollection<Order>)orders,
                TotalCount = totalOrders,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<IEnumerable<Order>> SearchEntitiesAsync(string searchTerm)
        {
            var orders = await _context.Orders
                .AsNoTracking()
                .Where(o => o.Address.Contains(searchTerm) || 
                            o.Region.Contains(searchTerm) ||
                            o.City.Contains(searchTerm))
                .ToListAsync();
            return orders;
        }

        public async Task<IEnumerable<Order>> FilterEntitiesAsync(IEnumerable<Order> orders,Filter filter)
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
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == id);
            if (order == null)
            {
                _message = $"Order with Id [{id}] not found.";
                _logger.LogError(_message);
                throw new KeyNotFoundException(_message);
            }
            else
                _logger.LogInformation($"Order with Id [{id}] found.");
            return order == null ? null : order;
        }

        public async Task AddAsync(Order order)
        {
            if (order == null)
            {
                _message = "Order was not provided for creation";
                _logger.LogError (_message);
                throw new ArgumentNullException(_message, nameof(order));
            }
            try
            {
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Order created succesfully");
            }
            catch (ArgumentNullException ex)
            {
                _message = "Order entity cannot be null.";
                _logger.LogError(_message);
                throw new ArgumentException(_message, ex);
            }
            catch (Exception ex) {
                _message = "Error occured while adding the order to database.";
                _logger.LogError(_message);
                throw new InvalidOperationException(_message, ex);
            }
        }

        public async Task UpdateAsync(Order order)
        {
            if (order == null)
            {
                _message = "Order was not provided for update.";
                _logger.LogError (_message);
                throw new ArgumentNullException (_message, nameof(order));
            }

            if(!await _context.Orders.AnyAsync(o => o.OrderId == order.OrderId))
            {
                _message = $"Order with Id [{order.OrderId}] does not exist.";
                _logger.LogError (_message);
                throw new InvalidOperationException(_message);
            }

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Order with Id[{order.OrderId}] updated succesfully.");
        }

        public async Task DeleteAsync(Guid id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
                throw new KeyNotFoundException();

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }

}
