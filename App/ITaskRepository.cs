using TaskManagementSystem.Domain;

namespace TaskManagementSystem.App
{
    public interface ITaskRepository
    {
        IEnumerable<TaskItem> GetAll();
        TaskItem GetById(int id);
        TaskItem GetByUserId(int id);
        void Add(TaskItem task);
        void Update(TaskItem task);
        void Delete(int id);
    }
}
