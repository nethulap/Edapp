using System.Threading.Tasks;
using AutoMapper;
using Edapp.Data;
using Edapp.Model.Response;
using Microsoft.EntityFrameworkCore;

namespace Edapp.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly EdappContext _edappContext;
        private readonly IMapper _mapper;

        public UserRepository(EdappContext edappContext, IMapper mapper)
        {
            _edappContext = edappContext;
            _mapper = mapper;
        }

        public async Task<UserResponse> GetUserAsync(int id)
        {
            var userEntity = await _edappContext.User.FirstOrDefaultAsync(u => u.Id == id);
            var user = _mapper.Map<UserResponse>(userEntity);
            return user;
        }
    }
}