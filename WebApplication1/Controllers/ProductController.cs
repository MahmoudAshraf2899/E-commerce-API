using HPlusSport.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Classes;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiVersion("1.0")]
    [Route("[controller]")]
    [ApiController]
    public class ProductV1_0Controller : ControllerBase
    {
        private readonly ShopContext _context;

        public ProductV1_0Controller(ShopContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        //GetProducts Method by using IActionResults
        [HttpGet]
        public async Task<IActionResult> getProducts([FromQuery] ProductQueryParameters queryParam)
        {           
            IQueryable<Product> products = _context.Products;
            if (queryParam.MinPrice != null &&
               queryParam.MaxPrice != null)
            {
                products=
                         products.Where(p => p.Price >= queryParam.MinPrice.Value &&
                                             p.Price <= queryParam.MaxPrice.Value);
            }
            if (!string.IsNullOrEmpty(queryParam.SearchTerm))
            {
                products = products.Where(p => p.Sku.ToLower().Contains(queryParam.SearchTerm.ToLower()) ||
                                               p.Name.ToLower().Contains(queryParam.SearchTerm.ToLower()));
            }
            if (!string.IsNullOrEmpty(queryParam.Sku))
            {
                products = products.Where(p => p.Sku == queryParam.Sku);    
            }
            if (!string.IsNullOrEmpty(queryParam.Name))
            {
                products = products.Where(p => p.Name.ToLower().Contains(queryParam.Name.ToLower()));
            }
            if (!string.IsNullOrEmpty(queryParam.SortBy))
            {
                if (typeof(Product).GetProperty(queryParam.SortBy) != null)
                {
                    products = products.OrderByCustom(queryParam.SortBy, queryParam.SortOrder);
                }
            }
            products = products
                .Skip(queryParam.Size * (queryParam.Page - 1))
                .Take(queryParam.Size);
            return Ok(await products.ToArrayAsync());
        }

        //Get Element By Find Id and Handling Error by mark the tybe of parameter and make exception
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct (int id)
        {
            var product = await _context.Products.FindAsync(id);
            if(product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        //Add Products using Post Verb
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct([FromBody] Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                "GetProduct",
                new { id = product.Id },
                product
            );
        }

        //Update Products using Put Verb
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct([FromRoute] int id, [FromBody] Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }
            _context.Entry(product).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_context.Products.Find(id) == null)
                {
                    return NotFound();
                }
                throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();            
            return product;
        }
    }

      /*
       Second Controller For Version
       */

    [ApiVersion("2.0")]
    [Route("[controller]")]
    [ApiController]
    public class ProductV2_0Controller : ControllerBase
    {
        private readonly ShopContext _context;

        public ProductV2_0Controller(ShopContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }


        //GetProducts Method by using IActionResults
        [HttpGet]
        public async Task<IActionResult> getProducts([FromQuery] ProductQueryParameters queryParam)
        {
            IQueryable<Product> products = _context.Products.Where(p =>p.IsAvailable == true);
            if (queryParam.MinPrice != null &&
               queryParam.MaxPrice != null)
            {
                products =
                         products.Where(p => p.Price >= queryParam.MinPrice.Value &&
                                             p.Price <= queryParam.MaxPrice.Value);
            }
            if (!string.IsNullOrEmpty(queryParam.SearchTerm))
            {
                products = products.Where(p => p.Sku.ToLower().Contains(queryParam.SearchTerm.ToLower()) ||
                                               p.Name.ToLower().Contains(queryParam.SearchTerm.ToLower()));
            }
            if (!string.IsNullOrEmpty(queryParam.Sku))
            {
                products = products.Where(p => p.Sku == queryParam.Sku);
            }
            if (!string.IsNullOrEmpty(queryParam.Name))
            {
                products = products.Where(p => p.Name.ToLower().Contains(queryParam.Name.ToLower()));
            }
            if (!string.IsNullOrEmpty(queryParam.SortBy))
            {
                if (typeof(Product).GetProperty(queryParam.SortBy) != null)
                {
                    products = products.OrderByCustom(queryParam.SortBy, queryParam.SortOrder);
                }
            }
            products = products
                .Skip(queryParam.Size * (queryParam.Page - 1))
                .Take(queryParam.Size);
            return Ok(await products.ToArrayAsync());
        }

        //Get Element By Find Id and Handling Error by mark the tybe of parameter and make exception
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        //Add Products using Post Verb
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct([FromBody] Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                "GetProduct",
                new { id = product.Id },
                product
            );
        }

        //Update Products using Put Verb
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct([FromRoute] int id, [FromBody] Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }
            _context.Entry(product).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_context.Products.Find(id) == null)
                {
                    return NotFound();
                }
                throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return product;
        }
    }
}
