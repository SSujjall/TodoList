using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using Todo.Application.DTOs;
using Todo.Application.Helpers;
using Todo.Application.Interface.IRepositories;
using Todo.Application.Interface.IServices;
using Todo.Infrastructure.Services;

namespace Todo.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        public readonly ITasksService _taskService;

        public TasksController(ITasksService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("GetAll/{listId}")]
        public async Task<IActionResult> GetAllTasks(int listId)
        {
            var tasks = await _taskService.GetAll(listId);

            if (tasks == null || !tasks.Any())
            {
                return NotFound(new { message = "No tasks found for this list." });
            }

            return Ok(new Response(tasks, null, HttpStatusCode.OK));
        }

        [HttpGet("GetById/{taskId}")]
        public async Task<IActionResult> GetTaskById(int taskId)
        {
            var task = await _taskService.GetById(taskId);

            if (task == null)
            {
                return NotFound(new { message = "Task not found." });
            }

            return Ok(new Response(task, null, HttpStatusCode.OK));
        }

        [HttpPost("AddTask")]
        public async Task<IActionResult> AddTask([FromBody] AddTaskDTO addTaskDto)
        {
            if (addTaskDto == null)
            {
                return BadRequest(new { message = "Invalid Model for adding task." });
            }

            var errors = new List<string>();

            var (result, taskId) = await _taskService.AddTask(addTaskDto, errors);

            if (errors.Any())
            {
                return BadRequest(new Response(null, errors, HttpStatusCode.BadRequest));
            }

            return Ok(new { message = result, TaskId = taskId });
        }

        [HttpPut("UpdateTask")]
        public async Task<IActionResult> UpdateTask([FromBody] UpdateTaskDTO updateTaskDto)
        {
            var errors = new List<string>();

            var result = await _taskService.UpdateTask(updateTaskDto, errors);

            if (errors.Any())
            {
                return BadRequest(new Response(null, errors, HttpStatusCode.BadRequest));
            }

            return Ok(new { message = result });
        }

        [HttpDelete("DeleteTask/{taskId}")]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            var errors = new List<string>();

            var result = await _taskService.DeleteTask(taskId, errors);

            if (errors.Any())
            {
                return BadRequest(new Response(null, errors, HttpStatusCode.BadRequest));
            }

            return Ok(new { message = result });
        }
    }
}