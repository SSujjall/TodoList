using Todo.Application.DTOs;
using Todo.Application.Interface.IRepositories;
using Todo.Application.Interface.IServices;
using Todo.Domain.Entities;

namespace Todo.Infrastructure.Services
{
    public class SubTasksService : ISubTasksService
    {
        private readonly ISubTasksRepository _subTasksRepository;

        public SubTasksService(ISubTasksRepository subTasksRepository)
        {
            _subTasksRepository = subTasksRepository;
        }

        public async Task<SubTaskDTO> GetSubtaskById(int id)
        {
            var subTask = await _subTasksRepository.GetById(id);
            if (subTask == null) return null;

            return new SubTaskDTO
            {
                Id = subTask.Id,
                TaskId = subTask.TaskId,
                SubTaskName = subTask.SubTaskName,
                IsComplete = subTask.IsComplete,
                CreatedAt = subTask.CreatedAt
            };
        }

        public async Task<List<SubTaskDTO>> GetAllSubTasks(int taskId)
        {
            var subTasks = await _subTasksRepository.GetAll(null);
            var filteredSubTasks = subTasks.Where(st => st.TaskId == taskId).ToList();

            return filteredSubTasks.Select(st => new SubTaskDTO
            {
                Id = st.Id,
                SubTaskName = st.SubTaskName,
                IsComplete = st.IsComplete,
                TaskId = st.TaskId,
                CreatedAt = st.CreatedAt
            }).ToList();
        }

        public async Task<string> AddSubTask(AddSubTaskDTO addSubTaskDto)
        {
            var subTask = new SubTask
            {
                SubTaskName = addSubTaskDto.SubTaskName,
                TaskId = addSubTaskDto.TaskId,
                CreatedAt = DateTime.Now
            };

            await _subTasksRepository.Add(subTask);
            await _subTasksRepository.SaveChangesAsync();

            return "Subtask successfully added.";
        }

        public async Task<string> UpdateSubTask(UpdateSubTaskDTO updateSubTaskDto)
        {
            var subTask = await _subTasksRepository.GetById(updateSubTaskDto.Id);

            if (subTask == null)
            {
                return "Subtask not found.";
            }

            subTask.SubTaskName = updateSubTaskDto.SubTaskName ?? subTask.SubTaskName;
            subTask.IsComplete = updateSubTaskDto.IsComplete;

            await _subTasksRepository.Update(subTask);
            await _subTasksRepository.SaveChangesAsync();

            return "Subtask successfully updated.";
        }

        public async Task<string> DeleteSubTask(int id)
        {
            var subTask = await _subTasksRepository.GetById(id);

            if (subTask == null)
            {
                return "Subtask not found.";
            }

            await _subTasksRepository.Delete(subTask);
            await _subTasksRepository.SaveChangesAsync();

            return "Subtask successfully deleted.";
        }
    }
}