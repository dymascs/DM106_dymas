using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using projeto_dm106.br.com.correios.ws;
using projeto_dm106.CRMClient;
using projeto_dm106.Data;
using projeto_dm106.Models;

namespace projeto_dm106.Controllers
{
    [Authorize]
    [RoutePrefix("api/orders")]
    public class OrdersController : ApiController
    {
        private projeto_dm106Context db = new projeto_dm106Context();

        [ResponseType(typeof(string))]
        [HttpGet]
        [Route("cep")]
        public IHttpActionResult ObtemCEP()
        {
            CRMRestClient crmClient = new CRMRestClient();
            Customer customer = crmClient.GetCustomerByEmail(User.Identity.Name);

            if (customer != null)
            {
                return Ok(customer.zip);
            }
            else
            {
                return BadRequest("Falha ao consultar o CRM");
            }
        }


        [ResponseType(typeof(string))]
        [HttpGet]
        [Route("shipping")]
        public IHttpActionResult CalcShipping(int id)
        {
            Order order = db.Orders.Find(id);
            CRMRestClient crmClient = new CRMRestClient();
            Customer customer = crmClient.GetCustomerByEmail(order.userName);
            
            
            string frete;
            CalcPrecoPrazoWS correios = new CalcPrecoPrazoWS();
            cResultado resultado = correios.CalcPrecoPrazo("", "", "04014", "37540000", customer.zip, "50", 1, 1, 1, 1, 1, "N", 100, "S"); ;
            if (resultado.Servicos[0].Erro.Equals("0"))
            {
                frete = "Valor do frete: " + resultado.Servicos[0].Valor + " - Prazo de entrega: " + resultado.Servicos[0].PrazoEntrega + " dia(s)";
                return Ok(frete);
            }
            else
            {
                return BadRequest("Código do erro: " + resultado.Servicos[0].Erro + "-" + resultado.Servicos[0].MsgErro);
            }
        }

        // GET: api/Orders
        [Authorize(Roles = "ADMIN")]
        public List<Order> GetOrders()
        {
            return db.Orders.Include(order => order.OrderItems).ToList();
        }

        [ResponseType(typeof(List<Order>))]
        [HttpGet]
        [Route("usersearch")]
        // GET: api/Orders
        public List<Order> GetOrdersSearchUser(string username)
        {
            if (User.IsInRole("ADMIN") || User.Identity.Name.Equals(username))
            {
                return db.Orders.Where(p => p.userName == username).ToList();
            }

            return null;           
        }


        // GET: api/Orders/5
        [ResponseType(typeof(Order))]
        public IHttpActionResult GetOrder(int id)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return BadRequest("pedido não encontrado!");
            }

            

            return Ok(order);
        }

        // PUT: api/Orders/5
        /*[ResponseType(typeof(void))]
        public IHttpActionResult PutOrder(int id, Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order.Id)
            {
                return BadRequest();
            }

            db.Entry(order).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }*/

        // POST: api/Orders
        [ResponseType(typeof(Order))]
        public IHttpActionResult PostOrder(Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            /*if (!User.IsInRole("ADMIN") && !User.Identity.Name.Equals(order.userName))
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }*/


            if (User.IsInRole("ADMIN"))
            {
                if (order.userName == null || order.userName.Length == 0)
                {
                    return BadRequest();
                }
            }
            else
            {
                if (order.userName != null)
                {
                    return BadRequest();
                }

                order.userName = User.Identity.Name;

            }

            
            order.status = "novo";
            order.pesoTotal = 0;
            order.precoFrete = 0;
            order.precoTotal = 0;
            order.dataPostada = DateTime.Now.ToString();

            db.Orders.Add(order);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = order.Id }, order);
        }

        // DELETE: api/Orders/5
        [ResponseType(typeof(Order))]
        public IHttpActionResult DeleteOrder(int id)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("ADMIN") && !User.Identity.Name.Equals(order.userName))
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }

            db.Orders.Remove(order);
            db.SaveChanges();

            return Ok(order);
        }

        [ResponseType(typeof(Order))]
        [HttpPost]
        [Route("finish")]
        public IHttpActionResult FinishOrder(int id)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("ADMIN") && !User.Identity.Name.Equals(order.userName))
            {
                return BadRequest("usuario não cardastrado.");
            }

            if (order.precoFrete == 0)
            {
                return BadRequest("Frete do pedido está nulo.");
            }

            if (order.status.Equals("fechado"))
            {
                return BadRequest("Finalizado.");
            }


            order.status = "fechado";


            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();

            return Ok(order);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderExists(int id)
        {
            return db.Orders.Count(e => e.Id == id) > 0;
        }
    }
}