using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Todo.Application.DTOs;
using Todo.Application.Helpers;
using Todo.Application.Interface.IServices;

namespace Todo.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SubTasksController : ControllerBase
    {
        private readonly ISubTasksService _subTasksService;

        public SubTasksController(ISubTasksService subTasksService)
        {
            _subTasksService = subTasksService;
        }

        [HttpGet("GetAllSubTasks/{taskId}")]
        public async Task<IActionResult> GetAllSubTasks(int taskId)
        {
            var subTasks = await _subTasksService.GetAllSubTasks(taskId);

            if (subTasks == null || subTasks.Count == 0)
            {
                return NotFound(new { message = "No SubTasks found for this task." });
            }

            return Ok(new Response(subTasks, null, HttpStatusCode.OK));
        }

        [HttpGet("GetSubtaskById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var subTask = await _subTasksService.GetSubtaskById(id);
            if (subTask == null)
            {
                return NotFound(new Response(null, new List<string> { "SubTask not found." }, HttpStatusCode.NotFound));
            }
            return Ok(new Response(subTask, null, HttpStatusCode.OK));
        }

        [HttpPost("AddSubTask")]
        public async Task<IActionResult> AddSubTask([FromBody] AddSubTaskDTO addSubTaskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _subTasksService.AddSubTask(addSubTaskDto);
            return Ok(new { message = result });
        }

        [HttpPut("UpdateSubTask")]
        public async Task<IActionResult> UpdateSubTask([FromBody] UpdateSubTaskDTO updateSubTaskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _subTasksService.UpdateSubTask(updateSubTaskDto);
            return Ok(new { message = result });
        }

        [HttpDelete("DeleteSubTask/{id}")]
        public async Task<IActionResult> DeleteSubTask(int id)
        {
            var result = await _subTasksService.DeleteSubTask(id);
            return Ok(new { message = result });
        }
    }
}