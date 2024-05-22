﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommereceSiteData.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepositroy Category { get; }
        IProductRepository Product { get; }

        void Save();
    }
}