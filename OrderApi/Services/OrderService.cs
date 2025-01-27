using AutoMapper;
using Library.Extensions;
using OrderApi.Models;

namespace OrderApi.Services
{
    public class OrderService(IOrderRepository repository, IMapper mapper, ILogger<IOrderService> logger) : IOrderService
    {
        private readonly IOrderRepository _repository = repository;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<IOrderService> _logger = logger;
        private string _message = string.Empty;

        public async Task<PaginatedResult<OrderDto>> GetAllAsync(int pageNumber, int pageSize, string searchTerm, Filter? filter)
        {
            var paginatedOrders = await _repository.GetAllPaginatedAsync(pageNumber, pageSize, searchTerm, filter);

            if (paginatedOrders == null || paginatedOrders.Items == null)
            {
                _message = "Failed to fetch paginated delivery types.";
                _logger.LogError(_message);
                throw new InvalidOperationException(_message);
            }

            _logger.LogInformation("Delivery types successfully fetched.");

            return new PaginatedResult<OrderDto>
            {
                Items = _mapper.Map<ICollection<OrderDto>>(paginatedOrders.Items),
                TotalCount = paginatedOrders.TotalCount,
                PageNumber = paginatedOrders.PageNumber,
                PageSize = paginatedOrders.PageSize
            };
        }

        public async Task<OrderDto?> GetByIdAsync(Guid id)
        {
            var order = await _repository.GetByIdAsync(id);

            if (order == null)
            {
                _message = $"Order with Id [{id}] not found.";
                _logger.LogError(_message);
                throw new KeyNotFoundException(_message);
            }

            _logger.LogInformation($"Order with Id [{id}] fetched succesfully.");

            return order == null ? null : _mapper.Map<OrderDto>(order);
        }

        public async Task CreateAsync(OrderDto entity)
        {
            if(entity == null)
            {
                _message = "Order was not provided for creation.";
                _logger.LogError(_message);
                throw new ArgumentNullException(null, _message);
            }
            var order = _mapper.Map<Order>(entity);

            try
            {
                order.OrderId = Guid.NewGuid();
                await _repository.CreateAsync(order);
                _logger.LogInformation("Order created successfully.");
            }
            catch(Exception ex)
            {
                _message = $"Error occured while adding an order with tID [{entity.Id}].";
                _logger.LogError(_message);
                throw new InvalidOperationException(_message, ex);
            }

        }

        public async Task UpdateAsync(OrderDto entity)
        {
            if (entity == null)
            {
                _message = "Order was not provided for the update";
                _logger.LogError(_message);
                throw new ArgumentNullException(null, _message);
            }

            var order = _mapper.Map<Order>(entity);
            try
            {
                await _repository.UpdateAsync(order);
                _logger.LogInformation($"Order with ID [{entity.Id}] updated succesfully.");
            }
            catch (InvalidOperationException)
            {
                _message = $"Order with Id {entity.Id} not found for update.";
                _logger.LogError(_message);
                throw new KeyNotFoundException(_message);
            }
            catch(Exception ex)
            {
                _message = $"Error occured while updating the order with Id [{entity.Id}]";
                _logger.LogError(_message);
                throw new InvalidOperationException(_message, ex);
            }

        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                await _repository.DeleteAsync(id);
                _logger.LogInformation($"Order with Id [{id}] deleted succesfully");
            }
            catch (InvalidOperationException)
            {
                _message = $"Order with Id [{id}] not found for deletion.";
                _logger.LogError(_message);
                throw new KeyNotFoundException(_message);
            }
            catch(Exception ex)
            {
                _message = $"Error occurred while deleting the order with Id [{id}].";
                _logger.LogError(_message);
                throw new InvalidOperationException(_message, ex);
            }
        }
    }
}
