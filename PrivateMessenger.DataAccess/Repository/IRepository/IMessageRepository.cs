using PrivateMessenger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace PrivateMessenger.DataAccess.Repository.IRepository
{
    public interface IMessageRepository : IRepository<Message>
    {
        Task UpdateAsync(Message message);
    }
}
