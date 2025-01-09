using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Application.DTOs;

namespace Todo.Application.Interface.IServices
{
    public interface IListService
    {
        Task<List<ListDTO>> GetAll(string currentUserId);
        //Task<ListDTO> GetListById(int listId, string currentUserId);
        Task<string> AddList(AddListDTO addListDto, List<string> errors, string currentUserId);
        Task<string> UpdateList(UpdateListDTO updateListDto, List<string> errors, string currentUserId);
        Task<string> DeleteList(int id, List<string> errors, string currentUserId);
    }
}
