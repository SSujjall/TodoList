using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Domain.Entities;
using Todo.Domain.Common;

namespace Todo.Application.DTOs
{
    public class ListDTO
    {
        public int Id { get; set; }
        public string ListName { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; } = MyDateTime.CreatedDate;
    }

    public class AddListDTO
    {
        public string ListName { get; set; }
    }

    public class UpdateListDTO
    {
        public int Id { get; set; }
        public string ListName { get; set; }
    }
}
