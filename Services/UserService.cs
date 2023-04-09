using Database;
using Interfaces;
using Models;

namespace Services {
    public class UserService : IUserService
    {
        private readonly UserProfileContext _context;
        public UserService(UserProfileContext context)
        {
            this._context = context;
        }

        public async Task<List<UserModel>> GetAllUsers()
        {
            var users =  _context.Users.ToList();
            return await Task.FromResult(users);
        }

        public async Task<UserModel?> GetUserDetail(int id)
        {
            var users = await _context.Users.FindAsync(id);
            return users ?? null;
        }
        public async Task<UserModel?> CreateUser(UserModel model)
        {
            var user = _context.Users.Add(model);
            await _context.SaveChangesAsync();
            return await GetUserDetail(model.Id) ?? null;
        }

        public async Task<UserModel?> UpdateUser(UserModel model)
        {
            var user = await GetUserDetail(model.Id);
            if (user == null)
                return null;
            _context.Update(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<UserModel?> DeleteUser(int id)
        {
            var user = await GetUserDetail(id);
            if (user == null)
                return null;
            _context.Remove(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}