using AutoMapper;
using Library.Common;
using Library.DTOs.UserRelated.User;
using OrderApi.Models;

namespace OrderApi.Services
{
    public class OrderService(
        IOrderRepository repository,
        IMapper mapper,
        ILogger<IOrderService> logger) : IOrderService
    {
        private readonly IOrderRepository _repository = repository;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<IOrderService> _logger = logger;
        private string _message = string.Empty;

        public async Task<PaginatedResult<OrderDto>> GetAllAsync(int pageNumber, int pageSize, string searchTerm,
            Filter? filter, Sort? sort)
        {
            var paginatedOrders =
                await _repository.GetAllAsync(pageNumber, pageSize, searchTerm, filter, sort);

            if (paginatedOrders == null || paginatedOrders.Items == null)
            {
                _message = "Failed to fetch paginated orders.";
                _logger.LogError(_message);
                throw new InvalidOperationException(_message);
            }

            _logger.LogInformation("Successfully fetched [{Count}] orders.", paginatedOrders.Items.Count);

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
                _message = $"Order with ID [{id}] not found.";
                _logger.LogError(_message);
                throw new KeyNotFoundException(_message);
            }

            _logger.LogInformation($"Order with ID [{id}] successfully fetched.");

            return order == null ? null : _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderForUserCard?> GetForUserCardAsync(Guid id)
        {
            try
            {
                return await _repository.GetForUserCardAsync(id);
            }
            catch
            {
                return null;
            }
        }

        public async Task<ICollection<OrderForUserDetails>?> GetAllForUserDetailsAsync(Guid id)
        {
            try
            {
                return await _repository.GetAllForUserDetailsAsync(id);
            }
            catch
            {
                return null;
            }
        }

        public async Task CreateAsync(OrderDto entity)
        {
            if (entity == null)
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
                _logger.LogInformation("Order successfully created.");
            }
            catch (Exception ex)
            {
                _message = $"Error occurred while adding the order with ID [{entity.Id}].";
                _logger.LogError(_message);
                throw new InvalidOperationException(_message, ex);
            }
        }

        public async Task UpdateAsync(OrderDto entity)
        {
            if (entity == null)
            {
                _message = "Order was not provided for update.";
                _logger.LogError(_message);
                throw new ArgumentNullException(null, _message);
            }

            var order = _mapper.Map<Order>(entity);
            try
            {
                await _repository.UpdateAsync(order);
                _logger.LogInformation($"Order with ID [{entity.Id}] successfully updated.");
            }
            catch (InvalidOperationException)
            {
                _message = $"Order with ID [{entity.Id}] not found for update.";
                _logger.LogError(_message);
                throw new KeyNotFoundException(_message);
            }
            catch (Exception ex)
            {
                _message = $"Error occurred while updating the order with ID [{entity.Id}].";
                _logger.LogError(_message);
                throw new InvalidOperationException(_message, ex);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                await _repository.DeleteAsync(id);
                _logger.LogInformation($"Order with ID [{id}] successfully deleted.");
            }
            catch (KeyNotFoundException)
            {
                _message = $"Order with ID [{id}] not found for deletion.";
                _logger.LogError(_message);
                throw new KeyNotFoundException(_message);
            }
            catch (Exception ex)
            {
                _message = $"Error occurred while deleting the order with ID [{id}].";
                _logger.LogError(_message);
                throw new InvalidOperationException(_message, ex);
            }
        }
    }
}