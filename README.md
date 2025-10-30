<h1 align="center">🫧 Custom C# Web Server</h1>
<h3 align="center">Routing • Sessions • Authorization</h3>

<p align="center">
  <img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white" alt="C#" />
  <img src="https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET" />
  <img src="https://img.shields.io/badge/HttpListener-008080?style=for-the-badge&logo=serverless&logoColor=white" alt="HttpListener" />
  <img src="https://img.shields.io/badge/Soft%20UI%20Theme-00bfa6?style=for-the-badge&logo=css3&logoColor=white" alt="Soft UI" />
  <img src="https://img.shields.io/badge/Status-Online-success?style=for-the-badge&logo=githubactions&logoColor=white" alt="Status" />
</p>

<p align="center">
  <img src="https://readme-typing-svg.herokuapp.com?font=Fira+Code&pause=1000&color=22D3EE&center=true&vCenter=true&width=600&lines=Low-Level+Custom+Web+Server+in+C%23;Routing+%7C+Sessions+%7C+Authorization;Built+on+.NET+HttpListener;Soft+UI+Bubble+Theme+%F0%9F%8C%88" alt="Typing Animation" />
</p>

---

## 📖 **Project Overview**

 🧩 A handcrafted web server built **from scratch** in C#, using only  
 `System.Net.HttpListener` — demonstrating core web concepts like **Routing**, **Sessions**, and **Authorization**  
 without any external frameworks such as ASP.NET.

Every page is wrapped in a consistent **Soft UI aesthetic** — soft borders, glowing shadows, and elegant spacing for a modern “bubbling” interface 💫

---

## 🛠️ **Tech Stack & Core Libraries**

| 🔧 Category | ⚙️ Technology | 🧾 Notes |
|--------------|---------------|----------|
| **Language** | ![C#](https://img.shields.io/badge/C%23-239120?style=flat&logo=csharp&logoColor=white) | Core application logic |
| **Platform** | ![.NET](https://img.shields.io/badge/.NET-512BD4?style=flat&logo=dotnet&logoColor=white) | Compilation & runtime |
| **Networking** | ![HttpListener](https://img.shields.io/badge/System.Net.HttpListener-008080?style=flat&logo=serverless&logoColor=white) | Handles HTTP requests |
| **Session Data** | 🧠 `Dictionary<string, string>` | Stores per-user session info |
| **UI Styling** | 🎨 Embedded CSS | Smooth corners, shadows, and gradients |



Client ─▶ Listener ─▶ Router ─▶ Handler
│          │           │
│          │           ├─▶ Serves HTML/CSS (Static)
│          │           └─▶ Executes C# Action
│          └─▶ SessionManager (Cookie + Data)
└─▶ Receives Response (Soft UI Page)


---

## 🧩 **Core Architecture**

### 🧠 1. Core Listener — `Server.cs`
- Asynchronous loop using `await listener.GetContextAsync()`
- Checks for or creates `SessionID` cookies
- Stores session data in-memory via `SessionManager`

---

### 🚦 2. Request Routing — `Route.cs` & `Router.cs`
- Matches URLs (`/admin`, `/submit`, etc.) to C# handlers  
- Default fallback to `/Website/` folder (`index.html`, `404.html`)

---

### 🔐 3. Session & Authorization Flow
- Routes can declare `RequiresAuthorization = true`
- Session checked for `IsLoggedIn = true`
- `/submit` POST handler simulates login
- Unauthorized users redirected to `/login`

---

## 🎨 **The "Bubbling Site" Aesthetic**

| 🌈 Page | 💫 Theme | 🎯 Purpose |
|---------|-----------|------------|
| `/submit` (Login) | 🩵 Soft Blue-Green | Friendly input & success |
| `/admin` (Protected) | 💚 Soft Green | Authorized access |
| `/login` (Denied) | ❤️ Soft Red | Calm error feedback |
| `index.html` / `404.html` | ⚪ Neutral Gray | Unified user experience |


## 🚀 **Run Locally**

### 🔧 Requirements

* 💻 [.NET SDK](https://dotnet.microsoft.com/download)

### ⚙️ Setup Steps

```bash
# 1️⃣ Clone Repository
git clone https://github.com/Ayomide-R/BasicWebServer
cd Custom-CSharp-WebServer

# 2️⃣ Restore Dependencies
dotnet restore

# 3️⃣ Run
dotnet run
```

Server will start at 👉 **[http://localhost:8080/](http://localhost:8080/)**

---

## 🌍 **Available Routes**

| 🌐 Route  | 🧠 Function              | 🔒 Authorization   |
| --------- | ------------------------ | ------------------ |
| `/`       | Serves `index.html`      | Open               |
| `/submit` | Handles login simulation | Open               |
| `/admin`  | Protected area           | ✅ Requires Session |
| `*`       | Custom `404.html`        | Open               |

---

## 📁 **Directory Layout**

```
📦 Custom-CSharp-WebServer
 ┣ 📜 Server.cs
 ┣ 📜 Route.cs
 ┣ 📜 Router.cs
 ┣ 📜 SessionManager.cs
 ┣ 📂 Website/
 ┃ ┣ 🗎 index.html
 ┃ ┣ 🗎 login.html
 ┃ ┣ 🗎 admin.html
 ┃ ┗ 🗎 404.html
 ┗ 📜 Program.cs
```


<p align="center">
  <img src="https://github.com/Ayomide-R/Custom-CSharp-WebServer/assets/animation/server-flow.gif" width="700" alt="Server Flow Animation">
</p>


## 💡 **Key Highlights**

✅ Built with **pure C#** – no frameworks
✅ Custom **session and cookie** management
✅ Elegant **Soft UI** on every page
✅ Asynchronous, non-blocking server loop
✅ Educational reference for **low-level web architecture**

---

## 🧑‍💻 **Author**

**Ayomide Roy**
🔗 [GitHub Profile](https://github.com/Ayomide-R)
💬 “Because even servers deserve a little style.”

---

<p align="center">
  <img src="https://forthebadge.com/images/badges/made-with-c-sharp.svg" />
  <img src="https://forthebadge.com/images/badges/uses-dotnet.svg" />
  <img src="https://forthebadge.com/images/badges/built-with-love.svg" />
</p>

<p align="center">
  <b>🫧 Built in C# • Styled with Soft UI • Powered by Pure Code </b><br>
  <sub>— Because handcrafted servers are art —</sub>
</p>
