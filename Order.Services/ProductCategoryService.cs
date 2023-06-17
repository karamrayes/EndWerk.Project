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
    public class ProductCategoryService
    {
        private Repository _repository;

        
        public ProductCategoryService(Repository repository)
        {
            _repository = repository;
        }

        public List<ProductCategory> GetProductCatagoryList()
        {
            return _repository.ProductCategories.ToList();
        }

        public ProductCategory GetProductCatagory(int id)
        {           
            return _repository.ProductCategories.FirstOrDefault(o => o.ProductCategoryId == id);
        }

        public ProductCategory UpdateOrCreateProductCategory(ProductCategory productCategory)
        {

            //insert
            try
            {
                if (productCategory.ProductCategoryId == 0)
                {
                    _repository.ProductCategories.Add(productCategory);
                }
                else
                {
                    //update
                    _repository.ProductCategories.Attach(productCategory);
                    var entry = _repository.ChangeTracker.Entries().FirstOrDefault(e => e.Entity == productCategory);
                    entry.State = EntityState.Modified;
                }

                _repository.SaveChanges();
                return productCategory;
            }
            catch (Exception ex)
            {
                // Handle the exception or log the error
                return null; // Indicate failure by returning null or throw an exception
            }

        }

        public bool DeleteProductCategory(int id)
        {
            var user = GetProductCatagory(id);

            if (user != null)
            {
                _repository.ProductCategories.Remove(user);
                _repository.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
