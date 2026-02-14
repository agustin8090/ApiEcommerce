using ApiEcommerce.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ApiEcommerce.Models.DTOs;
using ApiEcommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;


namespace ApiEcommerce.Controllers
{
    [Authorize(Roles ="Admin")]     
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersionNeutral]
    [ApiController]

    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository productRepository,ICategoryRepository categoryRepository, IMapper mapper)
        {
            _productRepository= productRepository;
            _categoryRepository= categoryRepository;

            _mapper= mapper;
        }
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public IActionResult GetProducts()
        {
            var products = _productRepository.GetProducts();
            var productsDto= _mapper.Map<List<ProductDto>>(products);

            
            return Ok(productsDto);
        }

        [AllowAnonymous]
        [HttpGet("{id:int}", Name ="GetProduct")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public IActionResult GetProduct(int id)
        {
            var product = _productRepository.GetProduct(id);
            if( product== null)
            {
                
                return NotFound($"El producto con el id {id} no existe");
            }

            var productDto= _mapper.Map<ProductDto>(product);
            return Ok(productDto);
            

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateProduct([FromBody] CreateProductDto createProductDto)
        {
            if (createProductDto == null)
            {
                
                return BadRequest(ModelState);
            }
            if (_productRepository.ProductExists(createProductDto.Name))
            {
                
                ModelState.AddModelError("CustomError", "El Producto ya existe");
                return BadRequest(ModelState);
            }
             if (!_categoryRepository.CategoryExist(createProductDto.CategoryId))
            {
                
                ModelState.AddModelError("CustomError", $"La categoria con el {createProductDto.CategoryId} no existe");
                return BadRequest(ModelState);
            }

            var product= _mapper.Map<Product>(createProductDto);

            if(!_productRepository.CreateProduct(product))
            {
                ModelState.AddModelError("CustomError", $"Algo salio mal al guardar el registro {product.Name}");
                return StatusCode(500, ModelState);
            }
            var createdProduct= _productRepository.GetProduct(product.Id);
            var productDto= _mapper.Map<ProductDto>(createdProduct);

            return CreatedAtRoute("GetProduct", new {id=product.Id}, product);

        }

        [HttpGet("searchProductByCategory/{categoryId:int}", Name ="GetProductForCategory")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public IActionResult GetProductForCategory(int categoryId)
        {
            var products = _productRepository.GetProductForCategory(categoryId);
            if( products.Count == 0)
            {
                
                return NotFound($"Los productos con la categoria Id: {categoryId} no existen");
            }

            var productsDto= _mapper.Map<List<ProductDto>>(products);
            return Ok(productsDto);
            

        }

        [HttpPatch("buyProduct/{name}/{quantity:int}", Name ="BuyProduct")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public IActionResult BuyProduct(string name, int quantity)
        {
           if(string.IsNullOrEmpty(name) || quantity <= 0){
                
                return BadRequest("El nombre del producto o la cantidad no son validos");
            }
            var foundProduct= _productRepository.ProductExists(name);

            if (!foundProduct)
            {
                return NotFound($"el producto con el {name} no existe");
            }
            if (_productRepository.BuyProduct(name, quantity))
            {
                
                ModelState.AddModelError("CustomError", $"No se puedo comprar el producto {name} o la cantidad solicitada es mayor al stock disponible");
            }
            var units= quantity==1? "unidad" : "unidades";
            return Ok($"Se compró {quantity} {units} del producto {name}");

        }


        [HttpPut("{productID:int}", Name="UpdateProduct")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateProduct(int productID, [FromBody] UpdateProductDto updateProductDto)
        {
            if (updateProductDto == null)
            {
                
                return BadRequest(ModelState);
            }
            if (!_productRepository.ProductExists(productID))
            {
                
                ModelState.AddModelError("CustomError", "El Producto ya existe");
                return BadRequest(ModelState);
            }
             if (!_categoryRepository.CategoryExist(updateProductDto.CategoryId))
            {
                
                ModelState.AddModelError("CustomError", $"La categoria con el {updateProductDto.CategoryId} no existe");
                return BadRequest(ModelState);
            }

            var product= _mapper.Map<Product>(updateProductDto);
        product.Id= productID;

            if(!_productRepository.UpdateProduct(product))
            {
                ModelState.AddModelError("CustomError", $"Algo salio mal al actualizar el producto {product.Name}");
                return StatusCode(500, ModelState);
            }
            var createdProduct= _productRepository.GetProduct(product.Id);
            var productDto= _mapper.Map<ProductDto>(createdProduct);

            return NoContent();
        }



        [HttpDelete("{productId:int}", Name ="DeleteProduct")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public IActionResult DeleteProduct(int productId)
        {

            if (productId == 0)
            {
                
                return BadRequest(ModelState);
            }
            var product = _productRepository.GetProduct(productId);
            if( product== null)
            {
                
                return NotFound($"El producto con el id {productId} no existe");
            }            
            
            
            if (!_productRepository.DeleteProduct(product))
            {
                ModelState.AddModelError("CustomError", $"Algo salio mal al eliminar el producto {product.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }

    }
}
