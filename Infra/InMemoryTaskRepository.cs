using TaskManagementSystem.App;
using TaskManagementSystem.Domain;

namespace TaskManagementSystem.Infra
{
    public class InMemoryTaskRepository : ITaskRepository
    {
        private readonly List<TaskItem> _tasks = new();
        private int _nextId = 1;

        public IEnumerable<TaskItem> GetAll() => _tasks;
        public TaskItem GetById(int id) => _tasks.FirstOrDefault(t => t.Id == id);
        public void Add(TaskItem task)
        {
            task.Id = _nextId++;
            _tasks.Add(task);
        }
        public void Update(TaskItem task)
        {
            var existing = GetById(task.Id);
            if (existing != null)
            {
                existing.Title = task.Title;
                existing.Description = task.Description;
                existing.DueDate = task.DueDate;
                existing.Priority = task.Priority;
                existing.Status = task.Status;
                existing.AssignedUserId = task.AssignedUserId;
            }
        }
        public void Delete(int id) => _tasks.RemoveAll(t => t.Id == id);

        public TaskItem GetByUserId(int id) => _tasks.FirstOrDefault(t => t.AssignedUserId == id);
    }
}
