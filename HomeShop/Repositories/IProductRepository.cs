using HomeShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace HomeShop.Repositories
{
    interface IProductRepository:IDisposable
    {
        IQueryable<Product> All { get; }
        IQueryable<Product> AllIncluding(params Expression<Func<Product, object>>[] includeProperties);
        Product Find(int id);
    }
}