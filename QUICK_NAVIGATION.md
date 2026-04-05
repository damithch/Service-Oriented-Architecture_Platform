# KMC Event Platform - Quick Navigation Guide

**Welcome!** This guide helps you navigate the KMC Event Platform project quickly.

## 🚀 I Just Want to Get Started

```
Start here: GETTING_STARTED.md
│
├─ Backend: Follow 3 steps
│  └─ Terminal: cd backend && dotnet run
│
├─ Frontend: Follow 3 steps
│  └─ Terminal: cd frontend && npm install && npm start
│
└─ Success! Open http://localhost:3000
```

**Time needed**: 5 minutes

---

## 📚 Quick Navigation

### For Different Roles

#### 👨‍💻 Backend Developer
1. **First read**: `DEVELOPMENT.md` (Architecture overview)
2. **Setup**: `backend/README.md` + `GETTING_STARTED.md`
3. **Code**: See `backend/src/` directory
4. **API Docs**: Run backend, visit `http://localhost:5000/swagger`
5. **Database**: MongoDB Atlas (connection in `appsettings.json`)

**Key Files to Study**:
- `backend/src/Services/EventService.cs` - Business logic
- `backend/src/Data/EventRepository.cs` - Database queries
- `backend/src/Controllers/EventsController.cs` - API endpoints

#### 🎨 Frontend Developer
1. **First read**: `frontend/FRONTEND_README.md`
2. **Setup**: `GETTING_STARTED.md` (Frontend section)
3. **Code**: See `frontend/src/` directory
4. **Components**: `frontend/src/components/` and `frontend/src/pages/`
5. **Styling**: `frontend/src/styles/`

**Key Files to Study**:
- `frontend/src/App.tsx` - Routing setup
- `frontend/src/services/EventApiService.ts` - API integration
- `frontend/src/pages/HomePage.tsx` - Example page component

#### 🏗️ Full-Stack Developer
1. **First read**: `ARCHITECTURE.md` (System overview)
2. **Then read**: `GETTING_STARTED.md` (Setup both layers)
3. **Understand**: `DEVELOPMENT.md` (Patterns and principles)
4. **Code**: Both `backend/` and `frontend/` directories

#### 🐳 DevOps/Infrastructure
1. **First read**: `ARCHITECTURE.md` (Deployment section)
2. **Files needed**: `docker-compose.yml`, `Dockerfile`
3. **Deployment**: `GETTING_STARTED.md` (Docker Compose section)
4. **Config**: Backend `appsettings.json`, Frontend `.env`

#### 📊 Project Manager
1. **First read**: `COMPLETION_SUMMARY.md` (Status overview)
2. **Features**: `PROJECT_SUMMARY.md` (What was built)
3. **Status**: `README.md` (Project description)
4. **Demo**: Run frontend at `http://localhost:3000`

---

## 📖 Documentation by Topic

### Project Overview
- `README.md` - Start here, 3-line tech stack, overview
- `COMPLETION_SUMMARY.md` - What was delivered, metrics
- `PROJECT_SUMMARY.md` - Features and architecture
- `FILE_INVENTORY.md` - All files explained

### Setup & Getting Started
- `GETTING_STARTED.md` - Complete setup guide (Backend + Frontend)
- `QUICKSTART.md` - Quick reference, API examples
- `backend/README.md` - Backend-specific documentation
- `frontend/FRONTEND_README.md` - Frontend-specific documentation

### Development
- `DEVELOPMENT.md` - Patterns, architecture, data models
- `ARCHITECTURE.md` - System design, deployment, scaling
- Code comments in source files

### API Reference
- Swagger UI: `http://localhost:5000/swagger` (after running backend)
- `QUICKSTART.md` - cURL examples for all endpoints

---

## 🗂️ Project Structure at a Glance

```
Service-Oriented Architecture_Platform/
│
├── Backend (ASP.NET Core 8.0)
│   └── backend/src/
│       ├── Models/ (Event, Participant)
│       ├── Data/ (Repositories)
│       ├── Services/ (Business logic)
│       └── Controllers/ (18 API endpoints)
│
├── Frontend (React 18 + TypeScript)
│   └── frontend/src/
│       ├── components/ (Navigation, Footer, EventCard)
│       ├── pages/ (7 pages)
│       ├── services/ (EventApiService, AuthService)
│       └── styles/ (Global CSS)
│
├── Database
│   └── MongoDB Atlas Cloud
│       └── Connection in backend/appsettings.json
│
└── Documentation
    ├── Setup Guides (GETTING_STARTED.md)
    ├── Architecture (ARCHITECTURE.md)
    ├── Development (DEVELOPMENT.md)
    └── API Reference (QUICKSTART.md)
```

---

## ⚡ 5-Minute Quick Start

### Terminal 1 - Backend
```bash
cd backend
dotnet restore
dotnet build
dotnet run
# Wait for: "Now listening on: http://localhost:5000"
```

### Terminal 2 - Frontend
```bash
cd frontend
npm install
npm start
# Browser opens automatically at http://localhost:3000
```

### Test the Integration
1. Open `http://localhost:3000` in browser
2. Click "Sign Up"
3. Register a new account
4. Create an event (if you chose "Organizer" role)
5. Browse events on home page
6. Register for an event

**Success!** You have the complete system running.

---

## 📚 Documentation Quick Links

| Document | Purpose | Read Time |
|----------|---------|-----------|
| `README.md` | Overview & tech stack | 2 min |
| `GETTING_STARTED.md` | Setup both backend & frontend | 5 min |
| `QUICKSTART.md` | API reference with examples | 5 min |
| `DEVELOPMENT.md` | Architecture & patterns | 15 min |
| `ARCHITECTURE.md` | System design & deployment | 20 min |
| `COMPLETION_SUMMARY.md` | Project completion report | 10 min |
| `PROJECT_SUMMARY.md` | Feature documentation | 10 min |
| `FILE_INVENTORY.md` | All files explained | 5 min |

---

## 🔍 Find Something Specific

### "How do I..."

#### General Questions
- **Understand the project?** → Start with `README.md`
- **Set up the project?** → Read `GETTING_STARTED.md`
- **Deploy to production?** → Read `ARCHITECTURE.md` deployment section
- **Find a specific file?** → Check `FILE_INVENTORY.md`

#### Backend Questions
- **Add a new API endpoint?** → Guide in `DEVELOPMENT.md`
- **Understand the database?** → Read `DEVELOPMENT.md` data section
- **Fix a bug?** → Check `backend/src/Services/`
- **See API documentation?** → Run backend, visit Swagger UI at `:5000/swagger`

#### Frontend Questions
- **Add a new page?** → Create in `frontend/src/pages/`
- **Add a new component?** → Create in `frontend/src/components/`
- **Change styling?** → Edit `frontend/src/styles/App.css`
- **Fix an API error?** → Check `frontend/src/services/EventApiService.ts`

#### Database Questions
- **Change MongoDB connection?** → Edit `backend/appsettings.json`
- **See all collections?** → Visit MongoDB Atlas UI online
- **Understand the schema?** → Read `DEVELOPMENT.md` schema section

---

## 🎯 Common Tasks

### Add a New Feature to Event
**Example: Add "capacity percentage" display**

1. **Backend**: Update `Event.cs` model (if needed)
2. **Backend**: Update `EventRepository.cs` query
3. **Backend**: Update `EventService.cs` logic
4. **Backend**: Update `EventsController.cs` response
5. **Frontend**: Update `EventDto` interface in `types/index.ts`
6. **Frontend**: Update component (e.g., `EventCard.tsx`)

### Create a New API Endpoint
**Example: `GET /api/events/trending`**

1. **Backend**: Add method to `EventRepository.cs`
2. **Backend**: Add method to `EventService.cs`
3. **Backend**: Add method to `EventsController.cs`
4. **Frontend**: Add method to `EventApiService.ts`
5. **Frontend**: Create page or component using the service
6. **Add route** in `App.tsx` if it's a new page

### Deploy the Application
1. **Build backend**: `dotnet publish -c Release`
2. **Build frontend**: `npm run build`
3. **Or use Docker**: `docker-compose up --build`
4. See details in `ARCHITECTURE.md` deployment section

---

## 🐛 Troubleshooting

### Backend Issues
| Problem | Solution |
|---------|----------|
| Port 5000 in use | Use different port: `dotnet run --urls "http://localhost:5001"` |
| MongoDB connection error | Check `appsettings.json` connection string |
| NuGet packages fail | Run `dotnet clean && dotnet restore` |

### Frontend Issues
| Problem | Solution |
|---------|----------|
| Port 3000 in use | Use `PORT=3001 npm start` |
| npm install fails | Run `npm cache clean --force` then retry |
| API calls fail | Check `.env` file has correct `REACT_APP_API_BASE_URL` |

### Integration Issues
| Problem | Solution |
|---------|----------|
| Frontend can't reach backend | Ensure backend runs first on port 5000 |
| CORS errors | Backend has CORS configured (see `Program.cs`) |
| Data not persisting | Verify MongoDB connection is working |

---

## 🚀 Next Steps After Setup

### Development
1. Clone/fork the repository
2. Create a feature branch
3. Make changes (backend and/or frontend)
4. Test locally
5. Commit and push
6. Create pull request

### Testing
1. **Manual testing**: Click through the UI
2. **API testing**: Use Swagger UI at `:5000/swagger`
3. **Database testing**: Check MongoDB Atlas UI
4. **Integration testing**: Test full workflows (signup → create event → register)

### Deployment
1. Build both backend and frontend
2. Use `docker-compose up` for automated deployment
3. Or manually deploy to cloud provider
4. See `ARCHITECTURE.md` for detailed deployment guides

---

## 📞 Getting Help

### For Setup Issues
→ Read `GETTING_STARTED.md` TroubleShooting section

### For Architecture Questions
→ Read `ARCHITECTURE.md` or `DEVELOPMENT.md`

### For API Questions
→ Visit Swagger UI at `http://localhost:5000/swagger`

### For Code Questions
→ Check source code comments and documentation in respective files

### For Deployment
→ Read `ARCHITECTURE.md` Deployment sections

---

## ✅ Project Status

- ✅ **Backend**: Complete (18 API endpoints)
- ✅ **Frontend**: Complete (7 pages, 3 components)
- ✅ **Database**: Configured (MongoDB Atlas)
- ✅ **Documentation**: Complete (8 guides)
- ✅ **Containerization**: Ready (Docker Compose)

**The project is production-ready!** 🎉

---

## 📋 Checklist for Different Scenarios

### "I want to run it locally"
- [ ] Clone repository
- [ ] Read `GETTING_STARTED.md`
- [ ] Start backend: `cd backend && dotnet run`
- [ ] Start frontend: `cd frontend && npm install && npm start`
- [ ] Test: Visit `http://localhost:3000`

### "I want to deploy it"
- [ ] Read `ARCHITECTURE.md`
- [ ] Review `docker-compose.yml`
- [ ] Update configuration (connection strings, etc.)
- [ ] Run: `docker-compose up --build`
- [ ] Verify: Check both services are running

### "I want to develop features"
- [ ] Read `DEVELOPMENT.md`
- [ ] Understand architecture
- [ ] Create feature branch
- [ ] Make changes (backend and/or frontend)
- [ ] Test thoroughly
- [ ] Push changes

### "I want to understand the code"
- [ ] Read `README.md`
- [ ] Read `DEVELOPMENT.md`
- [ ] Read `ARCHITECTURE.md`
- [ ] Study backend code in `backend/src/`
- [ ] Study frontend code in `frontend/src/`
- [ ] Check file comments and documentation

---

## 🎓 Learning Paths

### Backend Learning Path
1. `DEVELOPMENT.md` - Understand architecture
2. `backend/src/Models/` - Entity definitions
3. `backend/src/Data/` - Repository pattern
4. `backend/src/Services/` - Business logic
5. `backend/src/Controllers/` - API endpoints
6. `ARCHITECTURE.md` - System design

### Frontend Learning Path
1. `frontend/FRONTEND_README.md` - Overview
2. `frontend/src/App.tsx` - Routing setup
3. `frontend/src/components/` - Component examples
4. `frontend/src/pages/` - Page structure
5. `frontend/src/services/` - API integration
6. `ARCHITECTURE.md` - System design

### Full-Stack Learning Path
1. `README.md` - Overview
2. `ARCHITECTURE.md` - System design
3. `DEVELOPMENT.md` - Architecture & patterns
4. Backend code → Frontend code → Integration

---

## 📞 Support Resources

| Resource | Purpose |
|----------|---------|
| `GETTING_STARTED.md` | Setup and troubleshooting |
| `QUICKSTART.md` | API quick reference |
| Swagger UI (`:5000/swagger`) | Interactive API docs |
| GitHub Issues | Bug reports and features |
| Comments in code | Implementation details |

---

**Last Updated**: 2025  
**Project Status**: Complete ✅  
**Ready for**: Development, Testing, Deployment

Happy coding! 🚀
