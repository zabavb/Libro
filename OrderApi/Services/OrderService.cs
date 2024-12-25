using AutoMapper;
using Library.Extensions;
using OrderApi.Models;

namespace OrderApi.Services
{
    internal class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<IOrderService> _logger;
        private string _message;

        public OrderService(IOrderRepository repository, IMapper mapper, ILogger<IOrderService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _message = string.Empty;
        }

        public async Task<PaginatedResult<OrderDto>> GetOrdersAsync(int pageNumber, int pageSize, string searchTerm, Filter? filter)
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

        public async Task<OrderDto?> GetOrderByIdAsync(Guid orderId)
        {
            var order = await _repository.GetByIdAsync(orderId);

            if (order == null)
            {
                _message = $"Order with Id [{orderId}] not found.";
                _logger.LogError(_message);
                throw new KeyNotFoundException(_message);
            }

            _logger.LogInformation($"Order with Id [{orderId}] fetched succesfully.");

            return order == null ? null : _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto> CreateOrderAsync(OrderDto orderDto)
        {
            if(orderDto == null)
            {
                _message = "Order wasn't provided.";
                _logger.LogError(_message);
                throw new ArgumentNullException(_message,nameof(orderDto));
            }
            var order = _mapper.Map<Order>(orderDto);

            try
            {
                await _repository.AddAsync(order);
                _logger.LogInformation("Order created successfully.");
            }
            catch(Exception ex)
            {
                _message = "Error occured while adding an order.";
                _logger.LogError(_message);
                throw new InvalidOperationException(_message, ex);
            }

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto?> UpdateOrderAsync(OrderDto orderDto)
        {
            if (orderDto == null)
            {
                _message = "Order was not provided for the update";
                _logger.LogError(_message);
                throw new ArgumentNullException(_message,nameof(orderDto));
            }

            var order = _mapper.Map<Order>(orderDto);
            try
            {
                await _repository.UpdateAsync(order);
                _logger.LogInformation("Order updated succesfully.");
            }
            catch (InvalidOperationException)
            {
                _message = $"Order with Id {orderDto.Id} not found for update.";
                _logger.LogError(_message);
                throw new KeyNotFoundException(_message);
            }
            catch(Exception ex)
            {
                _message = $"Error occured while updating the order with Id [{orderDto.Id}]";
                _logger.LogError(_message);
                throw new InvalidOperationException(_message, ex);
            }

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<bool> DeleteOrderAsync(Guid id)
        {
            var order = await _repository.GetByIdAsync(id);

            if (order == null)
            {
                _message = $"Order with Id [{id}] not found.";
                _logger.LogError(_message);
                throw new KeyNotFoundException(_message);
            }

            try
            {
                await _repository.DeleteAsync(order);
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
            return true;
        }
    }
}
