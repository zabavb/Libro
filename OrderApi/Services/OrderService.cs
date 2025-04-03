using AutoMapper;
using BookAPI;
using BookAPI.Repositories;
using BookAPI.Repositories.Interfaces;
using Library.Common;
using Library.DTOs.UserRelated.User;
using Library.Interfaces;
using Library.Sorts;
using Microsoft.EntityFrameworkCore;
using OrderApi.Models;
using OrderAPI;

namespace OrderApi.Services
{
    public class OrderService(IOrderRepository repository, IMapper mapper, ILogger<IOrderService> logger, IBookRepository bookRepository) : IOrderService
    {
        private readonly IOrderRepository _repository = repository;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<IOrderService> _logger = logger;
        private string _message = string.Empty;
        private readonly IBookRepository _bookRepository = bookRepository;
        public async Task<PaginatedResult<OrderDto>> GetAllAsync(int pageNumber, int pageSize, string searchTerm, Filter? filter, Sort? sort)
        {
            var paginatedOrders = await _repository.GetAllPaginatedAsync(pageNumber, pageSize, searchTerm, filter, sort);

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
                _logger.LogInformation("Order successfully created.");
            }
            catch(Exception ex)
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
            catch(Exception ex)
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
            catch(Exception ex)
            {
                _message = $"Error occurred while deleting the order with ID [{id}].";
                _logger.LogError(_message);
                throw new InvalidOperationException(_message, ex);
            }
        }

        public async Task<SingleSnippet<OrderCardSnippet>> GetCardSnippetByUserIdAsync(Guid id)
        {
            try
            {

                OrderFilter filter = new OrderFilter() { UserId = id };
                OrderSort sort = new OrderSort() { OrderDate = Bool.DESCENDING };

                PaginatedResult<Order> orders = await _repository.GetAllPaginatedAsync(GlobalConstants.DefaultPageNumber,1,"",filter,sort);



                var orderSnippet = new OrderCardSnippet();

                if (orders.TotalCount > 0)
                {
                    orderSnippet = new OrderCardSnippet
                    {
                        LastOrder = orders.Items.First().OrderId.ToString().Split('-')[4],
                        OrdersCount = orders.TotalCount
                    };
                }

                return new SingleSnippet<OrderCardSnippet>(false, orderSnippet);
            }
            catch
            {
                return new SingleSnippet<OrderCardSnippet>(true, new OrderCardSnippet());
            }
        }

        public async Task<CollectionSnippet<OrderDetailsSnippet>> GetAllByUserIdAsync(Guid id, int pageNumber)
        {
            try
            {
                var filter = new OrderFilter { UserId = id };

                var orders = await _repository.GetAllPaginatedAsync(pageNumber, GlobalConstants.DefaultPageSize, "", filter,null);
                var orderDetailsSnippets = new List<OrderDetailsSnippet>();

                foreach (var order in orders.Items)
                {
                    var bookNames = new List<string>();

                    foreach (var book in order.Books)
                    {
                        var bookObject = await _bookRepository.GetByIdAsync(book.Key);
                        if (bookObject != null)
                            bookNames.Add(bookObject.Title);
                    }

                    orderDetailsSnippets.Add(new OrderDetailsSnippet
                    {
                        OrderUiId = order.OrderId.ToString().Split('-')[4],
                        Price = order.Price + order.DeliveryPrice,
                        BookNames = bookNames
                    });
                }

                return new CollectionSnippet<OrderDetailsSnippet>(false, orderDetailsSnippets);
            }
            catch
            {
                return new CollectionSnippet<OrderDetailsSnippet>(true, new List<OrderDetailsSnippet>());
            }
        }
    }
}
