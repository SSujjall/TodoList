using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Application.DTOs;

namespace Todo.Application.Interface.IServices
{
    public interface ITasksService
    {
        Task<List<TaskDTO>> GetAll(int listId);
        Task<TaskDTO?> GetById(int taskId);
        Task<(string message, int? taskId)> AddTask(AddTaskDTO addTaskDto, List<string> errors);
        Task<string> UpdateTask(UpdateTaskDTO updateTaskDto, List<string> errors);
        Task<string> DeleteTask(int taskId, List<string> errors);
    }
}
