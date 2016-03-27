using HomeShop.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace HomeShop.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private HomeStoreEntities context = new HomeStoreEntities();

        public IQueryable<Product> All
        {
            get
            {
                return context.Products;
            }
        }

        public IQueryable<Product> AllIncluding(params Expression<Func<Product,object>>[] includeProperties)
        {
            IQueryable<Product> query = context.Products;
            foreach(var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public Product Find(int id)
        {
            return context.Products.Find(id);
        }
    }
}