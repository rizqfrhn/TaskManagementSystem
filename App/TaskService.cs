using Microsoft.Extensions.Logging;
using TaskManagementSystem.Domain;

namespace TaskManagementSystem.App
{
    public class TaskService
    {
        private readonly ITaskRepository _repository;
        private readonly ILogger<TaskService> _logger;

        public TaskService(ITaskRepository repository, ILogger<TaskService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public IEnumerable<TaskItem> GetAllTasks()
        {
            _logger.LogInformation("Fetching all tasks.");
            return _repository.GetAll();
        }
        public TaskItem GetTaskById(int id)
        {
            _logger.LogInformation("Fetching task with ID: {TaskId}", id);
            return _repository.GetById(id);
        }
        public void CreateTask(TaskItem task)
        {
            if (task.DueDate < DateTime.UtcNow)
            {
                _logger.LogWarning("Attempt to create a task with an invalid due date: {DueDate}", task.DueDate);
                throw new ArgumentException("Due date cannot be in the past");
            }

            _repository.Add(task);
            _logger.LogInformation("Task created with ID: {TaskId}", task.Id);
        }
        public void UpdateTask(TaskItem task)
        {
            var existingTask = _repository.GetById(task.Id);
            if (existingTask == null)
            {
                _logger.LogWarning("Attempt to update non-existent task with ID: {TaskId}", task.Id);
                throw new ArgumentException("Task not found");
            }

            _repository.Update(task);
            _logger.LogInformation("Task updated with ID: {TaskId}", task.Id);
        }
        public void DeleteTask(int id)
        {
            var task = _repository.GetById(id);
            if (task == null)
            {
                _logger.LogWarning("Attempt to delete non-existent task with ID: {TaskId}", id);
                throw new ArgumentException("Task not found");
            }

            _repository.Delete(id);
            _logger.LogInformation("Task deleted with ID: {TaskId}", id);
        }
        public TaskItem GetTaskByUserId(int id)
        {
            _logger.LogInformation("Fetching task with UserID: {UserId}", id);
            return _repository.GetByUserId(id);
        }
    }
}
