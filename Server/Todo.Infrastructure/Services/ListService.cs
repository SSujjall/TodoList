using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Todo.Application.DTOs;
using Todo.Application.Interface.IRepositories;
using Todo.Application.Interface.IServices;
using Todo.Domain.Entities;

namespace Todo.Infrastructure.Services
{
    public class ListService : IListService
    {
        private readonly IListRepository _listRepository;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ListService(IListRepository listRepository, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _listRepository = listRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> AddList(AddListDTO addListDto, List<string> errors, string currentUserId)
        {
            try
            {
                var newList = new List
                {
                    ListName = addListDto.ListName,
                    UserId = currentUserId 
                };

                await _listRepository.Add(newList);
                await _listRepository.SaveChangesAsync();
                return "List successfully added.";
            }
            catch (Exception ex)
            {
                errors.Add("Failed To Add List as user Not Logged In");
                return "Failed to add list.";
            }
        }

        public async Task<string> DeleteList(int id, List<string> errors, string currentUserId)
        {
            try
            {
                var list = await _listRepository.GetById(id);

                if (list == null || list.UserId != currentUserId)
                {
                    errors.Add("List not found or you don't have permission to delete this list.");
                    return "Failed to delete list.";
                }

                await _listRepository.Delete(list);
                await _listRepository.SaveChangesAsync();

                return "List successfully deleted.";
            }
            catch (Exception ex)
            {
                errors.Add("Failed to delete list due to an error.");
                return "Failed to delete list.";
            }
        }

        public async Task<List<ListDTO>> GetAll(string currentUserId)
        {
            var lists = await _listRepository.GetAll(null);
            var filteredLists = lists.Where(list => list.UserId == currentUserId);

            //Yo ra tala gareko same ho
            var listDto = filteredLists.Select(list => new ListDTO
            {
                Id = list.Id,
                ListName = list.ListName,
                UserId = list.UserId,
                CreatedAt = list.CreatedAt
            }).ToList();

            //var listDto = new List<ListDTO>();

            //foreach (var item in filteredLists)
            //{
            //    var items = new ListDTO
            //    {
            //        Id = item.Id,
            //        ListName = item.ListName,
            //        UserId = item.UserId,
            //        IsMine = true, // Since we filtered, all lists are owned by the current user
            //        CreatedAt = item.CreatedAt
            //    };
            //    listDto.Add(items);
            //}

            return listDto;
        }

        public async Task<string> UpdateList(UpdateListDTO updateListDto, List<string> errors, string currentUserId)
        {
            try
            {
                var list = await _listRepository.GetById(updateListDto.Id);

                if (list == null || list.UserId != currentUserId)
                {
                    errors.Add("List not found or you don't have permission to update this list.");
                    return "Failed to update list.";
                }

                list.ListName = updateListDto.ListName;

                await _listRepository.Update(list);
                await _listRepository.SaveChangesAsync();

                return "List successfully updated.";
            }
            catch (Exception e)
            {
                errors.Add("Failed to update list due to an error.");
                return "Failed to update list.";
            }
        }
    }
}