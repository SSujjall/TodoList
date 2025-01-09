using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Application.DTOs
{
    public class SubTaskDTO
    {
        public int Id { get; set; }
        public string SubTaskName { get; set; }
        public bool IsComplete { get; set; }
        public int TaskId { get; set; }
        public DateTime? CreatedAt { get; set; }
    }

    public class AddSubTaskDTO
    {
        [Required]
        public string SubTaskName { get; set; }
        [Required]
        public int TaskId { get; set; }
    }

    public class UpdateSubTaskDTO
    {
        [Required]
        public int Id { get; set; }
        public string SubTaskName { get; set; }
        public bool IsComplete { get; set; }
    }
}
