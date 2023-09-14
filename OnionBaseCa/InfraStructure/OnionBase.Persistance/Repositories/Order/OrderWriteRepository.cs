﻿using OnionBase.Application.Repositories;
using OnionBase.Domain.Entities;
using OnionBase.Persistance.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionBase.Persistance.Repositories
{
    public class OrderWriteRepository : WriteRepository<Order>, IOrderWriteRepository
    {
        public OrderWriteRepository(UserDbContext context) : base(context)
        {
        }
    }
}
