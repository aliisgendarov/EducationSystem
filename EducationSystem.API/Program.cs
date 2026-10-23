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
using EducationSystem.API.ExceptionHandling;

namespace EducationSystem.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("EducationSystem")
                ?? throw new InvalidOperationException(
                    "Connection string 'EducationSystem' was not configured.");

            // Infrastructure
            builder.Services.AddSingleton<IDbConnectionFactory>(
                _ => new DbConnectionFactory(connectionString));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Application services
            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddScoped<IGroupService, GroupService>();
            builder.Services.AddScoped<IManagerService, ManagerService>();
            builder.Services.AddScoped<ITeacherService, TeacherService>();

            // Validators
            builder.Services.AddScoped<IValidator<CreateStudentDto>, CreateStudentDtoValidator>();
            builder.Services.AddScoped<IValidator<UpdateStudentDto>, UpdateStudentDtoValidator>();
            builder.Services.AddScoped<IValidator<CreateGroupDto>, CreateGroupDtoValidator>();
            builder.Services.AddScoped<IValidator<UpdateGroupDto>, UpdateGroupDtoValidator>();
            builder.Services.AddScoped<IValidator<CreateManagerDto>, CreateManagerDtoValidator>();
            builder.Services.AddScoped<IValidator<UpdateManagerDto>, UpdateManagerDtoValidator>();
            builder.Services.AddScoped<IValidator<CreateTeacherDto>, CreateTeacherDtoValidator>();
            builder.Services.AddScoped<IValidator<UpdateTeacherDto>, UpdateTeacherDtoValidator>();

            // API + Swagger
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            var app = builder.Build();

            app.UseExceptionHandler();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}