using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IDbInitializer  // Migration , Update Db , Data Seeding ( 2y 7aga Na2sa Ht3mlha)
    {
        Task InitializeAsync();
        Task InitializeIdentityAsync();

    }
}
