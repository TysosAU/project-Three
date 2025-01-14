using API.Models;
using MongoDB.Driver;

namespace API.Services
{
    public class userService
    {
        private readonly IMongoCollection<User> _generalUsers;

        public userService(IMongoDatabase database)
        {
            _generalUsers = database.GetCollection<User>("Users");
        }
        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _generalUsers.Find(user => user.Username == username).FirstOrDefaultAsync();
        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _generalUsers.Find(user => user.Email == email).FirstOrDefaultAsync();
        }
        public async Task<User> CreateUserAsync(User user)
        {
            await _generalUsers.InsertOneAsync(user);
            return user;
        }
    }
}
