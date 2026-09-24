using CRMSystem.Data;
using CRMSystem.Interceptors;
using CRMSystem.Middleware;
using CRMSystem.Models.Configuration;
using CRMSystem.Repositories.Implementation;
using CRMSystem.Repositories.Interface;
using CRMSystem.Services.Implementation;
using CRMSystem.Services.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ICurrentUserServices, CurrentUserServices>();
builder.Services.AddScoped<AuditSaveChangesInterceptor>();

builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    );

    options.AddInterceptors(
        serviceProvider.GetRequiredService<AuditSaveChangesInterceptor>());
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt"));

var jwtSettings =
    builder.Configuration
        .GetSection("Jwt")
        .Get<JwtSettings>()!;

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;

        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer =
                    jwtSettings.Issuer,

                ValidAudience =
                    jwtSettings.Audience,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            jwtSettings.Key))
            };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.TryGetValue(
                        "access_token",
                        out var token))
                {
                    context.Token = token;
                }

                return Task.CompletedTask;
            }
        };
    });


builder.Services.AddScoped<IOrganizationRepository, OrganizationRepository>();
builder.Services.AddScoped<IOrganizationServices, OrganizationServices>();
builder.Services.AddHttpClient<IAutomationServices, AutomationServices>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthServices, AuthServices>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerServices, CustomerServices>();
builder.Services.AddScoped<ILeadSourceRepository, LeadSourceRepository>();
builder.Services.AddScoped<ILeadSourceServices, LeadSourceServices>();
builder.Services.AddScoped<ILeadStatusRepository, LeadStatusRepository>();
builder.Services.AddScoped<ILeadStatusServices, LeadStatusServices>();
builder.Services.AddScoped<IPipelinesRepository, PipelinesRepository>();
builder.Services.AddScoped<IPipelinesServices, PipelinesServices>();
builder.Services.AddScoped<IPipelineStageRepository, PipelineStageRepository>();
builder.Services.AddScoped<IPipelineStageServices, PipelineStageServices>();
builder.Services.AddScoped<ILeadRepository, LeadRepository>();
builder.Services.AddScoped<ILeadServices, LeadServices>();
builder.Services.AddScoped<ICustomerCodeServices, CustomerCodeServices>();
builder.Services.AddScoped<ICustomerAddressRepository, CustomerAddressRepository>();
builder.Services.AddScoped<ICustomerAddressServices, CustomerAddressServices>();
builder.Services.AddScoped<ICustomerContactRepository, CustomerContactRepository>();
builder.Services.AddScoped<ICustomerContactServices, CustomerContactServices>();
builder.Services.AddScoped<IOpportunityRepository, OpportunityRepository>();
builder.Services.AddScoped<IOpportunityServices, OpportunityServices>();
builder.Services.AddScoped<ITaskItemRepository, TaskItemRepository>();
builder.Services.AddScoped<ITaskItemServices, TaskItemServices>();
builder.Services.AddScoped<IActivityRepository, ActivityRepository>();
builder.Services.AddScoped<IActivityServices, ActivityServices>();
builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<INoteServices, NoteServices>();
builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();
builder.Services.AddScoped<IAuditLogServices, AuditLogServices>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var roleManager =
        scope.ServiceProvider
            .GetRequiredService<RoleManager<IdentityRole<Guid>>>();

    var userManager =
        scope.ServiceProvider
            .GetRequiredService<UserManager<ApplicationUser>>();

    await RoleSeeder.SeedAsync(roleManager);
    await UserSeeder.SeedAsync(userManager);
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My CRMproject V1");
    });
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
