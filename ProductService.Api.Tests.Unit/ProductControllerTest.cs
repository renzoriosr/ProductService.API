using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductService.Api.V1.Models.Products.Requests;
using ProductService.Api.V1.Models.Products.Responses;
using ProductService.Domain.Application.Commands;
using ProductService.Domain.Application.Queries;
using ProductService.Domain.Models;
using ProductService.V1.Controllers;
using ProductService.V1.Mappings;
using System.Threading.Tasks;
using Xunit;

namespace ProductService.Api.Tests.Unit
{
    public class ProductControllerTest
    {
        private readonly Mock<IMediator> _mockedMediator;
        private readonly IMapper _mockedMapper;

        public ProductControllerTest()
        {
            _mockedMediator = new Mock<IMediator>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ApiProductMappingProfile());
            });

            _mockedMapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task GetById_GivenExistanId_ShouldReturnOkResult()
        {
            //Arrange
            var productId = 1;

            var mockedProduct = new Product()
            {
                Id = productId,
                Name = "laptop",
                Description = "laptop asus",
                StatusId = 1,
                Price = 150,
                Stock = 50
            };

            _mockedMediator.Setup(s => s.Send(It.IsAny<GetProductRequest>(), new System.Threading.CancellationToken()))
                .Returns(Task.FromResult(mockedProduct));

            var controller = new ProductController(_mockedMapper, _mockedMediator.Object);

            //Act
            var result = await controller.GetById(productId);


            //Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetById_GivenExistanId_ShouldReturnNotFound()
        {
            //Arrange
            var productId = 4;

            _mockedMediator.Setup(s => s.Send(It.IsAny<GetProductRequest>(), new System.Threading.CancellationToken()))
                .Returns(Task.FromResult<Product>(null));

            var controller = new ProductController(_mockedMapper, _mockedMediator.Object);

            //Act
            var result = await controller.GetById(productId);


            //Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateProduct_GivenValidRequest_ShouldReturnCreatedAtAction()
        {
            //Arrange
            var product = new CreateProductRequest()
            {
                Name = "watch",
                Stock = 180,
                Price = 1800,
                StatusId = 0,
                Description = "apple smart watch 5"
            };

            _mockedMediator.Setup(s => s.Send(It.IsAny<CreateProductCommand>(), new System.Threading.CancellationToken()))
                .Returns(Task.FromResult<int>(5));

            var controller = new ProductController(_mockedMapper, _mockedMediator.Object);

            //Act
            var result = await controller.CreateProduct(product);


            //Assert
            Assert.IsType<CreatedAtActionResult>(result);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task UpdateProduct_GivenValidRequest_ShouldReturnNoContentResponse()
        {
            //Arrange
            var productId = 7;

            var product = new UpdateProductRequest()
            {
                Name = "watch",
                Stock = 180,
                Price = 1800,
                StatusId = 0,
                Description = "apple smart watch 5"
            };

            _mockedMediator.Setup(s => s.Send(It.IsAny<UpdateProductCommand>(), new System.Threading.CancellationToken())); ;

            var controller = new ProductController(_mockedMapper, _mockedMediator.Object);

            //Act
            var result = controller.UpdateProduct(productId, product);


            //Assert
            Assert.IsType<NoContentResult>(result);
            Assert.NotNull(result);
        }
    }
}
