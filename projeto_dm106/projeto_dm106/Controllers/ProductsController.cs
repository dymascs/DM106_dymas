using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using projeto_dm106.Data;
using projeto_dm106.Models;

namespace projeto_dm106.Controllers
{
    [Authorize]
    [RoutePrefix("api/products")]
    public class ProductsController : ApiController
    {
        private projeto_dm106Context db = new projeto_dm106Context();

        [ResponseType(typeof(Product))]
        [HttpGet]
        [Route("byname")]
        public IHttpActionResult GetProductByName(string name)
        {
            var product = db.Products.Where(p => p.nome == name);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        

        // GET: api/Products
        public IQueryable<Product> GetProducts()
        {
            Trace.TraceInformation("Nome do usuário: " + User.Identity.Name);
            if (User.IsInRole("USER"))
            {
                Trace.TraceInformation("Usuário com papel USER");
            }
            else if (User.IsInRole("ADMIN"))
            {
                Trace.TraceInformation("Usuário com papel ADMIN");
            }
            return db.Products;
        }

        // GET: api/Products/5
        [ResponseType(typeof(Product))]
        public IHttpActionResult GetProduct(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/Products/5
        [Authorize(Roles = "ADMIN")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.Id)
            {
                return BadRequest();
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Products
        [Authorize(Roles = "ADMIN")]
        [ResponseType(typeof(Product))]
        public IHttpActionResult PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (db.Products.Count(e => e.modelo == product.modelo) > 0 || db.Products.Count(e => e.codigo == product.codigo) > 0)
            {
                return BadRequest("modelo ou codigo igual a algum existente.");
            }


            db.Products.Add(product);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [Authorize(Roles = "ADMIN")]
        [ResponseType(typeof(Product))]
        public IHttpActionResult DeleteProduct(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
            db.SaveChanges();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.Id == id) > 0;
        }


    }
}