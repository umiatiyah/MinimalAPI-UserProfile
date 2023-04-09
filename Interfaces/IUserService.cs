using Models;

namespace Interfaces
{
    public interface IUserService
    {
        Task<List<UserModel>> GetAllUsers();
        Task<UserModel?> GetUserDetail(int id);
        Task<UserModel?> CreateUser(UserModel model);
        Task<UserModel?> UpdateUser(UserModel model);
        Task<UserModel?> DeleteUser(int id);
    }
}