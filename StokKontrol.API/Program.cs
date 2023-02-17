using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StokKontrol.Repository.Abstarct;
using StokKontrol.Repository.Concrete;
using StokKontrol.Repository.Context;
using StokKontrol.Service.Abstract;
using StokKontrol.Service.Concrete;


namespace StokKontrol.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddNewtonsoftJson(option =>
            option.SerializerSettings.ReferenceLoopHandling=ReferenceLoopHandling.Ignore);
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StokKontrolContext>(option =>
            {
                option.UseSqlServer("Server=DESKTOP-B0BPUKI;database=NRM1StokDB;Trusted_Connection=True;");
            });

            builder.Services.AddTransient(typeof(IGenericService<>), typeof(GenericManager<>));
            builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));


            //alttaki kýsýmlar hangi uygulamayý kullanacaðýmýzý belirtir.

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