using KMCEventPlatform.Data.Context;
using KMCEventPlatform.Data.Configuration;
using KMCEventPlatform.Data.Repositories;
using KMCEventPlatform.Services.Services;
using KMCEventPlatform.Services.Mappings;
using MongoDB.Driver;

var builder = WebApplicationBuilder.CreateBuilder(args);

// Add configuration
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection(MongoDbSettings.SectionName));

// Add MongoDB context
builder.Services.AddSingleton<MongoDbContext>();

// Add repositories
builder.Services.AddScoped<IEventRepository, EventRepository>(provider =>
{
    var context = provider.GetRequiredService<MongoDbContext>();
    return new EventRepository(context.Events);
});

builder.Services.AddScoped<IParticipantRepository, ParticipantRepository>(provider =>
{
    var context = provider.GetRequiredService<MongoDbContext>();
    return new ParticipantRepository(context.Participants);
});

builder.Services.AddScoped<IRegistrationRepository, RegistrationRepository>(provider =>
{
    var context = provider.GetRequiredService<MongoDbContext>();
    return new RegistrationRepository(context.Registrations);
});

// Add services
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IParticipantService, ParticipantService>();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add controllers
builder.Services.AddControllers();

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "KMC Event Platform API",
        Version = "v1",
        Description = "Service-Oriented Architecture for Kandy Municipal Council Event Management",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "KMC Development Team",
            Email = "support@kmc.lk"
        }
    });
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "KMC Event Platform API V1");
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();
