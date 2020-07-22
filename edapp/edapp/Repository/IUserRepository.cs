using System.Threading.Tasks;
using Edapp.Data;
using Edapp.Model.Response;

namespace Edapp.Repository
{
    public interface IUserRepository
    {
        Task<UserResponse> GetUserAsync(int id);
    }
}