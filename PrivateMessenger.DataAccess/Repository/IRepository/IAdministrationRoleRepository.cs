using PrivateMessenger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateMessenger.DataAccess.Repository.IRepository
{
    public interface IAdministrationRoleRepository : IRepository<AdministrationRole>
    {
        Task UpdateAsync(AdministrationRole administrationRole);
    }
}
