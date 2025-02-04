using Microsoft.AspNetCore.Http;
using PrivateMessenger.DataAccess.Repository.IRepository;
using PrivateMessenger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PrivateMessenger.Services
{
    public interface ICurrentUserService
    {
        int GetCurrentUserId();
        Task<User> GetCurrentUserAsync();
    }


    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
        }

        public int GetCurrentUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) throw new UnauthorizedAccessException("User not authenticated");
            
            return int.Parse(userIdClaim.Value);
        }

        public async Task<User> GetCurrentUserAsync()
        {
            var userId = GetCurrentUserId();
            var user = await _unitOfWork.User.GetAsync(u => u.Id == userId);
            if (user == null) throw new UnauthorizedAccessException("User not found");

            return user;
        }
    }
}
