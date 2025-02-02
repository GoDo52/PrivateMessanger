using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateMessanger.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository User { get; }
        IMessageRepository Message { get; }
        IChatRepository Chat { get; }
        IUserChatRepository UserChat { get; }

        Task SaveAsync();

        Task CompleteAsync();
    }
}
