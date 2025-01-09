using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Application.DTOs;

namespace Todo.Application.Interface.IServices
{
    public interface ISubTasksService
    {
        public Task<List<SubTaskDTO>> GetAllSubTasks(int taskId);
        public Task<SubTaskDTO> GetSubtaskById (int id);
        public Task<string> AddSubTask(AddSubTaskDTO addSubTaskDto);
        public Task<string> UpdateSubTask(UpdateSubTaskDTO updateSubTaskDto);
        public Task<string> DeleteSubTask(int id);
    }
}
