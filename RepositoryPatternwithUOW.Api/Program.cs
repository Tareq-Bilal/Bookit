
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RepositoryPatternwithUOW.Api.DTO_s.Publisher;
using RepositoryPatternwithUOW.Api.DTO_s.User;
using RepositoryPatternwithUOW.Api.Mappings;
using RepositoryPatternwithUOW.Api.Mappings.Authors;
using RepositoryPatternwithUOW.Api.Validators.Auhtor;
using RepositoryPatternwithUOW.Api.Validators.Book;
using RepositoryPatternwithUOW.Api.Validators.Category;
using RepositoryPatternwithUOW.Api.Validators.Loan;
using RepositoryPatternwithUOW.Api.Validators.Login;
using RepositoryPatternwithUOW.Api.Validators.Publisher;
using RepositoryPatternwithUOW.Api.Validators.Register;
using RepositoryPatternwithUOW.Api.Validators.User;
using RepositoryPatternWithUOW.Core;
using RepositoryPatternWithUOW.Core.Helpers;
using RepositoryPatternWithUOW.Core.Models;
using RepositoryPatternWithUOW.Core.Repositories;
using RepositoryPatternWithUOW.EF;
using RepositoryPatternWithUOW.EF.Mappings;
using RepositoryPatternWithUOW.EF.Mappings.Books;
using RepositoryPatternWithUOW.EF.Repositories;
using System.Reflection;
using System.Text;

namespace RepositoryPatternwithUOW.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnections")));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                // Add JWT Authentication to Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
            });

            //builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
            builder.Services.AddAutoMapper(typeof(BooksProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(AuthorProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(CategoryProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(UserProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(PublisherProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(LoanProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(TransactionProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(WishlistProfile).Assembly);

            builder.Services.AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<BookAddDTOValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<AuthorAddDTOValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<CategoryAddDTOValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<CategoryUpdateDTOValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<UserAddDTOValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<UserUpdateDTOValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<PublisherAddDTOValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<PublisherUpdateDTOValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<LoanAddValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<LoanUpdateValidator>();
                builder.Services.AddValidatorsFromAssemblyContaining<RegisterDTOValidator>();
                builder.Services.AddValidatorsFromAssemblyContaining<LoginDTOValidator>();


            });

            //builder.Services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

            var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtService>();
            builder.Services.AddSingleton(jwtOptions);
            builder.Services.AddAuthentication()
                   .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                   {
                       options.SaveToken = true; //if the token valid will save it in Authentication Properties
                       options.TokenValidationParameters = new TokenValidationParameters
                       {
                           ValidateIssuer = true,
                           ValidIssuer = jwtOptions.ValidIssuer,
                           ValidateAudience = true,
                           ValidAudience = jwtOptions.ValidAudience,
                           ValidateIssuerSigningKey = true,
                           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key))

                       };
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
        }
    }
}
