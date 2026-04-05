# Getting Started - KMC Event Platform

Complete guide to set up and run the full-stack KMC Event Platform application.

## System Requirements

### Backend
- **.NET SDK 8.0** or higher
- **Docker** & **Docker Compose** (optional, for containerization)
- **Git** for version control

### Frontend
- **Node.js 16** or higher (Node.js 18+ recommended)
- **npm 8** or higher (comes with Node.js)

### External Services
- **MongoDB Atlas** Cloud Database (already configured with connection string)

## Quick Start (5 minutes)

### 1. Backend Setup

#### Option A: Native .NET Runtime

```bash
# Navigate to backend directory
cd backend

# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run --project src/KMCEventPlatform.csproj

# Backend will be available at: http://localhost:5000
# Swagger UI at: http://localhost:5000/swagger
```

#### Option B: Docker Compose (Recommended)

```bash
# From project root directory
docker-compose up --build

# Backend will be available at: http://localhost:5000
# MongoDB will run in a container
# Swagger UI at: http://localhost:5000/swagger
```

### 2. Frontend Setup

```bash
# Navigate to frontend directory
cd frontend

# Install dependencies
npm install

# Start development server
npm start

# Frontend will open automatically at: http://localhost:3000
```

## Architecture Overview

```
┌─────────────────────────────────────────────────────────────┐
│                     KMC Event Platform                       │
├─────────────────────────────────────────────────────────────┤
│                                                               │
│  ┌──────────────────────┐        ┌──────────────────────┐   │
│  │   React Frontend     │        │   ASP.NET Core API   │   │
│  │  (Port 3000)         │◄──────►│  (Port 5000)         │   │
│  │                      │        │                      │   │
│  │ - Event Listing      │        │ GET /api/events      │   │
│  │ - Search Events      │        │ POST /api/events     │   │
│  │ - User Registration  │        │ PUT /api/events/{id} │   │
│  │ - Event Details      │        │ DELETE /api/events   │   │
│  │ - Organizer Dashboard│        │ And 14 more...       │   │
│  └──────────────────────┘        └──────────────────────┘   │
│          ▲                                   ▲                │
│          │                                   │                │
│          │      Axios HTTP Calls             │ .NET Services │
│          │      TypeScript Interfaces        │ Repositories  │
│          │                                   │ Auto Mapper   │
│          │                                   ▼                │
│          │        ┌────────────────────────────────────┐    │
│          │        │   MongoDB Atlas Database           │    │
│          │        │  (mongodb+srv://...)              │    │
│          │        │  Database: newone                 │    │
│          │        │  Collections: events, participants│    │
│          └────────┤  registrations                    │    │
│                   └────────────────────────────────────┘    │
│                                                               │
└─────────────────────────────────────────────────────────────┘
```

## Project Directory Structure

```
Service-Oriented Architecture_Platform/
├── backend/                          # ASP.NET Core Backend
│   ├── src/
│   │   ├── KMCEventPlatform.csproj
│   │   ├── Models/                   # Domain entities
│   │   ├── Data/                     # Repository layer
│   │   ├── Services/                 # Business logic
│   │   ├── Controllers/              # API endpoints
│   │   ├── Program.cs                # DI & configuration
│   │   └── appsettings.json          # MongoDB connection
│   ├── KMCEventPlatform.sln
│   ├── Dockerfile
│   └── README.md                     # Backend documentation
│
├── frontend/                         # React Frontend
│   ├── public/
│   │   └── index.html
│   ├── src/
│   │   ├── components/               # React components
│   │   ├── pages/                    # Page components
│   │   ├── services/                 # API client
│   │   ├── types/                    # TypeScript interfaces
│   │   ├── utils/                    # Utilities
│   │   ├── styles/                   # CSS files
│   │   ├── App.tsx
│   │   └── index.tsx
│   ├── package.json
│   ├── tsconfig.json
│   ├── .env
│   └── FRONTEND_README.md            # Frontend documentation
│
├── docker-compose.yml                # Container orchestration
├── README.md                         # Project overview
├── QUICKSTART.md                     # Quick reference
├── DEVELOPMENT.md                    # Development guide
└── Getting_Started.md                # This file
```

## Core Features

### 📅 Event Management
- ✅ Create, read, update, delete events
- ✅ Advanced search (title, category, date range)
- ✅ Event organizer management
- ✅ Participant registration/unregistration
- ✅ Capacity management

### 👤 User Management
- ✅ User registration (Regular/Organizer)
- ✅ Authentication (Email/Password)
- ✅ Role-based access control
- ✅ User profile management
- ✅ Event history tracking

### 📊 Organizer Dashboard
- ✅ Event CRUD operations
- ✅ Registration management
- ✅ Event analytics
- ✅ Participant list viewing

## Configuration

### Environment Variables

#### Backend (appsettings.json)
```json
{
  "MongoDB": {
    "Connection": "mongodb+srv://awantha:ABdgpqSthXmK1k8K@cluster0wolfenox.aza9jrr.mongodb.net/newone",
    "Database": "newone"
  }
}
```

#### Frontend (.env)
```
REACT_APP_API_BASE_URL=http://localhost:5000/api
```

## API Documentation

### Base URL
```
http://localhost:5000/api
```

### Main Endpoints

#### Events
- `GET /events` - Get all events
- `GET /events/{id}` - Get event by ID
- `POST /events` - Create event
- `PUT /events/{id}` - Update event
- `DELETE /events/{id}` - Delete event
- `GET /events/search/title/{title}` - Search by title
- `GET /events/search/category/{category}` - Filter by category
- `GET /events/search/daterange` - Filter by date range
- `POST /events/{eventId}/register/{participantId}` - Register for event
- `DELETE /events/{eventId}/unregister/{participantId}` - Unregister from event

#### Participants
- `GET /participants` - Get all participants
- `GET /participants/{id}` - Get participant by ID
- `GET /participants/email/{email}` - Get by email
- `POST /participants` - Create participant
- `PUT /participants/{id}` - Update participant
- `DELETE /participants/{id}` - Delete participant

**Full API documentation available at:** `http://localhost:5000/swagger`

## Development Workflow

### Working on Backend
```bash
# Terminal 1: Start backend
cd backend
dotnet run

# Backend runs on http://localhost:5000
# API docs on http://localhost:5000/swagger
```

### Working on Frontend
```bash
# Terminal 2: Start frontend
cd frontend
npm start

# Frontend runs on http://localhost:3000
# Hot reload enabled for development
```

### Testing the Integration

1. **Create an Event** (using Swagger or Frontend)
   - POST /api/events with event details

2. **Register for Event** (Frontend)
   - Login/Register as user
   - Browse events on homepage
   - Click "Register" on an event

3. **Organizer Dashboard** (Frontend)
   - Login as organizer role user
   - Click "Dashboard" in navigation
   - Create, edit, delete events

## Deployment

### Production Build - Backend
```bash
cd backend
dotnet publish -c Release
```

### Production Build - Frontend
```bash
cd frontend
npm run build
# Output in build/ directory - ready to serve with any static server
```

### Docker Containerization
```bash
# Using docker-compose
docker-compose up --build -d

# Check running services
docker-compose ps

# View logs
docker-compose logs -f

# Stop services
docker-compose down
```

## Troubleshooting

### Backend Issues

#### Port 5000 Already in Use
```bash
# Kill process on port 5000 (Windows PowerShell)
Get-Process | Where-Object {$_.Handles -eq 123456} | Stop-Process

# Or use different port
dotnet run --urls "http://localhost:5001"
```

#### MongoDB Connection Error
- Verify internet connection (cloud database)
- Check MongoDB Atlas whitelist IP: https://cloud.mongodb.com
- Verify connection string in appsettings.json

#### CORS Error in Frontend
- Ensure backend has CORS configured in Program.cs
- Check frontend .env for correct API URL

### Frontend Issues

#### Node Modules Installation Fails
```bash
# Clear npm cache
npm cache clean --force

# Delete node_modules
rm -r node_modules
rm package-lock.json

# Reinstall
npm install
```

#### Port 3000 Already in Use
```bash
# Use different port
PORT=3001 npm start
```

#### REACT_APP Variables Not Loading
- Add variables to .env file
- Restart frontend development server (npm start)
- React builds the env vars at compile time

## Performance Considerations

### Backend
- MongoDB Atlas auto-scaling
- Connection pooling configured
- Async/await for I/O operations
- Swagger documentation for API exploration

### Frontend
- React Router code splitting
- Lazy loading components
- Bootstrap responsive grid
- Optimized Axios requests

## Next Steps

1. **Read Full Backend Documentation:** [README.md](./README.md)
2. **Read Frontend Documentation:** [frontend/FRONTEND_README.md](./frontend/FRONTEND_README.md)
3. **Development Guide:** [DEVELOPMENT.md](./DEVELOPMENT.md)
4. **Quick Reference:** [QUICKSTART.md](./QUICKSTART.md)
5. **Project Summary:** [PROJECT_SUMMARY.md](./PROJECT_SUMMARY.md)

## Database Schema

### Events Collection
```javascript
{
  _id: ObjectId,
  title: string,
  description: string,
  organizerId: string,
  organizerName: string,
  startDate: ISODate,
  endDate: ISODate,
  location: string,
  category: string,
  maxParticipants: number,
  registeredParticipants: number,
  status: "Active" | "Completed" | "Cancelled",
  contactEmail: string,
  contactPhone: string,
  participantIds: [string],
  tags: [string]
}
```

### Participants Collection
```javascript
{
  _id: ObjectId,
  fullName: string,
  email: string,
  role: "Regular" | "Organizer" | "Admin",
  registeredEventIds: [string],
  organizedEventIds: [string]
}
```

## Tech Stack Summary

| Layer | Technology | Purpose |
|-------|-----------|---------|
| Frontend | React 18 + TypeScript | Modern UI library with type safety |
| Frontend Routing | React Router 6 | Client-side navigation |
| Frontend HTTP | Axios | HTTP client with interceptors |
| Frontend Styling | Bootstrap 5 + CSS | Responsive design framework |
| Backend API | ASP.NET Core 8 | RESTful API framework |
| Backend Services | C# | Business logic implementation |
| Data Access | MongoDB.Driver | Database operations |
| ORM | AutoMapper | Object mapping |
| API Docs | Swagger/OpenAPI | Interactive API documentation |
| Database | MongoDB Atlas | Cloud NoSQL database |
| Containerization | Docker | Application packaging |
| Orchestration | Docker Compose | Multi-container management |

## Support & Questions

Refer to the respective documentation files for detailed information:
- Backend issues → See [README.md](./README.md) and [DEVELOPMENT.md](./DEVELOPMENT.md)
- Frontend issues → See [frontend/FRONTEND_README.md](./frontend/FRONTEND_README.md)
- API behavior → See Swagger docs at `http://localhost:5000/swagger`

## License

CSE5013 - Service Oriented Computing Assessment
