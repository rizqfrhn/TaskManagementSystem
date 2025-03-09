using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskManagementSystem.App;
using TaskManagementSystem.Domain;

namespace TaskManagementSystem.API
{
    [ApiController]
    [Route("api/tasks")]
    public class TasksController : ControllerBase
    {
        private readonly TaskService _taskService;
        private readonly ILogger<TasksController> _logger;

        public TasksController(TaskService taskService, ILogger<TasksController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Create(TaskItem task)
        {
            if (task.DueDate < DateTime.UtcNow)
            {
                _logger.LogWarning("Attempt to create a task with an invalid due date: {DueDate}", task.DueDate);
                return BadRequest("Due date cannot be in the past");
            }

            _taskService.CreateTask(task);
            _logger.LogInformation("Created task with ID: {TaskId}", task.Id);
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            _logger.LogInformation("Received request to fetch all tasks.");
            var tasks = _taskService.GetAllTasks();
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            _logger.LogInformation("Received request to fetch task with ID: {TaskId}", id);
            var task = _taskService.GetTaskById(id);
            return task != null ? Ok(task) : NotFound();
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, TaskItem task)
        {
            if (task.DueDate < DateTime.UtcNow)
            {
                _logger.LogWarning("Attempt to update a task with an invalid due date: {DueDate}", task.DueDate);
                return BadRequest("Due date cannot be in the past");
            }

            var existingTask = _taskService.GetTaskById(id);
            if (existingTask == null)
            {
                _logger.LogWarning("Attempt to update non-existent task with ID: {TaskId}", id);
                return NotFound();
            }

            task.Id = id;
            _taskService.UpdateTask(task);
            _logger.LogInformation("Updated task with ID: {TaskId}", task.Id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existingTask = _taskService.GetTaskById(id);
            if (existingTask == null)
            {
                _logger.LogWarning("Attempt to delete non-existent task with ID: {TaskId}", id);
                return NotFound();
            }

            _taskService.DeleteTask(id);
            _logger.LogInformation("Deleted task with ID: {TaskId}", id);
            return NoContent();
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetTasksByUser(int userId)
        {
            _logger.LogInformation("Received request to fetch task with UserID: {userId}", userId);
            var task = _taskService.GetTaskByUserId(userId);
            return task != null ? Ok(task) : NotFound();
        }
    }
}
