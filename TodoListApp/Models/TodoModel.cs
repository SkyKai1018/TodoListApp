using System;
using System.ComponentModel.DataAnnotations;

namespace TodoListApp.Models
{
    public class TodoModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public bool IsComplete { get; set; }
    }
}
