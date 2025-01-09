using Todo.Application.DTOs;
using Todo.Application.Interface.IRepositories;
using Todo.Application.Interface.IServices;
using Todo.Domain.Entities;

namespace Todo.Infrastructure.Services
{
    public class TasksService : ITasksService
    {
        private readonly ITasksRepository _tasksRepository;
        private readonly IListRepository _listRepository;

        public TasksService(ITasksRepository tasksRepository, IListRepository listRepository)
        {
            _tasksRepository = tasksRepository;
            _listRepository = listRepository;
        }

        public async Task<List<TaskDTO>> GetAll(int listId)
        {
            var tasks = await _tasksRepository.GetAll(null);
            var filteredTasks = tasks.Where(task => task.ListId == listId);

            var taskDto = filteredTasks.Select(task => new TaskDTO
            {
                Id = task.Id,
                TaskName = task.TaskName,
                Description = task.Description,
                DueDate = task.DueDate,
                IsComplete = task.IsComplete,
                ListId = task.ListId,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt
            }).ToList();

            return taskDto;
        }

        public async Task<TaskDTO?> GetById(int taskId)
        {
            var task = await _tasksRepository.GetById(taskId);
            if (task == null) return null;

            return new TaskDTO
            {
                Id = task.Id,
                TaskName = task.TaskName,
                Description = task.Description,
                DueDate = task.DueDate,
                IsComplete = task.IsComplete,
                ListId = task.ListId,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt
            };
        }

        public async Task<(string message, int? taskId)> AddTask(AddTaskDTO addTaskDto, List<string> errors)
        {
            try
            {
                var list = await _listRepository.GetById(addTaskDto.ListId);
                if (list == null)
                {
                    errors.Add("Invalid list ID.");
                    return ("Failed to add task.", null);
                }

                var newTask = new Tasks
                {
                    TaskName = addTaskDto.TaskName,
                    Description = addTaskDto.Description,
                    DueDate = addTaskDto.DueDate,
                    IsComplete = false,
                    ListId = addTaskDto.ListId
                };

                await _tasksRepository.Add(newTask);
                await _tasksRepository.SaveChangesAsync();
                return ("Task successfully added.", newTask.Id);
            }
            catch (Exception ex)
            {
                errors.Add("Failed to add task due to an error.");
                return ("Failed to add task.", null);
            }
        }

        public async Task<string> UpdateTask(UpdateTaskDTO updateTaskDto, List<string> errors)
        {
            try
            {
                var task = await _tasksRepository.GetById(updateTaskDto.Id);
                if (task == null)
                {
                    errors.Add("Task not found.");
                    return "Failed to update task.";
                }

                task.TaskName = updateTaskDto.TaskName;
                task.Description = updateTaskDto.Description;
                task.DueDate = updateTaskDto.DueDate;
                task.IsComplete = updateTaskDto.IsComplete;
                task.UpdatedAt = DateTime.Now;

                await _tasksRepository.Update(task);
                await _tasksRepository.SaveChangesAsync();

                return "Task successfully updated.";
            }
            catch (Exception ex)
            {
                errors.Add("Failed to update task due to an error.");
                return "Failed to update task.";
            }
        }

        public async Task<string> DeleteTask(int taskId, List<string> errors)
        {
            try
            {
                var task = await _tasksRepository.GetById(taskId);
                if (task == null)
                {
                    errors.Add("Task not found.");
                    return "Failed to delete task.";
                }

                await _tasksRepository.Delete(task);
                await _tasksRepository.SaveChangesAsync();

                return "Task successfully deleted.";
            }
            catch (Exception ex)
            {
                errors.Add("Failed to delete task due to an error.");
                return "Failed to delete task.";
            }
        }
    }
}
