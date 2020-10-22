using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoTask.Models
{
    public class TasksViewModel
    {
        public List<Task> Tasks { get; set; }
        public int Quantity { get; set; }
        public string Mode { get; set; }
        public int ItemsLeft { get; set; }
    }
}
