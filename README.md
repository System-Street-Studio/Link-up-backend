📁 AnonChatApp (Solution)
│
├── 📁 Core (Domain / Application Layer)
│   ├── 📁 Entities       --> User.cs, Post.cs, ChatRoom.cs (Database Models)
│   ├── 📁 Interfaces     --> IUserRepository.cs, IChatHub.cs
│   └── 📁 DTOs           --> UserDto.cs, MessageDto.cs
│
├── 📁 Infrastructure (Data Layer)
│   ├── AppDbContext.cs   --> Entity Framework Core Configuration
│   ├── 📁 Repositories   --> Database queries ලියන තැන (EF Core & Dapper)
│   └── 📁 Services       --> Redis Cache Service, Password Hasher
│
└── 📁 WebAPI (Presentation Layer)
    ├── 📁 Controllers    --> AuthController.cs, PostController.cs
    ├── 📁 Hubs           --> ChatHub.cs, MatchmakingHub.cs (SignalR)
    ├── Program.cs        --> Dependency Injection & Middleware
    └── appsettings.json  --> Connection Strings 