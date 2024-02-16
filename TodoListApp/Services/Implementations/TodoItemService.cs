using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoListApp.Models;

namespace TodoListApp.Services
{
    public class TodoItemService : ITodoItemService
    {
        private readonly ApplicationDbContext _context;

        public TodoItemService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TodoModel>> GetAllTodoItemsAsync()
        {
            return await _context.ToDoItems.ToListAsync();
        }

        public async Task<TodoModel> GetTodoItemByIdAsync(int id)
        {
            return await _context.ToDoItems.FindAsync(id);
        }

        public async Task<TodoModel> AddTodoItemAsync(TodoModel todoItem)
        {
            _context.ToDoItems.Add(todoItem);
            await _context.SaveChangesAsync();
            return todoItem;
        }

        public async Task UpdateTodoItemAsync(TodoModel todoItem)
        {
            _context.Entry(todoItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTodoItemAsync(int id)
        {
            var todoItem = await _context.ToDoItems.FindAsync(id);
            if (todoItem != null)
            {
                _context.ToDoItems.Remove(todoItem);
                await _context.SaveChangesAsync();
            }
        }
	}
}