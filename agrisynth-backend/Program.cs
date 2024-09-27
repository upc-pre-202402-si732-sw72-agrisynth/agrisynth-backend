using agrisynth_backend.Collaboration.Application.CommandServices;
using agrisynth_backend.Collaboration.Application.QueryServices;
using agrisynth_backend.Collaboration.Domain.Repositories;
using agrisynth_backend.Collaboration.Domain.Services;
using agrisynth_backend.Collaboration.Infrastructure.Persistence.EFC.Repositories;
using agrisynth_backend.Documents.Application.CommandServices;
using agrisynth_backend.Documents.Application.QueryServices;
using agrisynth_backend.Documents.Domain.Repositories;
using agrisynth_backend.Documents.Domain.Services;
using agrisynth_backend.Documents.Infrastructure.Persistence.EFC.Repositories;
using agrisynth_backend.IAM.Application.Internal.CommandServices;
using agrisynth_backend.IAM.Application.Internal.OutboundServices;
using agrisynth_backend.IAM.Application.Internal.QueryServices;
using agrisynth_backend.IAM.Domain.Repositories;
using agrisynth_backend.IAM.Domain.Services;
using agrisynth_backend.IAM.Infrastructure.Hashing.BCrypt.Services;
using agrisynth_backend.IAM.Infrastructure.Persistence.EFC.Repositories;
using agrisynth_backend.IAM.Infrastructure.Pipeline.Middleware.Extensions;
using agrisynth_backend.IAM.Infrastructure.Tokens.JWT.Configuration;
using agrisynth_backend.IAM.Infrastructure.Tokens.JWT.Services;
using agrisynth_backend.IAM.Interfaces.ACL;
using agrisynth_backend.IAM.Interfaces.ACL.Services;
using agrisynth_backend.Landrental.Application.CommandServices;
using agrisynth_backend.Landrental.Application.QueryServices;
using agrisynth_backend.Landrental.Domain.Repositories;
using agrisynth_backend.Landrental.Domain.Services;
using agrisynth_backend.Landrental.Infrastructure.Persistence.EFC.Repositories;
using agrisynth_backend.Machineryrental.Application.CommandServices;
using agrisynth_backend.Machineryrental.Application.QueryServices;
using agrisynth_backend.Machineryrental.Domain.Repositories;
using agrisynth_backend.Machineryrental.Domain.Services;
using agrisynth_backend.Machineryrental.Infrastructure.Persistence.EFC.Repositories;
using agrisynth_backend.Profiles.Application.Internal.CommandServices;
using agrisynth_backend.Profiles.Application.Internal.QueryServices;
using agrisynth_backend.Profiles.Domain.Repositories;
using agrisynth_backend.Profiles.Domain.Services;
using agrisynth_backend.Profiles.Infrastructure.Persistence.EFC.Repositories;
using agrisynth_backend.Profiles.Interfaces.ACL;
using agrisynth_backend.Profiles.Interfaces.ACL.Services;
using agrisynth_backend.Resource.Application.CommandServices;
using agrisynth_backend.Resource.Application.QueryServices;
using agrisynth_backend.Resource.Domain.Repositories;
using agrisynth_backend.Resource.Domain.Services;
using agrisynth_backend.Resource.Infrastructure.Persistence.EFC.Repositories;
using agrisynth_backend.Shared.Domain.Repositories;
using agrisynth_backend.Shared.Infrastructure.Persistence.EFC.Configuration;
using agrisynth_backend.Shared.Infrastructure.Persistence.EFC.Repositories;
using agrisynth_backend.Shared.Interfaces.ASP.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options => options.Conventions.Add(new KebabCaseRouteNamingConvention()));

// Add Database Connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(
    options =>
    {
        if (connectionString != null)
            if (builder.Environment.IsDevelopment())
                options.UseMySQL(connectionString)
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            else if (builder.Environment.IsProduction())
                options.UseMySQL(connectionString)
                    .LogTo(Console.WriteLine, LogLevel.Error)
                    .EnableDetailedErrors();
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    c =>
    {
        c.SwaggerDoc("v1",
            new OpenApiInfo
            {
                Title = "Agrisynth.API",
                Version = "v1",
                Description = "Agrisynth API",
                TermsOfService = new Uri("https://agrisynth.com/tos"),
                Contact = new OpenApiContact
                {
                    Name = "Agrisynth",
                    Email = "contact@agrisynths.com"
                },
                License = new OpenApiLicense
                {
                    Name = "Apache 2.0",
                    Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0.html")
                }
            });
        c.EnableAnnotations();
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer"
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                },
                Array.Empty<string>()
            }
        });
    });

// Configure Lowercase URLs
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Add CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowedAllPolicy",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Configure Dependency Injections

// Shared Bounded Context Dependency Injections
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// News Bounded Context Dependency Injections
builder.Services.AddScoped<ITerrainRepository, TerrainRepository>();
builder.Services.AddScoped<ITerrainCommandService, TerrainCommandService>();
builder.Services.AddScoped<ITerrainQueryService, TerrainQueryService>();

// Machinerys Bounded Context Dependency Injections
builder.Services.AddScoped<IMachineryRepository, MachineryRepository>();
builder.Services.AddScoped<IMachineryCommandService, MachineryCommandService>();
builder.Services.AddScoped<IMachineryQueryService, MachineryQueryService>();

// Documents Bounded Context Dependency Injections
builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
builder.Services.AddScoped<IDocumentCommandService, DocumentCommandService>();
builder.Services.AddScoped<IDocumentQueryService, DocumentQueryService>();

// Collaboration Bounded Context Dependency Injections
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<IWorkerRepository, WorkerRepository>();
builder.Services.AddScoped<ITeamWorkerRepository, TeamWorkerRepository>();

builder.Services.AddScoped<ITeamCommandService, TeamCommandService>();
builder.Services.AddScoped<IWorkerCommandService, WorkerCommandService>();
builder.Services.AddScoped<ITeamWorkerCommandService, TeamWorkerCommandService>();

builder.Services.AddScoped<ITeamQueryService, TeamQueryService>();
builder.Services.AddScoped<IWorkerQueryService, WorkerQueryService>();
builder.Services.AddScoped<ITeamWorkerQueryService, TeamWorkerQueryService>();

// Resources Bounded Context Dependency Injections
builder.Services.AddScoped<IResourceItemRepository, ResourceItemRepository>();
builder.Services.AddScoped<IResourceItemCommandService, ResourceItemCommandService>();
builder.Services.AddScoped<IResourceItemQueryService, ResourceItemQueryService>();

// Profiles Bounded Context Injection Configuration
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IProfileCommandService, ProfileCommandService>();
builder.Services.AddScoped<IProfileQueryService, ProfileQueryService>();
builder.Services.AddScoped<IProfilesContextFacade, ProfilesContextFacade>();

// IAM Bounded Context Injection Configuration
builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IHashingService, HashingService>();
builder.Services.AddScoped<IIamContextFacade, IamContextFacade>();

var app = builder.Build();

// Verify Database Objects are created
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    DotNetEnv.Env.Load("./env.development");
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI();

// Apply CORS Policy
app.UseCors("AllowedAllPolicy");

// Add Authorization Middleware to the Request Pipeline
app.UseRequestAuthorization();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
