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
    public class SupplierService
    {
        private Repository _repository;

        public SupplierService(Repository repository)
        {
            _repository = repository;
        }

        public List<Supplier> GetSuppliers()
        {
            return _repository.Suppliers.ToList();
        }

        public Supplier GetSupplier(int id)
        {            
            return _repository.Suppliers.FirstOrDefault(o => o.SupplierId == id);
        }

        public Supplier UpdateOrCreateSupplier(Supplier supplier)
        {

            //insert
            try
            {
                if (supplier.SupplierId == 0)
                {
                    _repository.Suppliers.Add(supplier);
                }
                else
                {
                    //update
                    _repository.Suppliers.Attach(supplier);
                    var entry = _repository.ChangeTracker.Entries().FirstOrDefault(e => e.Entity == supplier);
                    entry.State = EntityState.Modified;
                }

                _repository.SaveChanges();
                return supplier;
            }
            catch (Exception ex)
            {
                // Handle the exception or log the error
                return null; // Indicate failure by returning null or throw an exception
            }

        }

        public bool DeleteSupplier(int id)
        {
            var user = GetSupplier(id);

            if (user != null)
            {
                _repository.Suppliers.Remove(user);
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
