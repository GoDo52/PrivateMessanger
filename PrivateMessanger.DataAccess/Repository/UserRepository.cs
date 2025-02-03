using Microsoft.EntityFrameworkCore;
using Personal.DataAccess.Exceptions;
using PrivateMessanger.DataAccess.Data;
using PrivateMessanger.DataAccess.Repository.IRepository;
using PrivateMessanger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateMessanger.DataAccess.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public override async Task AddAsync(User user)
        {
            bool duplicateUserTag = await _db.Users.AnyAsync(u => u.Tag == user.Tag);

            if (duplicateUserTag)
            {
                throw new DuplicateUserTagException();
            }
            else
            {
                await base.AddAsync(user);
            }
        }

        public async Task UpdateAsync(User user)
        {
            var userFromDb = await _db.Users.FirstOrDefaultAsync(s => s.Id == user.Id);

            if (userFromDb != null)
            {
                bool duplicateUserTag = await _db.Users.AnyAsync(u => u.Tag == user.Tag);

                if (duplicateUserTag && user.Tag != userFromDb.Tag)
                {
                    throw new DuplicateUserTagException();
                }
                else
                {
                    userFromDb.Id = user.Id;
                    userFromDb.Tag = user.Tag;
                    userFromDb.UserName = user.UserName;

                    _db.Users.Update(userFromDb);
                }
            }
            else
            {
                throw new UserNotFoundException();
            }
        }
    }
}
