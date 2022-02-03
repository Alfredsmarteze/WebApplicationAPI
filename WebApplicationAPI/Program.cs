using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WebApplication.Context;
using WebApplication.Extensions;
var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Connection");
// Add services to the container.
builder.Services.AddJWTTokenServices(builder.Configuration);

builder.Services.AddDbContext<ModelContext>(x => x.UseSqlServer(connectionString));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                    Reference = new OpenApiReference {
                        Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                           
                    }
            },
            new string[] {}
        }
    });
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title ="Web API",
        Version = "v1",
        Description ="Token implementation",
        Contact=new OpenApiContact { 
        Name ="Simplex Business Solutions",
        Email ="xyz@simplexsystem.com",
        Url =new Uri("https://simplexsystem.com")
        },
    });
    
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
