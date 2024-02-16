document.addEventListener('DOMContentLoaded', function() {
    fetchTodos();

    document.getElementById('addTodoForm').addEventListener('submit', function(e) {
        e.preventDefault();
        addTodo();
    });
});

function fetchTodos()
{
    fetch('/api/TodoItems')
        .then(response => response.json())
        .then(data => {
            const tableBody = document.getElementById('todoTable').getElementsByTagName('tbody')[0];
            tableBody.innerHTML = ''; 
            data.forEach(item => {
            let row = tableBody.insertRow();
            let cell1 = row.insertCell(0);
            //生成checkbox
            let checkbox = document.createElement('input');
            checkbox.type = 'checkbox';
            checkbox.checked = item.isComplete;
            checkbox.addEventListener('change', () => toggleComplete(item, checkbox.checked)); // 新增事件監聽器
            cell1.appendChild(checkbox);

            //生成todo name
            let cell2 = row.insertCell(1);
            cell2.textContent = item.name;

            //生成刪除btn
            let cell3 = row.insertCell(2);
            cell3.innerHTML = `<button onclick="editTodo(${item.id})">Edit</button>`;

            //生成編輯btn
            let cell4 = row.insertCell(3);
            cell4.innerHTML = `<button onclick = "deleteTodo(${item.id})" > Delete </button>`;

            });
        });
}

function addTodo()
{
    const todoName = document.getElementById('todoName').value;
    fetch('/api/TodoItems', {
    method: 'POST',
        headers:
        {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ name: todoName, isComplete: false })
    })
    .then(response => {
         if (response.ok)
         {
             fetchTodos();
         }
     });
}

function deleteTodo(id)
{
    fetch(`/api/TodoItems/${id}`, {
    method: 'DELETE'
    })
    .then(response => {
         if (response.ok)
         {
             fetchTodos();
         }
     });
}

// 新增函數以處理checkbox的狀態切換
function toggleComplete(item, isComplete) {
    fetch(`/api/TodoItems/${item.id}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },

        body: JSON.stringify({ id: item.id, name: item.name, isComplete: isComplete })
    })
        .then(response => {
            if (response.ok) {
                console.log('Todo item updated successfully');
            } else {
                console.error('Failed to update todo item');
            }
        })
        .catch(error => console.error('Error:', error));
}
