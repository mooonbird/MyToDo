using MyToDo.Api.Context;
using Microsoft.EntityFrameworkCore;
using Arch.EntityFrameworkCore.UnitOfWork;
using MyToDo.Api.Repository;
using MyToDo.Api.Services;
using AutoMapper;
using MyToDo.Api.Extensions;

namespace MyToDo.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            //Ìí¼ÓMyToDoContext
            builder.Services.AddDbContext<MyToDoContext>(options =>
            {
                options.UseSqlite(builder.Configuration.GetConnectionString("ToDoConnection") 
                    ?? throw new InvalidOperationException("ToDoConnection not found"));
            })
                .AddUnitOfWork<MyToDoContext>()
            .AddCustomRepository<ToDo, ToDoRepository>()
            .AddCustomRepository<Memo, MemoRepository>()
            .AddCustomRepository<User, UserRepository>();

            //Ìí¼ÓToDoService,MemoService
            builder.Services.AddTransient<IToDoService, ToDoService>();
            builder.Services.AddTransient<IMemoService, MemoService>();
            builder.Services.AddTransient<ILoginService, LoginService>();

            //Ìí¼ÓAutoMapper
            var automapperConfiguration = new MapperConfiguration(config =>
            {
                config.AddProfile(new AutoMapperConfigurationProfile());
            });
            builder.Services.AddSingleton(automapperConfiguration.CreateMapper());



            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                
                
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseAuthorization();


            app.MapControllers();
            app.MapSwagger();

            app.Run();
        }
    }
}