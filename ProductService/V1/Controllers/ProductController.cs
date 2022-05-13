using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.Api.V1.Models.Products.Requests;
using ProductService.Api.V1.Models.Products.Responses;
using ProductService.Domain.Application.Commands;
using ProductService.Domain.Application.Queries;
using ProductService.Domain.Interfaces.Repository;
using Swashbuckle.AspNetCore.Annotations;

namespace ProductService.V1.Controllers
{
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IProductRepository _productRepository;

        public ProductController(IMapper mapper, IMediator mediator, 
                                 IProductRepository productRepository)
        {
            _mapper = mapper;
            _mediator = mediator;
            _productRepository = productRepository;
        }

        [HttpGet]
        [Route("products")]
        public ActionResult GetAll()
        {
            var response = _productRepository.GetAll();
            return Ok(response);
        }

        /// <summary>
        /// Retrieves a product according to productId requested.
        /// </summary>
        /// <param name="productId">Input parameter is productId</param>
        /// <returns><see cref="ProductDetailResponse"/></returns>
        [HttpGet]
        [Route("products/{productId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDetailResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status200OK, "Product retrieved correctly.", typeof(ProductDetailResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Product id was not found")]
        public async Task<ActionResult> GetById(int productId)
        {
            var request = new GetProductRequest { Id = productId };
            
            var response = await _mediator.Send(request);

            if(response == null)
            {
                return NotFound();
            }
            
            return Ok(_mapper.Map<ProductDetailResponse>(response));
        }

        /// <summary>
        /// Retrieves a productId when case is created successfully
        /// </summary>
        /// <param name="request">
        /// Input parameter is an CreateProductRequest entity with name, statusId, stock
        /// description and price.
        /// </param>
        /// <returns><see cref="int"/></returns>
        [HttpPost]
        [Route("products")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status201Created, "Product created correctly", typeof(int))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Request is invalid")]
        public async Task<ActionResult> CreateProduct([FromBody] CreateProductRequest request)
        {
            if (request == null || !ModelState.IsValid)
            {
                return BadRequest("Request is invalid");
            }

            var command = _mapper.Map<CreateProductCommand>(request);

            var response = await _mediator.Send(command);

            return CreatedAtAction("", new { id = response });
        }

        /// <summary>
        /// Retrieves a NoContent response in case product was updated.
        /// </summary>
        /// <param name="productId">Input parameter is productId</param>
        /// <param name="request">Product information to be updated</param>
        /// <returns>No content response</returns>
        [HttpPut]
        [Route("products/{productId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        public ActionResult UpdateProduct([FromRoute] int productId, [FromBody] UpdateProductRequest request)
        {
            var command = _mapper.Map<UpdateProductCommand>(request);
            command.Id = productId;

            _mediator.Send(command);

            return NoContent();
        }
    }
}
