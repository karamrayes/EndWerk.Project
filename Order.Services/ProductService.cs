﻿using EndWerk.Project.Data;
using Microsoft.EntityFrameworkCore;
using Order.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Services
{
    public class ProductService
    {
        private Repository _repository;

        public ProductService(Repository repository)
        {
            _repository = repository;
        }

        public List<Product> GetProducts()
        {
                      
            return _repository.Products.Include(ct => ct.ProductCategory).Include(sp => sp.Supplier).ToList();
        }

        public Product GetProduct(int id) 
        {
            //changed for api
            //return _repository.Products.Include(ct => ct.ProductCategory).Include(sp => sp.Supplier).Include(od => od.OrderDetails).FirstOrDefault(p => p.ProductId == id);
            return _repository.Products.Include(ct => ct.ProductCategory).Include(sp => sp.Supplier).FirstOrDefault(p => p.ProductId == id);
        }

        public Product CreateOrUpdateProduct(Product product) 
        {
            try
            {
                if (product.ProductId == 0)
                {
                    _repository.Products.Add(product);
                }
                else
                {
                    _repository.Products.Attach(product);
                    var entry = _repository.ChangeTracker.Entries().FirstOrDefault(e => e.Entity == product);
                    entry.State = EntityState.Modified;
                }

                _repository.SaveChanges();
                return product;
            }
            catch (Exception ex)
            {
                // Handle the exception or log the error
                return null; // Indicate failure by returning null or throw an exception
            }
        }

        public bool DeleteProduct(int id) 
        {
            var product = GetProduct(id);
            if (product != null) 
            {
                _repository.Products.Remove(product);
                _repository.SaveChanges();
                return true;

            }
            else
            {
                return false;
            }
        }

        public void UpdateReposistory()
        {
            _repository.SaveChanges();
        }

        public void UpdateProductUnitInstock(List<OrderDetail> OrderDetailsList)
        {
            
                foreach (var item in OrderDetailsList)
                {
                    GetProduct(item.ProductId).UnitInStock -= item.Quantity;
                }

                _repository.SaveChanges();
           
        }

        //to be used for later up dates
        //public void UpdateProductUnitInstock(OrderDetail OrderDetail)
        //{


        //    GetProduct(OrderDetail.ProductId).UnitInStock -= OrderDetail.Quantity;

        //    _repository.SaveChanges();

        //}

        //for the delete method in controller
        public void UpdateProductUnitInstock(List<OrderDetail> OrderDetailsList ,bool delete)
        {

            foreach (var item in OrderDetailsList)
            {
                //add the number of products in the Canceled order to the unitinstock
                GetProduct(item.ProductId).UnitInStock += item.Quantity;
            }

            _repository.SaveChanges();

        }

    }
}
