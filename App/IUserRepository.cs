using TaskManagementSystem.Domain;

namespace TaskManagementSystem.App
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(Guid userId);
        Task<List<User>> GetAllUsersAsync();
    }
}
