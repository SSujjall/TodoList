using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Domain.Entities
{
    public class SubTask
    {
        [Key]
        public int Id { get; set; }
        public int TaskId { get; set; }

        [ForeignKey(nameof(TaskId))]
        public virtual Tasks Task { get; set; }

        public string SubTaskName { get; set; } = string.Empty;
        public bool IsComplete { get; set; } = false;
        public DateTime? CreatedAt { get; set; } = null;
    }
}
