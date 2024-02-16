using System.Collections.Generic;
using System.Threading.Tasks;
using TodoListApp.Models;
namespace TodoListApp.Services
{
    public interface ITodoItemService
    {
        Task<IEnumerable<TodoModel>> GetAllTodoItemsAsync();
        Task<TodoModel> GetTodoItemByIdAsync(int id);
        Task<TodoModel> AddTodoItemAsync(TodoModel todoItem);
        Task UpdateTodoItemAsync(TodoModel todoItem);
        Task DeleteTodoItemAsync(int id);
    }
}