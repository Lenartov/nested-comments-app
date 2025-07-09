# 🧩 Nested Comments Web App

This is a full-stack web application that allows users to post, view, and reply to comments in a nested (threaded) format. The app supports image uploads, HTML message formatting, real-time updates via WebSockets, and various security features.

## 🌐 Technologies Used

- **Frontend**: Angular
- **Backend**: ASP.NET Core + Entity Framework Core
- **Database**: Azure SQL Server
- **Deployment**: Render
- **Containerization**: Docker

---

## ✨ Features

### 💬 Commenting System
- Nested (threaded) replies with infinite depth
- Infinite scroll for replies
- Sorting by date, username, or email
- Rich text input with support for **HTML tags**
- Live preview of the message before submission

### 🖼️ Media Uploads
- Upload images with each comment
- Preview images directly in the comment thread
- Automatic **resize** of large images on the server

### 🔐 Security and Validation
- CAPTCHA verification (custom implementation)
- Frontend and backend **validation** of all fields
- XSS and SQL Injection protection
- File extension whitelisting
- Origin-based CORS policy

### ⚙️ Backend Optimizations
- Comments are saved **in batches using a background queue**
- Cached database queries for performance
- Custom CAPTCHA implementation
- Dockerized backend with all dependencies included

### 🔁 Real-time Updates
- SignalR WebSocket integration
- Comments list updates **live** for all users when new messages arrive

---

Live Demo:
🌐 https://nested-comments-app.onrender.com
> ⚠️ Initial page load might be slow due to free-tier cold start on Render.
> Saved files can also disappear for the same reason.

---

## 🚀 Deployment Instructions

### 📁 Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/)
- [Node.js + Angular CLI](https://angular.io/cli)
  
---

###Frontend
```
cd nested-comments-app-main\comments-app
npm install
ng serve
```
> May need to be change apiUrl here: src/environments/environment.development.ts 

###Backend
Need to change Connection string & Corps:
  "ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=NestedCommentsDb;Trusted_Connection=True;TrustServerCertificate=True;" //your local DB
  },
  "AllowedHosts": "*",
    "Cors": {
      "AllowedOrigins": ["http://localhost:4200"] //your frontend addres
    }
```
cd NestedComments.Api\NestedComments.Api
dotnet restore
dotnet ef migrations add Initial
dotnet ef database update
dotnet run
```

### 🌐 Production Deployment
You can deploy both frontend and backend to platforms like **Render**, **Vercel**, or **Azure**. Make sure to:
- Set proper **CORS** policies and **API base URLs** in production config files.
- Also you can use Dockerfiles in project to make it.
