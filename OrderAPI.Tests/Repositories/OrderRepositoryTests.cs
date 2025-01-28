using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using OrderApi.Data;
using OrderApi.Repository;
using StackExchange.Redis;

namespace OrderAPI.Tests.Repositories
{
    public class OrderRepositoryTests
    {
        private readonly OrderDbContext _dbContext;
        private readonly Mock<OrderDbContext> _contextMock;
        private readonly Mock<IConnectionMultiplexer> _redisMock;
        private readonly Mock<IDatabase> _redisDatabaseMock;
        private readonly Mock<ILogger<IOrderRepository>> _loggerMock;
        private readonly OrderRepository _repository;

        public OrderRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<OrderDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _dbContext = new OrderDbContext(options);

            _contextMock = new Mock<OrderDbContext>();
            _redisMock = new Mock<IConnectionMultiplexer>();
            _redisDatabaseMock = new Mock<IDatabase>();
            _loggerMock = new Mock<ILogger<IOrderRepository>>();

            _redisMock.Setup(r => r.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(_redisDatabaseMock.Object);

            _repository = new OrderRepository(_dbContext, _redisMock.Object, _loggerMock.Object);
        }
    }
}
