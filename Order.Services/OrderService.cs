﻿using EndWerk.Project.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;
using Order.Object;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityState = Microsoft.EntityFrameworkCore.EntityState;

namespace Order.Services
{
    public class OrderService
    {

        private Repository _repository;
        private OrderDetailsService _orderDetailsService;
        private ProductService _productService;


        public OrderService(Repository repository, OrderDetailsService orderDetailsService, ProductService procutService)
        {
            _repository = repository;
            _orderDetailsService = orderDetailsService;
            _productService = procutService;
        }

        public List<Order.Object.Order> GetOrders() 
        {          
            return _repository.Orders.Include(u => u.User).Include(o => o.OrderDetail).ThenInclude(od => od.Product).ThenInclude(p => p.ProductCategory)
                         .ToList();

        }

        public Order.Object.Order GetOrder(int id)
        { 
            
            return _repository.Orders.Include(u => u.User).Include(o => o.OrderDetail)
                         .ThenInclude(od => od.Product).FirstOrDefault(o => o.OrderId == id) ??new Object.Order();
                        
        }


        public Order.Object.Order? UpdateOrCreateOrder(Order.Object.Order order) 
        {

            //insert
            try
            {
                if (order.OrderId == 0)
                {
                    _repository.Orders.Add(order);
                }
                else
                {
                    //update
                    _repository.Orders.Attach(order);
                    var entry = _repository.ChangeTracker.Entries().FirstOrDefault(e => e.Entity == order);
                    entry.State = EntityState.Modified;
                }

                _repository.SaveChanges();
                return order;
            }
            catch (Exception ex)
            {
                // Handle the exception or log the error
                return null; // Indicate failure by returning null or throw an exception
            }

        }

        public bool DeleteOrder(int id) 
        {
            var user = GetOrder(id);
            if (user != null)
            {
                _repository.Remove(user);
                _repository.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        //from the controller you get a order and list of orderdetails , first we make order then we use that order id to make the orderdetails
        public void MakeOrder(Order.Object.Order NewOrder, List<OrderDetail> OrderDetailsList)
        {
            
            var Result = UpdateOrCreateOrder(NewOrder);

            
            
            if (Result != null)
            {
                //update the property unti in stock for the bought items
                _productService.UpdateProductUnitInstock(OrderDetailsList);

                foreach (var orderDetails in OrderDetailsList)
                {
                    orderDetails.OrderId = NewOrder.OrderId;
                    _orderDetailsService.UpdateOrCreateOrderDetails(orderDetails);
                }
                

            }

        }

        public decimal CalculateOrderAmount(List<OrderDetail> OrderDetailsList)
        {
            decimal totalAmount=0;

            foreach (var item in OrderDetailsList)
            {
                item.UnitPrice = _productService.GetProduct(item.ProductId).ProductPrice;

                totalAmount += item.Quantity * item.UnitPrice;
            }

            return totalAmount;
        }

        public decimal CalculateOrderAmount(OrderDetail OrderDetails)
        {
            decimal totalAmount = 0;

           
                var UnitPrice = _productService.GetProduct(OrderDetails.ProductId).ProductPrice;

                totalAmount += OrderDetails.Quantity * OrderDetails.UnitPrice;
            

            return totalAmount;
        }

        public void UpdateReposistory()
        {
            _repository.SaveChanges();
        }

        public bool CheckUnitInStock(List<OrderDetail> orderDetailsList) 
        {
            foreach (var item in orderDetailsList)
            {
                if (item.Quantity > _productService.GetProduct(item.ProductId).UnitInStock)
                {

                    return false;
                }
            }

            return true;

        }
    }
}
