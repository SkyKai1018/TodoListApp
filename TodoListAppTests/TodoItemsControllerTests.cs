using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoListApp.Controllers;
using TodoListApp.Models;
using TodoListApp.Services;


namespace TodoListApp.Tests
{
    [TestClass]
    public class TodoItemsControllerTests
    {
        [TestMethod]
        public async Task GetTodoItems_ReturnsAllItems()
        {
            // Arrange
            var mockService = new Mock<ITodoItemService>();
            mockService.Setup(service => service.GetAllTodoItemsAsync())
                       .ReturnsAsync(new List<TodoModel>
                       {
                       new TodoModel { Id = 1, Name = "Test 1", IsComplete = false },
                       new TodoModel { Id = 2, Name = "Test 2", IsComplete = true }
                       });

            var controller = new TodoItemsController(mockService.Object);

            // Act
            var result = await controller.GetTodoItems();

            // Assert
            var actionResult = result.Result as OkObjectResult;
            Assert.IsNotNull(actionResult);
            var items = actionResult.Value as List<TodoModel>;
            Assert.AreEqual(2, items.Count);
        }

        [DataTestMethod]
        [DataRow(1, "Test 1", false)]
        [DataRow(2, "Test 2", true)]
        public async Task GetTodoItem_ReturnsItem_WhenItemExists(int todoId, string name, bool isComplete)
        {
            // Arrange
            var mockService = new Mock<ITodoItemService>();
            mockService.Setup(service => service.GetTodoItemByIdAsync(todoId))
                        .ReturnsAsync(new TodoModel { Id = todoId, Name = name, IsComplete = isComplete });

            var controller = new TodoItemsController(mockService.Object);

            // Act
            var result = await controller.GetTodoItem(todoId);

            // Assert
            var actionResult = result.Result as OkObjectResult;
            Assert.IsNotNull(actionResult);
            var item = actionResult.Value as TodoModel;
            Assert.IsNotNull(item);
            Assert.AreEqual(todoId, item.Id);
            Assert.AreEqual(name, item.Name);
            Assert.AreEqual(isComplete, item.IsComplete);
        }

        [TestMethod]
        public async Task GetTodoItem_ReturnsNotFound_WhenItemDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<ITodoItemService>();
            mockService.Setup(service => service.GetTodoItemByIdAsync(It.IsAny<int>()))
                        .ReturnsAsync((TodoModel)null);

            var controller = new TodoItemsController(mockService.Object);

            // Act
            var result = await controller.GetTodoItem(99); // Assuming this ID does not exist

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task PostTodoItem_CreatesNewItem()
        {
            // Arrange
            var mockService = new Mock<ITodoItemService>();
            var newItem = new TodoModel { Name = "New Item", IsComplete = false };
            mockService.Setup(service => service.AddTodoItemAsync(It.IsAny<TodoModel>()))
                        .ReturnsAsync((TodoModel todoItem) =>
                        {
                            todoItem.Id = 3; // Simulate setting ID after saving to the database
                        return todoItem;
                        });

            var controller = new TodoItemsController(mockService.Object);

            // Act
            var result = await controller.PostTodoItem(newItem);

            // Assert
            var actionResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(actionResult);
            var item = actionResult.Value as TodoModel;
            Assert.AreEqual(3, item.Id); // Verify that the ID was set
        }

        [TestMethod]
        public async Task PutTodoItem_ReturnsNoContent_WhenItemExistsAndIsUpdated()
        {
            // Arrange
            var todoId = 1;
            var existingTodoItem = new TodoModel { Id = todoId, Name = "Existing Item", IsComplete = false };
            var updatedTodoItem = new TodoModel { Id = todoId, Name = "Updated Item", IsComplete = true };

            var mockService = new Mock<ITodoItemService>();
            // Setup to return an existing item when requested
            mockService.Setup(service => service.GetTodoItemByIdAsync(todoId))
                       .ReturnsAsync(existingTodoItem);

            // Assuming the update operation completes successfully
            mockService.Setup(service => service.UpdateTodoItemAsync(It.IsAny<TodoModel>()))
                       .Returns(Task.CompletedTask);

            var controller = new TodoItemsController(mockService.Object);

            // Act
            var result = await controller.PutTodoItem(todoId, updatedTodoItem);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task PutTodoItem_ReturnsBadRequest_WhenItemIdDoesNotMatch()
        {
            // Arrange
            var todoId = 1;
            var mismatchedTodoItem = new TodoModel { Id = 2, Name = "Mismatched Item", IsComplete = false }; // Note the ID mismatch

            var mockService = new Mock<ITodoItemService>();
            var controller = new TodoItemsController(mockService.Object);

            // Act
            var result = await controller.PutTodoItem(todoId, mismatchedTodoItem);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task DeleteTodoItem_ReturnsNoContent_WhenItemExists()
        {
            // Arrange
            int todoId = 1;
            var mockService = new Mock<ITodoItemService>();
            mockService.Setup(service => service.GetTodoItemByIdAsync(todoId))
                        .ReturnsAsync(new TodoModel { Id = todoId });
            mockService.Setup(service => service.DeleteTodoItemAsync(todoId))
                        .Returns(Task.CompletedTask);

            var controller = new TodoItemsController(mockService.Object);

            // Act
            var result = await controller.DeleteTodoItem(todoId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task DeleteTodoItem_ReturnsNotFound_WhenItemDoesNotExist()
        {
            // Arrange
            int todoId = 99; // Assuming this ID does not exist
            var mockService = new Mock<ITodoItemService>();
            mockService.Setup(service => service.GetTodoItemByIdAsync(todoId))
                        .ReturnsAsync((TodoModel)null);

            var controller = new TodoItemsController(mockService.Object);

            // Act
            var result = await controller.DeleteTodoItem(todoId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

    }
}