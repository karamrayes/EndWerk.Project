using EndWerk.Project.Data;
using Microsoft.EntityFrameworkCore;
using Order.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Services
{
    public class OrderDetailsService
    {
        private Repository _repository;

        public OrderDetailsService(Repository repository)
        {
            _repository = repository;
        }

        public List<OrderDetail> GetOrderDetails()
        {
            //return _repository.Order.ToList();
            return _repository.OrderDetails.Include(o => o.Order).Include(u => u.Product).ThenInclude(pc => pc.ProductCategory)
                         .ToList();

        }

        public OrderDetail GetOrderDetail(int id)
        {
            return _repository.OrderDetails.FirstOrDefault(c => c.OrderDetailId == id);

        }

        public OrderDetail UpdateOrCreateOrderDetails(OrderDetail orderdetails)
        {

            //insert
            try
            {
                
                if (orderdetails.OrderDetailId == 0)
                {
                    // Insert new OrderDetails record
                    //_repository.OrderDetails.Add(orderdetails);
                    _repository.OrderDetails.Add(orderdetails);
                }
                else
                {
                    //update
                    _repository.OrderDetails.Attach(orderdetails);
                    var entry = _repository.ChangeTracker.Entries().FirstOrDefault(e => e.Entity == orderdetails);
                    entry.State = EntityState.Modified;
                }

                _repository.SaveChanges();
                return orderdetails;
            }
            catch (Exception ex)
            {
                // Handle the exception or log the error
                return null; // Indicate failure by returning null or throw an exception
            }

        }
    }
}
