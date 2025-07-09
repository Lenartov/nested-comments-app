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

---

## 🚀 Deployment Instructions

### 📁 Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/)
- [Node.js + Angular CLI](https://angular.io/cli)
- [Docker](https://www.docker.com/)
- SQL Server instance (local or cloud, e.g., Azure SQL)

---

### 🧪 1. Run EF Core Migrations (if needed)

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

---

### 🐳 1. Build and Run Docker

```bash
# From the NestedComments.Api project root
docker build -t nested-comments-api .
docker run -p 5000:80 nested-comments-api
```

> Ensure your `appsettings.Production.json` contains the correct SQL connection string and CORS settings.

---

### 💻 3. Run Angular Frontend (Locally)

```bash
cd nested-comments-app/comments-app/
npm install
ng serve
```
---

### 🌐 Production Deployment
You can deploy both frontend and backend to platforms like **Render**, **Vercel**, or **Azure**. Make sure to:
- Set proper **CORS** policies and **API base URLs** in production config files.

