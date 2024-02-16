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
            //�ͦ�checkbox
            let checkbox = document.createElement('input');
            checkbox.type = 'checkbox';
            checkbox.checked = item.isComplete;
            checkbox.disabled = true;
            cell1.appendChild(checkbox);

            //�ͦ�todo name
            let cell2 = row.insertCell(1);
            cell2.textContent = item.name;

            //�ͦ��R��btn
            let cell3 = row.insertCell(2);
            cell3.innerHTML = `<button onclick="deleteTodo(${item.id})">Edit</button>`;
            //�ͦ��s��btn
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
