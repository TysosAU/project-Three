using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace API.Controllers
{
    [Authorize(Roles = "admin")]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController1 : Controller
    {
        private readonly IMongoCollection<Product> _productCollection;

        public ProductsController1(IMongoDatabase database)
        {
            _productCollection = database.GetCollection<Product>("Products");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult?> GetAllProducts()
        {
            var products = await _productCollection.Find(_ => true).ToListAsync();
            if (products == null || products.Count == 0)
            {
                return NotFound(new { message = "No products found." });
            }
            return Ok(products);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult?> GetProduct(int id)
        {
            var product = await _productCollection.Find(p => p._id_ == id).FirstOrDefaultAsync();
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [AllowAnonymous]
        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult?> GetProductsByCategory(int categoryId)
        {
            var products = await _productCollection.Find(p => p.CategoryId == categoryId).ToListAsync();
            if (products == null || products.Count == 0)
            {
                return NotFound();
            }
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult?> CreateProduct([FromBody] Product product)
        {
            await _productCollection.InsertOneAsync(product);
            return CreatedAtAction(nameof(GetProduct), new { id = product._id_ }, product);
        }

        [AllowAnonymous]
        [HttpGet("search")]
        public async Task<IActionResult?> GetProductsByIds([FromQuery] List<int> ids)
        {
            var filter = Builders<Product>.Filter.In(p => p._id_, ids);
            var products = await _productCollection.Find(filter).ToListAsync();
            if (products == null || products.Count == 0)
            {
                return NotFound();
            }
            return Ok(products);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult?> DeleteProduct(int id)
        {
            var result = await _productCollection.DeleteOneAsync(p => p._id_ == id);
            if (result.DeletedCount == 0)
            {
                return NotFound();
            }
            return Ok(new { message = "Product Deleted Successfully." });
        }

        [HttpDelete("Multiple")]
        public async Task<IActionResult?> DeleteProducts([FromQuery] List<int> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return BadRequest(new { message = "No product IDs provided." });
            }

            var filter = Builders<Product>.Filter.In(p => p._id_, ids);
            var result = await _productCollection.DeleteManyAsync(filter);

            if (result.DeletedCount == 0)
            {
                return NotFound(new { message = "No matching products found to delete." });
            }

            return Ok(new { message = $"{result.DeletedCount} product(s) deleted successfully." });
        }

        [AllowAnonymous]
        [HttpGet("AdvancedSearch")]
        public async Task<IActionResult?> GetAllProducts(
        [FromQuery] string? brand = null,
        [FromQuery] string? model = null,
        [FromQuery] int? year = null,
        [FromQuery] int? salePriceMin = null,
        [FromQuery] int? salePriceMax = null,
        [FromQuery] bool? onSale = null,
        [FromQuery] int? engineSize = null,
        [FromQuery] int? categoryId = null,
        [FromQuery] string? sortField = null,
        [FromQuery] string sortDirection = "asc")
        {
            var filterBuilder = Builders<Product>.Filter;
            var filter = filterBuilder.Empty;


            if (!string.IsNullOrEmpty(brand))
            {
                filter &= filterBuilder.Regex(p => p.Brand, new BsonRegularExpression(brand, "i"));
            }
            if (!string.IsNullOrEmpty(model))
            {
                filter &= filterBuilder.Regex(p => p.Model, new BsonRegularExpression(model, "i"));
            }
            if (year.HasValue)
            {
                filter &= filterBuilder.Eq(p => p.Year, year.Value);
            }
            if (salePriceMin.HasValue)
            {
                filter &= filterBuilder.Gte(p => p.SalePrice, salePriceMin.Value);
            }
            if (salePriceMax.HasValue)
            {
                filter &= filterBuilder.Lte(p => p.SalePrice, salePriceMax.Value);
            }
            if (onSale.HasValue)
            {
                filter &= filterBuilder.Eq(p => p.onSale, onSale.Value);
            }
            if (engineSize.HasValue)
            {
                filter &= filterBuilder.Eq(p => p.engineSize, engineSize.Value);
            }
            if (categoryId.HasValue)
            {
                filter &= filterBuilder.Eq(p => p.CategoryId, categoryId.Value);
            }

            var products = await _productCollection.Find(filter).ToListAsync();
            if (products == null || products.Count == 0)
            {
                return NotFound(new { message = "No products found." });
            }

            if (!string.IsNullOrEmpty(sortField))
            {
                products = sortField switch
                {
                    "ID" => sortDirection == "desc"
                                ? products.OrderByDescending(p => p._id_).ToList()
                                : products.OrderBy(p => p._id_).ToList(),
                    "Brand" => sortDirection == "desc"
                                ? products.OrderByDescending(p => p.Brand.ToLower()).ToList()
                                : products.OrderBy(p => p.Brand.ToLower()).ToList(),
                    "Model" => sortDirection == "desc"
                                ? products.OrderByDescending(p => p.Model.ToLower()).ToList()
                                : products.OrderBy(p => p.Model.ToLower()).ToList(),
                    "Year" => sortDirection == "desc"
                                ? products.OrderByDescending(p => p.Year).ToList()
                                : products.OrderBy(p => p.Year).ToList(),
                    "SalePrice" => sortDirection == "desc"
                                    ? products.OrderByDescending(p => p.SalePrice).ToList()
                                    : products.OrderBy(p => p.SalePrice).ToList(),
                    "onSale" => sortDirection == "desc"
                                ? products.OrderByDescending(p => p.onSale).ToList()
                                : products.OrderBy(p => p.onSale).ToList(),
                    "engineSize" => sortDirection == "desc"
                                    ? products.OrderByDescending(p => p.engineSize).ToList()
                                    : products.OrderBy(p => p.engineSize).ToList(),
                    "CategoryId" => sortDirection == "desc"
                                    ? products.OrderByDescending(p => p.CategoryId).ToList()
                                    : products.OrderBy(p => p.CategoryId).ToList(),
                    _ => products
                };
            }

            return Ok(products);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product updatedProduct)
        {
            if (updatedProduct == null)
            {
                return BadRequest("Product data is required.");
            }
            updatedProduct._id_ = id;
            var result = await _productCollection.ReplaceOneAsync(
                p => p._id_ == id,
                updatedProduct);
            if (result.MatchedCount == 0)
            {
                return NotFound("Product not found.");
            }
            return Ok("Product Updated!");
        }
    }

    [Authorize(Roles = "admin")]
    [ApiVersion("2.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController2 : Controller
    {
        private readonly IMongoCollection<Product> _productCollection;

        public ProductsController2(IMongoDatabase database)
        {
            _productCollection = database.GetCollection<Product>("Products");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult?> GetAllProductsOnSale()
        {
            var products = await _productCollection.Find(product => product.onSale == true).ToListAsync();
            if (products == null || products.Count == 0)
            {
                return NotFound(new { message = "No products that are on sale found." });
            }
            return Ok(products);
        }

        [AllowAnonymous]
        [HttpGet("AdvancedSearch")]
        public async Task<IActionResult?> GetProductsOnSale(
        [FromQuery] string? brand = null,
        [FromQuery] string? model = null,
        [FromQuery] int? year = null,
        [FromQuery] int? salePriceMin = null,
        [FromQuery] int? salePriceMax = null,
        [FromQuery] int? engineSize = null,
        [FromQuery] int? categoryId = null,
        [FromQuery] string? sortField = null,
        [FromQuery] string sortDirection = "asc")
        {
            var filterBuilder = Builders<Product>.Filter;
            var filter = filterBuilder.Eq(p => p.onSale, true);

            if (!string.IsNullOrEmpty(brand))
            {
                filter &= filterBuilder.Regex(p => p.Brand, new BsonRegularExpression(brand, "i"));
            }
            if (!string.IsNullOrEmpty(model))
            {
                filter &= filterBuilder.Regex(p => p.Model, new BsonRegularExpression(model, "i"));
            }
            if (year.HasValue)
            {
                filter &= filterBuilder.Eq(p => p.Year, year.Value);
            }
            if (salePriceMin.HasValue)
            {
                filter &= filterBuilder.Gte(p => p.SalePrice, salePriceMin.Value);
            }
            if (salePriceMax.HasValue)
            {
                filter &= filterBuilder.Lte(p => p.SalePrice, salePriceMax.Value);
            }
            if (engineSize.HasValue)
            {
                filter &= filterBuilder.Eq(p => p.engineSize, engineSize.Value);
            }
            if (categoryId.HasValue)
            {
                filter &= filterBuilder.Eq(p => p.CategoryId, categoryId.Value);
            }

            var products = await _productCollection.Find(filter).ToListAsync();
            if (products == null || products.Count == 0)
            {
                return NotFound(new { message = "No products found." });
            }

            if (!string.IsNullOrEmpty(sortField))
            {
                products = sortField switch
                {
                    "ID" => sortDirection == "desc"
                                ? products.OrderByDescending(p => p._id_).ToList()
                                : products.OrderBy(p => p._id_).ToList(),
                    "Brand" => sortDirection == "desc"
                                ? products.OrderByDescending(p => p.Brand.ToLower()).ToList()
                                : products.OrderBy(p => p.Brand.ToLower()).ToList(),
                    "Model" => sortDirection == "desc"
                                ? products.OrderByDescending(p => p.Model.ToLower()).ToList()
                                : products.OrderBy(p => p.Model.ToLower()).ToList(),
                    "Year" => sortDirection == "desc"
                                ? products.OrderByDescending(p => p.Year).ToList()
                                : products.OrderBy(p => p.Year).ToList(),
                    "SalePrice" => sortDirection == "desc"
                                    ? products.OrderByDescending(p => p.SalePrice).ToList()
                                    : products.OrderBy(p => p.SalePrice).ToList(),
                    "engineSize" => sortDirection == "desc"
                                    ? products.OrderByDescending(p => p.engineSize).ToList()
                                    : products.OrderBy(p => p.engineSize).ToList(),
                    "CategoryId" => sortDirection == "desc"
                                    ? products.OrderByDescending(p => p.CategoryId).ToList()
                                    : products.OrderBy(p => p.CategoryId).ToList(),
                    _ => products
                };
            }

            return Ok(products);
        }

    }
}