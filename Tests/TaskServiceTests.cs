using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using TaskManagementSystem.App;
using TaskManagementSystem.Domain;

public class TaskServiceTests
{
    private readonly Mock<ITaskRepository> _mockTaskRepository;
    private readonly Mock<ILogger<TaskService>> _mockLogger;
    private readonly TaskService _taskService;

    public TaskServiceTests()
    {
        _mockTaskRepository = new Mock<ITaskRepository>();
        _mockLogger = new Mock<ILogger<TaskService>>();
        _taskService = new TaskService(_mockTaskRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public void CreateTask_ShouldAddTask_WhenValidTask()
    {
        // Arrange
        var newTask = new TaskItem
        {
            Title = "New Task",
            Description = "Task Description",
            DueDate = DateTime.UtcNow.AddDays(1),
            Priority = "High",
            Status = "Open",
            AssignedUserId = 1
        };

        // Act
        _taskService.CreateTask(newTask);

        // Assert
        _mockTaskRepository.Verify(r => r.Add(It.IsAny<TaskItem>()), Times.Once);
    }

    [Fact]
    public void CreateTask_ShouldThrowException_WhenDueDateIsInPast()
    {
        // Arrange
        var newTask = new TaskItem
        {
            Title = "New Task",
            Description = "Task Description",
            DueDate = DateTime.UtcNow.AddDays(-1),  // Past date
            Priority = "High",
            Status = "Open",
            AssignedUserId = 1
        };

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _taskService.CreateTask(newTask));
        Assert.Equal("Due date cannot be in the past", exception.Message);
    }

    [Fact]
    public void UpdateTask_ShouldUpdateExistingTask_WhenValidTask()
    {
        // Arrange
        var taskToUpdate = new TaskItem
        {
            Id = 1,
            Title = "Updated Task",
            Description = "Updated Description",
            DueDate = DateTime.UtcNow.AddDays(1),
            Priority = "Medium",
            Status = "In Progress",
            AssignedUserId = 2
        };

        _mockTaskRepository.Setup(r => r.GetById(It.IsAny<int>())).Returns(new TaskItem { Id = 1 });  // Mocking the existing task

        // Act
        _taskService.UpdateTask(taskToUpdate);

        // Assert
        _mockTaskRepository.Verify(r => r.Update(It.IsAny<TaskItem>()), Times.Once);
    }

    [Fact]
    public void DeleteTask_ShouldRemoveTask_WhenTaskExists()
    {
        // Arrange
        int taskId = 1;
        _mockTaskRepository.Setup(r => r.GetById(It.IsAny<int>())).Returns(new TaskItem { Id = taskId });

        // Act
        _taskService.DeleteTask(taskId);

        // Assert
        _mockTaskRepository.Verify(r => r.Delete(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public void GetAllTasks_ShouldReturnAllTasks()
    {
        // Arrange
        var tasks = new List<TaskItem>
        {
            new TaskItem { Id = 1, Title = "Task 1", DueDate = DateTime.UtcNow, Priority = "High", Status = "Open" },
            new TaskItem { Id = 2, Title = "Task 2", DueDate = DateTime.UtcNow, Priority = "Low", Status = "Completed" }
        };
        _mockTaskRepository.Setup(r => r.GetAll()).Returns(tasks);

        // Act
        var result = _taskService.GetAllTasks();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public void GetTaskById_ShouldReturnTask_WhenTaskExists()
    {
        // Arrange
        int taskId = 1;
        var task = new TaskItem { Id = taskId, Title = "Task 1", DueDate = DateTime.UtcNow, Priority = "High", Status = "Open" };
        _mockTaskRepository.Setup(r => r.GetById(It.IsAny<int>())).Returns(task);

        // Act
        var result = _taskService.GetTaskById(taskId);

        // Assert
        Assert.Equal(taskId, result.Id);
    }

    [Fact]
    public void GetTaskById_ShouldReturnNull_WhenTaskDoesNotExist()
    {
        // Arrange
        int taskId = 999;
        _mockTaskRepository.Setup(r => r.GetById(It.IsAny<int>())).Returns((TaskItem)null);

        // Act
        var result = _taskService.GetTaskById(taskId);

        // Assert
        Assert.Null(result);
    }
}
