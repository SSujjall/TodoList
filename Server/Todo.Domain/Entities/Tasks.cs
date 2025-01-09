using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Domain.Common;

namespace Todo.Domain.Entities
{
    public class Tasks
    {
        [Key]
        public int Id { get; set; }
        public int ListId { get; set; }

        [ForeignKey(nameof(ListId))]
        public virtual List List { get; set; }

        public string TaskName { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; } = null;
        public bool IsComplete { get; set; }
        public DateTime? CreatedAt { get; set; } = MyDateTime.CreatedDate;
        public DateTime? UpdatedAt { get; set; } = null;
    }
}