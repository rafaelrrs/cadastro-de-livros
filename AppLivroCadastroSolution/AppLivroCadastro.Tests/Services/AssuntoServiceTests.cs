using AppLivroCadastro.Application.DTOs;
using AppLivroCadastro.Application.Services;
using AppLivroCadastro.Domain.Entities;
using AppLivroCadastro.Infrastructure.Persistence;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace AppLivroCadastro.Tests.Services
{
    public class AssuntoServiceTests
    {
        private readonly Mock<AppDbContext> _dbContextMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<AssuntoService>> _loggerMock;
        private readonly AssuntoService _assuntoService;

        public AssuntoServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase") 
            .Options;
            _dbContextMock = new Mock<AppDbContext>(options);
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<AssuntoService>>();
            _assuntoService = new AssuntoService(_dbContextMock.Object, _mapperMock.Object, _loggerMock.Object);
        }
        private static Mock<DbSet<T>> CreateMockDbSet<T>(IEnumerable<T> data) where T : class
        {
            var queryable = data.AsQueryable();
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
            mockSet.Setup(d => d.AsQueryable()).Returns(queryable);
            return mockSet;
        }

        [Fact]
        public async Task GetAllAsync_ReturnsListOfAssuntoDTOs()
        {
            // Arrange
            var assuntos = new List<Assunto>
        {
            new Assunto { CodAs = 1, Descricao = "Assunto 1" },
            new Assunto { CodAs = 2, Descricao = "Assunto 2" }
        };

            var assuntoDTOs = new List<AssuntoDTO>
        {
            new AssuntoDTO { CodAs = 1, Descricao = "Assunto 1" },
            new AssuntoDTO { CodAs = 2, Descricao = "Assunto 2" }
        };

            var mockSet = CreateMockDbSet(assuntos);
            _dbContextMock.Setup(c => c.Assuntos).Returns(mockSet.Object);
            _mapperMock.Setup(m => m.Map<IEnumerable<AssuntoDTO>>(It.IsAny<IEnumerable<Assunto>>()))
                            .Returns(assuntoDTOs);
            
            // Act
            var result = await _assuntoService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<AssuntoDTO>>(result);
            Assert.Equal(2, result.Count());
            _dbContextMock.Verify(c => c.Assuntos, Times.Once); 
            _mapperMock.Verify(m => m.Map<IEnumerable<AssuntoDTO>>(It.IsAny<IEnumerable<Assunto>>()), Times.Once); 
            _loggerMock.VerifyNoOtherCalls();
        }

    }
}
