using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using Todo.Application.DTOs;
using Todo.Application.Helpers;
using Todo.Application.Interface.IServices;

namespace Todo.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ListController : ControllerBase
    {
        public readonly IListService _listService;

        public ListController(IListService listService)
        {
            _listService = listService;
        }

        [HttpGet("GetAllList")]
        public async Task<IActionResult> GetAll()
        {
            //claims bata user ko id nikalne
            var currentUserId = User.FindFirstValue("userId");

            if(string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized(new { message = "User Not Logged In"});
            }

            var lists = await _listService.GetAll(currentUserId);

            if(lists == null || !lists.Any())
            {
                return BadRequest(new { message = "No List Found For This User." });
            }
            return Ok(new Response(lists, null, HttpStatusCode.OK));
        }

        [HttpPost("AddList")]
        public async Task<IActionResult> AddList([FromBody] AddListDTO addListDto)
        {
            var errors = new List<string>();
            var currentUserId = User.FindFirstValue("userId");

            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized(new { message = "User is not authenticated." });
            }

            var result = await _listService.AddList(addListDto, errors, currentUserId);

            if (errors.Count > 0)
            {
                return BadRequest(new Response(null, errors, HttpStatusCode.BadRequest));
            }

            return Ok(new { message = result });
        }

        [HttpPut("UpdateList")]
        public async Task<IActionResult> UpdateList([FromBody] UpdateListDTO updateListDto)
        {
            var errors = new List<string>();

            var currentUserId = User.FindFirstValue("userId");

            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized(new { message = "User is not authenticated." });
            }

            var result = await _listService.UpdateList(updateListDto, errors, currentUserId);

            if (errors.Count > 0)
            {
                return BadRequest(new Response(null, errors, HttpStatusCode.BadRequest));
            }

            return Ok(new Response(new { message = result }, null, HttpStatusCode.OK));
        }

        [HttpDelete("DeleteList/{id}")]
        public async Task<IActionResult> DeleteList(int id)
        {
            var errors = new List<string>();
            var currentUserId = User.FindFirstValue("userId");

            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized(new { message = "User is not authenticated." });
            }

            var result = await _listService.DeleteList(id, errors, currentUserId);

            if (errors.Count > 0)
            {
                return BadRequest(new Response(null, errors, HttpStatusCode.BadRequest));
            }

            return Ok(new { message = result });
        }
    }
}