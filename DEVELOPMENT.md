# Development Guide - KMC Event Platform

## Project Overview

The KMC Event Platform is a **Service-Oriented Architecture (SOA)** solution developed for the Kandy Municipal Council to modernize event management and promotion.

### Technology Stack
- **Runtime:** .NET 8.0
- **API Framework:** ASP.NET Core
- **Database:** MongoDB
- **API Documentation:** Swagger/OpenAPI
- **Mapping:** AutoMapper
- **Architecture Pattern:** Clean Architecture with SOA

## Project Structure

```
src/
├── KMCEventPlatform.Models/          # Domain entities and models
│   ├── Event.cs
│   ├── Participant.cs
│   └── Registration.cs
│
├── KMCEventPlatform.Data/            # Data access layer
│   ├── Configuration/
│   │   └── MongoDbSettings.cs
│   ├── Context/
│   │   └── MongoDbContext.cs
│   └── Repositories/
│       ├── IRepository.cs
│       ├── EventRepository.cs
│       ├── ParticipantRepository.cs
│       └── RegistrationRepository.cs
│
├── KMCEventPlatform.Services/        # Business logic layer
│   ├── DTOs/
│   │   └── ServiceDtos.cs
│   ├── Mappings/
│   │   └── MappingProfile.cs
│   └── Services/
│       ├── EventService.cs
│       └── ParticipantService.cs
│
└── KMCEventPlatform.API/             # REST API layer
    ├── Controllers/
    │   ├── EventsController.cs
    │   └── ParticipantsController.cs
    ├── Program.cs
    ├── appsettings.json
    └── Properties/
        └── launchSettings.json
```

## Architecture Pattern: Clean Architecture + SOA

### Layer Responsibilities

**Presentation Layer (API Controllers)**
- Handles HTTP requests/responses
- Input validation
- Logging and error handling

**Business Logic Layer (Services)**
- Event management operations
- Participant management
- Registration handling
- Business rule validation

**Data Access Layer (Repositories)**
- MongoDB operations
- CRUD operations
- Complex queries

**Domain Layer (Models)**
- Entity definitions
- Business logic constants
- Enumerations

## Prerequisites

1. **.NET 8 SDK** - Download from https://dotnet.microsoft.com/download
2. **MongoDB** - Download from https://www.mongodb.com/try/download/community
3. **Visual Studio Code** or **Visual Studio 2022** (optional)
4. **Postman** or **Thunder Client** for API testing (optional)

## Installation & Setup

### 1. Install .NET 8 SDK

```powershell
# Check if .NET is installed
dotnet --version

# Should return 8.0.x or higher
```

### 2. Install MongoDB Locally

**Option A: Windows**
- Download MongoDB Community Server from https://www.mongodb.com/try/download/community
- Install with default settings
- MongoDB will run on `mongodb://localhost:27017`

**Option B: Docker**
```bash
docker run -d -p 27017:27017 --name mongodb mongo:latest
```

### 3. Restore Dependencies

```powershell
cd "Service-Oriented Architecture_Platform"

# Restore all projects
dotnet restore
```

### 4. Build Solution

```powershell
# Build all projects
dotnet build

# Build with release configuration
dotnet build --configuration Release
```

## Running the Application

### Start MongoDB (if not running as service)

**Windows:**
```powershell
mongod
```

**Docker:**
```bash
docker start mongodb
```

### Run the API

```powershell
cd "src/KMCEventPlatform.API"

# Run with development environment
dotnet run

# Output should show:
# - Now listening on: http://localhost:5000
# - Now listening on: https://localhost:7000
```

### Access Swagger UI

Open your browser and navigate to:
```
http://localhost:5000/swagger
```

You'll see the full API documentation with the ability to test endpoints directly.

## API Endpoints

### Events API

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/events` | Get all events |
| GET | `/api/events/{id}` | Get event by ID |
| POST | `/api/events` | Create new event |
| PUT | `/api/events/{id}` | Update event |
| DELETE | `/api/events/{id}` | Delete event |
| GET | `/api/events/search/title/{title}` | Search by title |
| GET | `/api/events/search/category/{category}` | Search by category |
| GET | `/api/events/search/daterange?startDate=&endDate=` | Search by date |
| GET | `/api/events/organizer/{organizerId}` | Get organizer's events |
| POST | `/api/events/{eventId}/register/{participantId}` | Register participant |
| DELETE | `/api/events/{eventId}/unregister/{participantId}` | Unregister participant |

### Participants API

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/participants` | Get all participants |
| GET | `/api/participants/{id}` | Get participant by ID |
| GET | `/api/participants/email/{email}` | Get by email |
| POST | `/api/participants` | Create participant |
| PUT | `/api/participants/{id}` | Update participant |
| DELETE | `/api/participants/{id}` | Delete participant |
| GET | `/api/participants/organizers/list` | Get all organizers |

## Testing the API

### Using Postman

1. **Create a Participant (Organizer)**
```
POST http://localhost:5000/api/participants
Content-Type: application/json

{
  "fullName": "John Doe",
  "email": "john@example.com",
  "phoneNumber": "+94701234567",
  "address": "123 Main St, Kandy",
  "role": 1,
  "isActive": true
}
```

2. **Create an Event**
```
POST http://localhost:5000/api/events
Content-Type: application/json

{
  "title": "City Marathon",
  "description": "Annual city marathon event",
  "organizerId": "{participantId}",
  "organizerName": "John Doe",
  "startDate": "2025-05-15T08:00:00Z",
  "endDate": "2025-05-15T12:00:00Z",
  "location": "Kandy City Center",
  "category": "Sports",
  "maxParticipants": 500,
  "contactEmail": "john@example.com",
  "contactPhone": "+94701234567",
  "tags": ["sports", "marathon", "community"]
}
```

3. **Create a Participant (Regular)**
```
POST http://localhost:5000/api/participants
Content-Type: application/json

{
  "fullName": "Jane Smith",
  "email": "jane@example.com",
  "phoneNumber": "+94702234567",
  "address": "456 Oak Ave, Kandy",
  "role": 0,
  "isActive": true
}
```

4. **Register Participant for Event**
```
POST http://localhost:5000/api/events/{eventId}/register/{participantId}
```

## Database Configuration

### MongoDB Connection Settings

Edit `appsettings.json` to configure MongoDB connection:

```json
{
  "MongoDbSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "KMCEventPlatformDb",
    "EventsCollectionName": "Events",
    "ParticipantsCollectionName": "Participants",
    "RegistrationsCollectionName": "Registrations"
  }
}
```

### Collections Structure

**Events Collection**
- Stores event information
- References to organizer and participants
- Search indexes on: Title, Category, StartDate, Status

**Participants Collection**
- User profiles
- Role-based access (Regular, Organizer, Admin)
- List of registered and organized events

**Registrations Collection**
- Event registrations
- Participant attendance tracking
- Special requirements and comments

## Service Layer Architecture

### IEventService
- `GetAllEventsAsync()` - Retrieve all events
- `GetEventByIdAsync(id)` - Get specific event
- `CreateEventAsync(dto)` - Create new event
- `UpdateEventAsync(id, dto)` - Update event
- `DeleteEventAsync(id)` - Delete event
- Search methods (by title, category, date range, organizer)
- `RegisterParticipantAsync()` - Handle registration
- `UnregisterParticipantAsync()` - Handle cancellation

### IParticipantService
- `GetAllParticipantsAsync()` - Get all users
- `GetParticipantByIdAsync(id)` - Get specific user
- `GetParticipantByEmailAsync(email)` - Find by email
- `CreateParticipantAsync(dto)` - Register new user
- `UpdateParticipantAsync(id, dto)` - Update profile
- `DeleteParticipantAsync(id)` - Delete account
- `GetOrganizersAsync()` - Get all organizers

## Logging

The application uses built-in ASP.NET Core logging. Configure in `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

## Error Handling

All endpoints return consistent HTTP status codes:
- **200 OK** - Successful GET/PUT
- **201 Created** - Successful POST
- **204 No Content** - Successful DELETE
- **400 Bad Request** - Validation error
- **404 Not Found** - Resource not found
- **500 Internal Server Error** - Server error

## Next Steps

1. **Implement Authentication** - Add JWT or OAuth2
2. **Add Unit Tests** - Create unit test projects
3. **Implement Caching** - Add Redis for performance
4. **Add API Versioning** - Handle future versions
5. **Create Client Application** - Web or mobile client
6. **Deploy to Production** - Docker/Kubernetes setup
7. **Add Logging & Monitoring** - Serilog or Application Insights

## Troubleshooting

### MongoDB Connection Failed
```
Error: Failed to connect to MongoDB
Solution: Ensure MongoDB is running on port 27017
- Windows: mongod command should be running
- Docker: docker start mongodb
```

### Port Already in Use
```
Error: Address already in use
Solution: Change port in launchSettings.json or kill process:
- netstat -ano | findstr :5000
- taskkill /PID {PID} /F
```

### Build Errors
```
Error: Project reference issues
Solution: 
- Run: dotnet clean
- Run: dotnet restore
- Run: dotnet build
```

## References

- [Microsoft Learn: ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/)
- [MongoDB Documentation](https://docs.mongodb.com/)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Service-Oriented Architecture](https://en.wikipedia.org/wiki/Service-oriented_architecture)

## Support

For issues or questions:
1. Check the Swagger documentation at `/swagger`
2. Review log output in the console
3. Check MongoDB connection settings
4. Ensure all dependencies are installed

---

**Last Updated:** April 2025
