using PrivateMessenger.DataAccess.Data;
using PrivateMessenger.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateMessenger.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public IUserRepository User {  get; private set; }
        public IAdministrationRoleRepository AdministrationRole {  get; private set; }
        public IMessageRepository Message {  get; private set; }
        public IChatRepository Chat {  get; private set; }
        public IUserChatRepository UserChat {  get; private set; }
        public IUserChatRoleRepository UserChatRole {  get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            User = new UserRepository(_db);
            AdministrationRole = new AdministrationRoleRepository(_db);
            Message = new MessageRepository(_db);
            Chat = new ChatRepository(_db);
            UserChat = new UserChatRepository(_db);
            UserChatRole = new UserChatRoleRepository(_db);
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task CompleteAsync()
        {
            await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
