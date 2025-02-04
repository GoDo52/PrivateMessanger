using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateMessenger.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository User { get; }
        IAdministrationRoleRepository AdministrationRole { get; }
        IMessageRepository Message { get; }
        IChatRepository Chat { get; }
        IUserChatRepository UserChat { get; }
        IUserChatRoleRepository UserChatRole { get; }


        Task SaveAsync();

        Task CompleteAsync();
    }
}
