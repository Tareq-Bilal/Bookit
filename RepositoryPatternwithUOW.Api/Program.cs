
using Microsoft.EntityFrameworkCore;
using RepositoryPatternWithUOW.Core;
using RepositoryPatternWithUOW.Core.Repositories;
using RepositoryPatternWithUOW.EF;
using RepositoryPatternWithUOW.EF.Repositories;
using RepositoryPatternWithUOW.EF.Mappings;
using RepositoryPatternWithUOW.EF.Mappings.Books;
using System.Reflection;
using FluentValidation.AspNetCore;
using RepositoryPatternwithUOW.Api.Validators.Book;
using RepositoryPatternwithUOW.Api.Mappings.Authors;
using RepositoryPatternwithUOW.Api.Validators.Auhtor;
using RepositoryPatternwithUOW.Api.Mappings;
using RepositoryPatternwithUOW.Api.Validators.Category;
using RepositoryPatternwithUOW.Api.DTO_s.User;
using RepositoryPatternwithUOW.Api.Validators.User;
using RepositoryPatternwithUOW.Api.DTO_s.Publisher;
using RepositoryPatternwithUOW.Api.Validators.Publisher;
using RepositoryPatternwithUOW.Api.Validators.Loan;

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
            builder.Services.AddSwaggerGen();

            //builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
            builder.Services.AddAutoMapper(typeof(BooksProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(AuthorProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(CategoryProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(UserProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(PublisherProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(LoanProfile).Assembly);

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
            });

            //builder.Services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

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
