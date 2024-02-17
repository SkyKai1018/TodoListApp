using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;
using TodoListApp.Models;
using TodoListApp.Services;

namespace TodoListApp.Tests
{
	[TestClass]
    public class TodoItemServiceTests
    {
        [TestMethod]
        public async Task GetAllTodoItemsAsync_ReturnsAllItems()
        {
            // 使用InMemory數據庫配置DbContext
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            // 使用InMemory數據庫上下實例化服務
            using (var context = new ApplicationDbContext(options))
            {
                context.ToDoItems.Add(new TodoModel { Id = 1, Name = "Test 1", IsComplete = false });
                context.ToDoItems.Add(new TodoModel { Id = 2, Name = "Test 2", IsComplete = true });
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new TodoItemService(context);
                var items = await service.GetAllTodoItemsAsync();

                Assert.AreEqual(2, items.Count());
            }
        }

        [TestMethod]
        public async Task GetTodoItemByIdAsync_ReturnsItem_WhenItemExists()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_GetById")
                .Options;

            // Insert seed data into the database using one instance of the context
            using (var context = new ApplicationDbContext(options))
            {
                context.ToDoItems.Add(new TodoModel { Id = 1, Name = "Test Item", IsComplete = false });
                await context.SaveChangesAsync();
            }

            // Use a clean instance of the context to run the test
            using (var context = new ApplicationDbContext(options))
            {
                var service = new TodoItemService(context);
                var item = await service.GetTodoItemByIdAsync(1);

                Assert.IsNotNull(item);
                Assert.AreEqual("Test Item", item.Name);
            }
        }

        [TestMethod]
        public async Task AddTodoItemAsync_AddsItemCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_AddItem")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var service = new TodoItemService(context);
                await service.AddTodoItemAsync(new TodoModel { Name = "New Item", IsComplete = false });

                Assert.AreEqual(1, context.ToDoItems.Count());
                Assert.AreEqual("New Item", context.ToDoItems.Single().Name);
            }
        }

        [TestMethod]
        public async Task UpdateTodoItemAsync_UpdatesItemCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_UpdateItem")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var originalItem = new TodoModel { Id = 1, Name = "Original Item", IsComplete = false };
                context.ToDoItems.Add(originalItem);
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new TodoItemService(context);
                var updatedItem = new TodoModel { Id = 1, Name = "Updated Item", IsComplete = true };
                await service.UpdateTodoItemAsync(updatedItem);

                var item = context.ToDoItems.Find(1);
                Assert.AreEqual("Updated Item", item.Name);
                Assert.IsTrue(item.IsComplete);
            }
        }

        [TestMethod]
        public async Task DeleteTodoItemAsync_DeletesItemCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_DeleteItem")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var itemToDelete = new TodoModel { Id = 1, Name = "Item to Delete", IsComplete = false };
                context.ToDoItems.Add(itemToDelete);
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new TodoItemService(context);
                await service.DeleteTodoItemAsync(1);

                Assert.AreEqual(0, context.ToDoItems.Count());
            }
        }
    }
}
