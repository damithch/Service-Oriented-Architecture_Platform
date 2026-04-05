# Architecture & Deployment Guide

## System Architecture

### High-Level Architecture Diagram

```
┌─────────────────────────────────────────────────────────────────────┐
│                    KMC Event Platform - Full Stack                  │
└─────────────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────────────────┐
│                          USER BROWSER                                │
│  ┌────────────────────────────────────────────────────────────────┐  │
│  │            React SPA (Single Page Application)                 │  │
│  │  ┌──────────────────────────────────────────────────────────┐  │  │
│  │  │  Routing Layer (React Router)                            │  │  │
│  │  │  HomePage → EventDetailsPage → SearchPage → Dashboard    │  │  │
│  │  └──────────────────────────────────────────────────────────┘  │  │
│  │  ┌──────────────────────────────────────────────────────────┐  │  │
│  │  │  Component Layer                                         │  │  │
│  │  │  Navigation | EventCard | Footer | Forms                 │  │  │
│  │  └──────────────────────────────────────────────────────────┘  │  │
│  │  ┌──────────────────────────────────────────────────────────┐  │  │
│  │  │  Service Layer                                           │  │  │
│  │  │  EventApiService (20+ methods) | AuthService            │  │  │
│  │  └──────────────────────────────────────────────────────────┘  │  │
│  └────────────────────────────────────────────────────────────────┘  │
│                     Port: 3000                                        │
└──────────────────────────────────────────────────────────────────────┘
                               │
                               │ HTTP/REST
                               │ Axios Client
                               │ Content-Type: application/json
                               ▼
┌──────────────────────────────────────────────────────────────────────┐
│                    ASP.NET Core 8.0 REST API                         │
│  ┌────────────────────────────────────────────────────────────────┐  │
│  │  Controller Layer                                              │  │
│  │  ┌──────────────────┐  ┌──────────────────┐                  │  │
│  │  │ EventsController │  │ParticipantsCtlr  │                  │  │
│  │  │  11 endpoints    │  │   7 endpoints    │                  │  │
│  │  └──────────────────┘  └──────────────────┘                  │  │
│  └────────────────────────────────────────────────────────────────┘  │
│  ┌────────────────────────────────────────────────────────────────┐  │
│  │  Service Layer                                                 │  │
│  │  ┌──────────────────────┐  ┌──────────────────────┐           │  │
│  │  │ EventService         │  │ ParticipantService   │           │  │
│  │  │ - CreateEvent        │  │ - CreateParticipant  │           │  │
│  │  │ - SearchEvent        │  │ - GetParticipant     │           │  │
│  │  │ - RegisterUser       │  │ - UpdateParticipant  │           │  │
│  │  │ - UnregisterUser     │  │ - GetOrganizers      │           │  │
│  │  └──────────────────────┘  └──────────────────────┘           │  │
│  └────────────────────────────────────────────────────────────────┘  │
│  ┌────────────────────────────────────────────────────────────────┐  │
│  │  Data Access Layer (Repository Pattern)                        │  │
│  │  ┌────────────────┐  ┌──────────────────┐  ┌──────────────┐  │  │
│  │  │ EventRepository│  │ParticipantRepo   │  │RegistrationR│  │  │
│  │  │ Methods:       │  │ Methods:         │  │ Methods:     │  │  │
│  │  │ - GetAll       │  │ - GetAll         │  │ - GetAll     │  │  │
│  │  │ - SearchByTitle│  │ - GetByEmail     │  │ - GetByEvent │  │  │
│  │  │ - SearchByCat  │  │ - GetOrganizers  │  │ - UpdateStat │  │  │
│  │  │ - SearchByDate │  │ - UpdateEvents   │  │              │  │  │
│  │  └────────────────┘  └──────────────────┘  └──────────────┘  │  │
│  └────────────────────────────────────────────────────────────────┘  │
│                     Port: 5000                                        │
└──────────────────────────────────────────────────────────────────────┘
                               │
                               │ MongoDB.Driver
                               │ BSON Serialization
                               │ Async Operations
                               ▼
┌──────────────────────────────────────────────────────────────────────┐
│           MongoDB Atlas Cloud Database (mongodb+srv)                 │
│  ┌────────────────────────────────────────────────────────────────┐  │
│  │  Database: newone                                              │  │
│  │  ┌──────────────────┐  ┌──────────────────┐  ┌──────────────┐ │  │
│  │  │ events           │  │participants      │  │registrations │ │  │
│  │  │ Collection       │  │ Collection       │  │ Collection   │ │  │
│  │  │                  │  │                  │  │              │ │  │
│  │  │ Documents:       │  │ Documents:       │  │ Documents:   │ │  │
│  │  │ - _id            │  │ - _id            │  │ - _id        │ │  │
│  │  │ - title          │  │ - fullName       │  │ - eventId    │ │  │
│  │  │ - description    │  │ - email          │  │ - participId │ │  │
│  │  │ - organizerId    │  │ - role           │  │ - status     │ │  │
│  │  │ - startDate      │  │ - registeredEvt  │  │              │ │  │
│  │  │ - endDate        │  │ - organizedEvt   │  │              │ │  │
│  │  │ - maxParticipant │  │                  │  │              │ │  │
│  │  │ - registeredCount│  │                  │  │              │ │  │
│  │  │ - participantIds │  │                  │  │              │ │  │
│  │  └──────────────────┘  └──────────────────┘  └──────────────┘ │  │
│  └────────────────────────────────────────────────────────────────┘  │
└──────────────────────────────────────────────────────────────────────┘
```

---

## Data Flow Diagrams

### 1. Event Creation Flow

```
User (Organizer)
      │
      │ Opens Dashboard
      ▼
┌─────────────────┐
│ OrganizerDash   │
│ "Create Event"  │
└────────┬────────┘
         │ Opens Modal
         ▼
┌──────────────────────────┐
│ Event Creation Form      │
│ (title, description...)  │
└────────┬─────────────────┘
         │ Submit
         ▼
┌──────────────────────────────────────┐
│ EventService.createEvent()           │
│ - Validate input                     │
│ - Apply business logic               │
│ - Auto-generate ID                   │
└────────┬─────────────────────────────┘
         │
         ▼
┌──────────────────────────────────────┐
│ EventRepository.Insert()             │
│ - Prepare BSON document              │
│ - Connect to MongoDB                 │
│ - Insert into events collection      │
└────────┬─────────────────────────────┘
         │
         ▼
┌──────────────────────────────────────┐
│ MongoDB Atlas                        │
│ Events Collection                    │
│ → New event document stored          │
└────────┬─────────────────────────────┘
         │
         ▼
┌──────────────────────────────────────┐
│ Return EventDto                      │
│ (200 OK)                             │
└────────┬─────────────────────────────┘
         │
         ▼
┌──────────────────────────────────────┐
│ Frontend State Update                │
│ - Add to events list                 │
│ - Show success toast                 │
│ - Close modal                        │
└──────────────────────────────────────┘
```

### 2. Event Search Flow

```
User
│ Enters search criteria
│ (title, category, date)
▼
┌─────────────────────────────┐
│ SearchPage.tsx              │
│ - handleSearch()            │
│ - Validate inputs           │
└──────────┬──────────────────┘
           │ Call service method
           ▼
┌────────────────────────────────────────────┐
│ EventApiService.searchEventsByTitle()      │
│ EventApiService.searchEventsByCategory()   │
│ EventApiService.searchEventsByDateRange()  │
│ - Construct request                        │
│ - Send HTTP GET request                    │
│ - With query parameters                    │
└──────────┬───────────────────────────────┘
           │ Axios HTTP call
           ▼
┌────────────────────────────────────────────┐
│ EventsController                           │
│ [HttpGet("search/title/{title}")]          │
│ [HttpGet("search/category/{category}")]    │
│ [HttpGet("search/daterange")]              │
│ - Parse parameters                         │
│ - Validate input                           │
└──────────┬───────────────────────────────┘
           │ Call service
           ▼
┌────────────────────────────────────────────┐
│ EventService.SearchByTitle()               │
│ EventService.SearchByCategory()            │
│ EventService.SearchByDateRange()           │
│ - Apply business logic                     │
└──────────┬───────────────────────────────┘
           │ Call repository
           ▼
┌────────────────────────────────────────────┐
│ EventRepository                            │
│ - Build MongoDB query                      │
│ - Use regex for title (partial matching)   │
│ - Use equality for category                │
│ - Use date comparison for range            │
└──────────┬───────────────────────────────┘
           │ Query MongoDB
           ▼
┌────────────────────────────────────────────┐
│ MongoDB Atlas                              │
│ events collection                          │
│ - Find matching documents                  │
│ - Return results                           │
└──────────┬───────────────────────────────┘
           │ Return list
           ▼
┌────────────────────────────────────────────┐
│ EventService                               │
│ - Map to DTOs                              │
│ - Return collection                        │
└──────────┬───────────────────────────────┘
           │ Return 200 OK
           ▼
┌────────────────────────────────────────────┐
│ Frontend                                   │
│ EventApiService                            │
│ - Parse response                           │
│ - Type cast as EventDto[]                  │
└──────────┬───────────────────────────────┘
           │ Set state
           ▼
┌────────────────────────────────────────────┐
│ SearchPage                                 │
│ - setResults(data)                         │
│ - Render grid of EventCards                │
│ - Display result count                     │
└────────────────────────────────────────────┘
```

### 3. User Registration to Event Flow

```
Logged-in User
│ Views event
│ Clicks "Register"
▼
┌────────────────────────┐
│ EventDetailsPage.tsx   │
│ handleRegister()       │
│ - Confirm action       │
│ - Get participantId    │
└──────────┬─────────────┘
           │ Call API
           ▼
┌───────────────────────────────────────────────┐
│ EventApiService.registerForEvent()            │
│ - POST /api/events/{eventId}/register/{pid}   │
│ - Send HTTP request                           │
└──────────┬────────────────────────────────────┘
           │ Axios HTTP call
           ▼
┌───────────────────────────────────────────────┐
│ EventsController                              │
│ [HttpPost("{eventId}/register/{participantId}")]
│ - Extract parameters                          │
│ - Validate                                    │
└──────────┬────────────────────────────────────┘
           │ Call service
           ▼
┌───────────────────────────────────────────────┐
│ EventService.RegisterUserForEvent()           │
│ - Check if user already registered            │
│ - Check if event has capacity                 │
│ - Create registration record                  │
│ - Update participant info                     │
│ - Increment registered count                  │
└──────────┬────────────────────────────────────┘
           │ Dual operation
           ├──────────┬──────────┤
           ▼          ▼          ▼
    ┌──────────────┐ ┌──────────────┐ ┌──────────────┐
    │ EventRepo    │ │ParticipantR  │ │RegistrationR│
    │ Update event │ │ Update user  │ │ Create link  │
    │ Add pid      │ │ Add eventId   │ │              │
    │ Inc count    │ │ to reg list   │ │              │
    └──────┬───────┘ └──────┬───────┘ └──────┬───────┘
           │                │                │
           └────────┬───────┴────────┬───────┘
                    ▼                ▼
           MongoDB Atlas
           - Update event
           - Update participant
           - Create registration
           
           ▼ (All succeed)
    
    Transaction committed
    ▼
┌───────────────────────────────────────────────┐
│ Return 200 OK                                 │
│ Updated EventDto                              │
└──────────┬────────────────────────────────────┘
           │ Return response
           ▼
┌───────────────────────────────────────────────┐
│ Frontend                                      │
│ EventDetailsPage                              │
│ - Update state with new event                 │
│ - Show success message                        │
│ - Update participant counter                  │
│ - Disable register button (if full)           │
└───────────────────────────────────────────────┘
```

---

## Deployment Architecture

### Development Environment

```
Developer Machine
├── VS Code
│   ├── C# Extension (Backend development)
│   ├── TypeScript Extension (Frontend development)
│   └── REST Client Extension (API testing)
├── Terminal
│   ├── Backend: dotnet run (port 5000)
│   └── Frontend: npm start (port 3000)
└── Browser
    └── React Dev Tools
        └── http://localhost:3000
```

### Docker Container Deployment

```
┌─────────────────────────────────────────┐
│     Docker Host / Docker Desktop        │
├─────────────────────────────────────────┤
│                                         │
│  ┌──────────────────────────────────┐  │
│  │   Container: kmc-api             │  │
│  │   Image: aspnetcore:8.0          │  │
│  │   Port: 5000 → Host:5000         │  │
│  │   Env: MONGO_URI                 │  │
│  └──────────────────────────────────┘  │
│                                         │
│  ┌──────────────────────────────────┐  │
│  │   Container: kmc-frontend        │  │
│  │   Image: node:18                 │  │
│  │   Port: 3000 → Host:3000         │  │
│  │   Cmd: npm start                 │  │
│  └──────────────────────────────────┘  │
│                                         │
│  ┌──────────────────────────────────┐  │
│  │   Volume: app-data               │  │
│  │   Used by: API container         │  │
│  └──────────────────────────────────┘  │
│                                         │
└─────────────────────────────────────────┘
         │         │          │
         │         │          └─ MongoDB Atlas
         │         │             (Cloud, no container)
         │         └─ Persistent data storage
         └─ Docker Network Bridge
```

### Production Deployment (Cloud)

```
┌────────────────────────────────────────────────────┐
│        Cloud Platform (Azure/AWS/GCP)              │
├────────────────────────────────────────────────────┤
│                                                    │
│  ┌──────────────────────────────────────────────┐ │
│  │    Web Server / CDN                          │ │
│  │  - Serve React static files (build/)         │ │
│  │  - SSL/TLS termination                       │ │
│  │  - Gzip compression                          │ │
│  └──────────────────────────────────────────────┘ │
│         ▲                                          │
│         │ HTTP                                     │
│         ▼                                          │
│  ┌──────────────────────────────────────────────┐ │
│  │    API Servers (Load Balanced)               │ │
│  │  ┌────────────┐  ┌────────────┐             │ │
│  │  │ ASP.NET #1 │  │ ASP.NET #2 │             │ │
│  │  │ (Port 5000)│  │ (Port 5000)│             │ │
│  │  └────────────┘  └────────────┘             │ │
│  │  - Horizontal scaling                        │ │
│  │  - Health checks                             │ │
│  │  - Auto-failover                             │ │
│  └──────────────────────────────────────────────┘ │
│         │                                          │
│         │ MongoDB.Driver                           │
│         ▼                                          │
│  ┌──────────────────────────────────────────────┐ │
│  │    MongoDB Atlas Cluster                     │ │
│  │  - Replica set (3+ nodes)                    │ │
│  │  - Automatic failover                        │ │
│  │  - Backup & restore                          │ │
│  │  - Encryption at rest                        │ │
│  └──────────────────────────────────────────────┘ │
│                                                    │
└────────────────────────────────────────────────────┘
```

---

## Scaling Strategy

### Horizontal Scaling (Load Balancing)

```
User Requests
│
└─→ Load Balancer (Round Robin / Least Connections)
    │
    ├─→ API Instance 1 (Port 5000)
    │   └─→ Local Connection Pool (10 connections)
    │
    ├─→ API Instance 2 (Port 5000)
    │   └─→ Local Connection Pool (10 connections)
    │
    └─→ API Instance 3 (Port 5000)
        └─→ Local Connection Pool (10 connections)
        
    All communicate with same MongoDB Atlas cluster
```

### Database Scaling (MongoDB)

```
Primary Node (Reads/Writes)
├─ Replication to Secondary 1
├─ Replication to Secondary 2
└─ Replication to Secondary 3 (Arbiter)

Sharding (if needed later)
├─ Shard 1 (Events A-M)
├─ Shard 2 (Events N-Z)
└─ Shard 3 (Participants)
```

---

## Performance Optimization

### Frontend Optimization
```
┌─ Code Splitting
│  └─ Lazy load pages with React.lazy()
├─ Image Optimization
│  └─ Use modern formats (WEBP)
├─ Caching Strategy
│  └─ HTTP caching headers
│  └─ Service Workers (PWA)
└─ Bundle Size
   └─ Tree shaking
   └─ Remove unused Bootstrap components
```

### Backend Optimization
```
┌─ Connection Pooling
│  └─ MongoDB Driver pool settings
├─ Query Optimization
│  └─ Indexes on frequently searched fields
│  └─ Projection to fetch only needed fields
├─ Async Operations
│  └─ Non-blocking I/O throughout
└─ Caching (Optional)
   └─ In-memory cache for frequently accessed data
```

---

## Monitoring & Logging

### Backend Logging

```
Program.cs Configuration:
├─ Structured logging (Serilog optional)
├─ Log levels: Debug, Information, Warning, Error
├─ Log outputs:
│  ├─ Console (development)
│  ├─ File (production)
│  └─ Cloud logging service
└─ Metrics to track:
   ├─ API response times
   ├─ Error rates
   ├─ Database query times
   └─ User activity
```

### Frontend Monitoring

```
Error Tracking:
├─ Try-catch blocks around API calls
├─ Error boundaries (React)
├─ Network error handling
└─ User input validation errors

Analytics:
├─ Page views
├─ User actions
├─ Search queries
└─ Registration completion rates
```

### Database Monitoring

```
MongoDB Atlas Dashboard:
├─ Connection metrics
├─ Query performance
├─ Storage usage
├─ Replication lag
└─ Backup status
```

---

## Disaster Recovery

### Backup Strategy

```
MongoDB Atlas:
├─ Automated daily backups (30-day retention)
├─ Point-in-time recovery (24-hour window)
├─ Replica set redundancy
└─ Multi-region replication (optional)

Source Code:
├─ Git repository
├─ GitHub / GitLab for remote backup
└─ Frequent commits with descriptive messages
```

### Recovery Procedures

```
Database Failure:
├─ MongoDB Atlas handles automatic failover
├─ Replica sets ensure high availability
└─ Manual restore from backup if needed

API Server Failure:
├─ Load balancer routes to healthy instances
├─ Auto-scaling restarts failed instances
└─ Health checks ensure availability

Frontend Failure:
├─ CDN serves cached static files
├─ User browser cache provides fallback
└─ Deployment of new build restarts service
```

---

## Security Considerations

### Network Security
```
├─ HTTPS/TLS for all communications
├─ MongoDB uses HTTPS connections
├─ CORS configured to allow only frontend domain
└─ API authentication via tokens (future enhancement)
```

### Data Security
```
├─ MongoDB Atlas encryption at rest
├─ Encryption in transit (HTTPS)
├─ Input validation on all endpoints
├─ SQL injection prevention (N/A - NoSQL)
└─ XSS prevention through React auto-escaping
```

### Access Control
```
├─ Role-based access control (RBAC)
│  ├─ Regular User
│  ├─ Organizer
│  └─ Admin
├─ Password hashing (future implementation)
├─ Session management via JWT (future)
└─ Rate limiting on API endpoints (future)
```

---

## Technology Decision Rationale

| Component | Choice | Reason |
|-----------|--------|--------|
| Language (Backend) | C# | Strong type system, mature framework, excellent for SOA |
| Framework (Backend) | ASP.NET Core | Modern, cross-platform, built-in DI, excellent performance |
| Database | MongoDB | Flexible schema, excellent for event/participant model, cloud-native |
| Frontend | React | Component reuse, large ecosystem, excellent for interactive UIs |
| Language (Frontend) | TypeScript | Type safety, better IDE support, catches errors at compile time |
| Styling | Bootstrap | Pre-built components, responsive, professional appearance |
| HTTP Client | Axios | Simple API, error handling, request/response interceptors |
| Containerization | Docker | Reproducible environments, easy deployment, industry standard |

---

## Future Enhancement Paths

### Phase 2: Advanced Features
```
├─ Email notifications for event updates
├─ Payment integration (Stripe/PayPal)
├─ Event reviews and ratings
├─ User following/feed system
└─ Event attendance QR codes
```

### Phase 3: Analytics & Insights
```
├─ Dashboard for admins
├─ Event popularity metrics
├─ User engagement analytics
├─ Revenue tracking
└─ Custom reports
```

### Phase 4: Mobile & PWA
```
├─ React Native mobile app
├─ Progressive Web App (PWA)
├─ Offline support
├─ Push notifications
└─ Mobile-optimized UI
```

### Phase 5: Marketing & Social
```
├─ Social media integration
├─ Social sharing features
├─ Email marketing campaigns
├─ SEO optimization
└─ Analytics dashboard
```

---

## Maintenance & Support

### Regular Maintenance Tasks
```
Daily:
├─ Monitor API logs for errors
├─ Check database performance
└─ Verify all services are running

Weekly:
├─ Review application metrics
├─ Check for security updates
└─ Backup verification

Monthly:
├─ Performance analysis
├─ Update dependencies (if safe)
└─ User feedback review
```

### Knowledge Transfer
```
Documentation:
├─ README files (setup & overview)
├─ DEVELOPMENT.md (architecture & patterns)
├─ Code comments (business logic)
└─ API documentation (Swagger)

Training:
├─ Pair programming sessions
├─ Architecture walkthrough
├─ Code review process
└─ Deployment procedures
```

---

**Architecture Last Updated**: 2025  
**Version**: 1.0  
**Status**: Production Ready
