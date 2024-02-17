## 專案介紹

這是一個簡單的TodoList應用，使用ASP.NET Core 3.0開發。它允許用戶添加、查看、修改以及刪除待辦事項。
![image](https://github.com/SkyKai1018/TodoListApp/assets/135136212/3c45fc19-8d6c-47d5-a785-c732421b2d6d)

## 功能

- **查看所有待辦事項**：列出所有添加的待辦事項。
- **添加新的待辦事項**：允許用戶輸入待辦事項並添加。
- **修改待辦事項**：用戶可以修改待辦事項的內容及狀態。
- **刪除待辦事項**：用戶可以刪除不再需要的待辦事項。

## 開發環境

- Visual Studio 2019
- ASP.NET Core 3.0
- LocalDB
- Entity Framework Core
- MSTest

## 資料夾結構
```
TodoListApp/
│
├── TodoListApp/ (主項目)
│ ├── Controllers/ (包含MVC控制器或WebApi控制器)
│ ├── Models/ (資料模型，包含與資料庫互動的程式碼，如DbContext)
│ ├── Views/ (MVC視圖，如果是WebApi專案則可能不需要此資料夾)
│ ├── Services/ (包含業務邏輯的服務層)
│ └── wwwroot/ (靜態文件，如JS、CSS和圖片等)
│
├── TodoListApp.Tests/ (單元測試項目)
│ ├── TodoItemsControllerTests.c (控制器測試)
│ └── TodoItemServiceTests.cs (TodoList CRUD 服務層測試)
│
└── TodoListApp.sln (解決方案檔)
```

## 單元測試

項目包含MSTest單元測試，測試基本Service增刪改查及controller功能。
![image](https://github.com/SkyKai1018/TodoListApp/assets/135136212/ba19301d-4c0f-4c09-adfd-b054ae27bf67)
