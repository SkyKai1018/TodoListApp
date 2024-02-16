using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoListApp.Models;
using TodoListApp.Services;

namespace TodoListApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodoItemService _todoItemService;

        public TodoItemsController(ITodoItemService todoItemService)
        {
            _todoItemService = todoItemService;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoModel>>> GetTodoItems()
        {
            var todoItem = await _todoItemService.GetAllTodoItemsAsync();
            return Ok(todoItem);
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoModel>> GetTodoItem(int id)
        {
            var todoItem = await _todoItemService.GetTodoItemByIdAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        // POST: api/TodoItems
        [HttpPost]
        public async Task<ActionResult<TodoModel>> PostTodoItem(TodoModel todoItem)
        {
            var createdTodoItem = await _todoItemService.AddTodoItemAsync(todoItem);
            return CreatedAtAction(nameof(GetTodoItem), new { id = createdTodoItem.Id }, createdTodoItem);
        }

        // PUT: api/TodoItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(int id, TodoModel todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            try
            {
                await _todoItemService.UpdateTodoItemAsync(todoItem);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await TodoItemExists(id)))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private async Task<bool> TodoItemExists(int id)
        {
            var todoItem = await _todoItemService.GetTodoItemByIdAsync(id);
            return todoItem != null;
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(int id)
        {
            var todoItem = await _todoItemService.GetTodoItemByIdAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            await _todoItemService.DeleteTodoItemAsync(id);
            return NoContent();
        }
    }
}
