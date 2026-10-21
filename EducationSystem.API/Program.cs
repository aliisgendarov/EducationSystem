using Education.Application.DTOs.Group;
using Education.Application.DTOs.Manager;
using Education.Application.DTOs.Student;
using Education.Application.DTOs.TeacherDtos;
using Education.Application.Interfaces;
using Education.Application.Interfaces.Services;
using Education.Application.Services;
using Education.Application.Validators;
using EducationSystem.Infrastructure.Data;
using EducationSystem.Infrastructure.UnitOfWork;
using FluentValidation;

namespace EducationSystem.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            var connectionString = builder.Configuration.GetConnectionString("EducationSystem")
                ?? throw new InvalidOperationException(
                    "Connection string 'EducationSystem' was not configured.");

            builder.Services.AddSingleton<IDbConnectionFactory>(
                _ => new DbConnectionFactory(connectionString));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddScoped<IGroupService, GroupService>();
            builder.Services.AddScoped<IManagerService, ManagerService>();
            builder.Services.AddScoped<ITeacherService, TeacherService>();
            builder.Services.AddScoped<IValidator<CreateStudentDto>, CreateStudentDtoValidator>();
            builder.Services.AddScoped<IValidator<UpdateStudentDto>, UpdateStudentDtoValidator>();
            builder.Services.AddScoped<IValidator<CreateGroupDto>, CreateGroupDtoValidator>();
            builder.Services.AddScoped<IValidator<UpdateGroupDto>, UpdateGroupDtoValidator>();
            builder.Services.AddScoped<IValidator<CreateManagerDto>, CreateManagerDtoValidator>();
            builder.Services.AddScoped<IValidator<UpdateManagerDto>, UpdateManagerDtoValidator>();
            builder.Services.AddScoped<IValidator<CreateTeacherDto>, CreateTeacherDtoValidator>();
            builder.Services.AddScoped<IValidator<UpdateTeacherDto>, UpdateTeacherDtoValidator>();

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
