using PrivateMessanger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateMessanger.DataAccess.Repository.IRepository
{
    public interface IChatRepository : IRepository<Chat>
    {
        Task UpdateAsync(Chat chat);
    }
}
