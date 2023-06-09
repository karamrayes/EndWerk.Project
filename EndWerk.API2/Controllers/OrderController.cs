using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EndWerk.Project.Data;
using Order.Object;
using EndWerk.API2.Dto;
using Order.Services;

namespace EndWerk.API2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly Repository _context;

        private OrderService _orderService { get; set; }

        private ProductService _productService { get; set; }

        private OrderDetailsService _orderDetailsService { get; set; }
        public OrderController(Repository context , OrderService orderService , ProductService productService ,OrderDetailsService orderDetailsService)
        {
            _context = context;
            _orderService = orderService;
            this._productService = productService;
            _orderDetailsService = orderDetailsService;

        }

        // GET: api/Order
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order.Object.Order>>> GetOrders()
        {
          if (_context.Orders == null)
          {
              return NotFound();
          }
            //return await _context.Orders.ToListAsync();
            return _orderService.GetOrders();
        }

        // GET: api/Order/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order.Object.Order>> GetOrder(int id)
        {
          if (_context.Orders == null)
          {
              return NotFound();
          }
            //var order = await _context.Orders.FindAsync(id);
            var order = _orderService.GetOrder(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Order/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutOrder(int id, Order.Object.Order order)
        //{
        //    if (id != order.OrderId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(order).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!OrderExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/Order
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order.Object.Order>> PostOrder(OrderDTO order)
        {
          if (_context.Orders == null)
          {
              return Problem("Entity set 'Repository.Orders'  is null.");
          }
            

            var orderX = new Order.Object.Order() {

                OrderAmount = 0,
                ShipDate = order.ShipDate,
                OrderDate = order.OrderDate,
                Shipped = order.Shipped,
                PaymentRecevied= order.PaymentRecevied,
                UserId = order.UserId,
                           
            };

            _orderService.UpdateOrCreateOrder(orderX);
           
            var list = new List<OrderDetail>();

            foreach (var item in order.Products)
            {
                var UnitPrice = _productService.GetProduct(item.ProductId).ProductPrice;

                var orderdetail = new Order.Object.OrderDetail() { ProductId = item.ProductId, Quantity = item.Quantity, UnitPrice = UnitPrice };

                orderdetail.OrderId = orderX.OrderId;

                _orderDetailsService.UpdateOrCreateOrderDetails(orderdetail);
                list.Add(orderdetail);
            }
            orderX.OrderAmount = _orderService.CalculateOrderAmount(list);
            
            
            
            return CreatedAtAction("GetOrder", new { id = orderX.OrderId }, orderX);
        }

        // DELETE: api/Order/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return (_context.Orders?.Any(e => e.OrderId == id)).GetValueOrDefault();
        }
    }
}
