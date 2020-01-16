using System;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NaucnaCentralaBackend.DataContextNamespace;
using NaucnaCentralaBackend.Models.Database;
using NaucnaCentralaBackend.Models.Enums;

namespace NaucnaCentralaBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<DataContext>();

                SeedData(services);
            }

            host.Run();
        }

        private static void SeedData(IServiceProvider services)
        {
            using (var dataContext = new DataContext(
                services.GetRequiredService<DbContextOptions<DataContext>>()))
            {

                SeedUsers(dataContext);
            }
        }

        private static void SeedUsers(DataContext dataContext)
        {
            if (dataContext.Users.Any())
            {
                return;
            }

            dataContext.Users.Add(
               new User
               {
                   Role = UserRoles.Administrator.ToString(),
                   Email = "administrator@hotmail.com",
                   EmailConfirmed = true,
                   Lastname = "lastname",
                   City = "city",
                   Country = "country",
                   Password = "administrator",
                   IsActive = true,
                   Username = "administrator",
                   Firstname = "firstname",
                   AdminConfirmed = true
               });

            dataContext.Users.Add(
                new User
                {
                    Role = UserRoles.Editor.ToString(),
                    Password = "editor",
                    EmailConfirmed = true,
                    ScientificAreas = "Anatomy, Chemistry",
                    IsActive = true,
                    Lastname = "Catch",
                    City = "Paris",
                    Country = "France",
                    Username = "editor",
                    AdminConfirmed = true,
                    Email = "editor@hotmail.com",
                    Firstname = "John"
                });

            dataContext.Users.Add(
                new User
                {
                    ScientificAreas = "Earth Science, Engineering",
                    Password = "editor1",
                    Email = "editor1@hotmail.com",
                    Lastname = "Vick",
                    IsActive = true,
                    Username = "Michael Vick",
                    AdminConfirmed = true,
                    Role = UserRoles.Editor.ToString(),
                    City = "New York",
                    Firstname = "Michael",
                    Country = "United States",
                    EmailConfirmed = true
                });

            dataContext.Users.Add(
               new User
               {
                   IsActive = true,
                   Role = UserRoles.Editor.ToString(),
                   Firstname = "Tom",
                   EmailConfirmed = true,
                   Country = "United States",
                   Lastname = "Brady",
                   AdminConfirmed = true,
                   ScientificAreas = "Anatomy, Chemistry, Earth Science, Engineering",
                   City = "Boston",
                   Username = "Tom Brady",
                   Email = "editor2@hotmail.com",
                   Password = "editor2"
               });

            dataContext.Users.Add(
                new User
                {
                    Password = "reviewer1",
                    Role = UserRoles.Reviewer.ToString(),
                    ScientificAreas = "Anatomy, Chemistry",
                    EmailConfirmed = true,
                    City = "Boston",
                    AdminConfirmed = true,
                    Country = "United States",
                    Email = "reviewer1@hotmail.com",
                    Firstname = "Julian",
                    IsActive = true,
                    Username = "Julian Edelman",
                    Lastname = "Edelman"
                });

            dataContext.Users.Add(
                new User
                {
                    Role = UserRoles.Reviewer.ToString(),
                    Country = "Serbia",
                    EmailConfirmed = true,
                    Password = "reviewer2",
                    AdminConfirmed = true,
                    Firstname = "Nemanja",
                    City = "Uzice",
                    Email = "reviewer2@hotmail.com",
                    ScientificAreas = "Anatomy, Chemistry",
                    IsActive = true,
                    Lastname = "Vidic",
                    Username = "Nemanja Vidic"
                });

            dataContext.Users.Add(
                new User
                {
                    AdminConfirmed = true,
                    Username = "Pedja D Boy",
                    City = "Curug",
                    Country = "Serbia",
                    Email = "reviewer3@hotmail.com",
                    IsActive = true,
                    Lastname = "Boy",
                    Role = UserRoles.Reviewer.ToString(),
                    EmailConfirmed = true,
                    Password = "reviewer3",
                    Firstname = "Pedja D",
                    ScientificAreas = "Anatomy, Chemistry"
                });

            dataContext.Users.Add(
                new User
                {
                    Username = "Ivan Drago",
                    Firstname = "Ivan",
                    Role = UserRoles.Reviewer.ToString(),
                    Email = "reviewer4@hotmail.com",
                    AdminConfirmed = true,
                    ScientificAreas = "Earth Science, Engineering",
                    IsActive = true,
                    EmailConfirmed = true,
                    Lastname = "Drago",
                    City = "Moscow",
                    Country = "Russia",
                    Password = "reviewer4"
                });

            dataContext.Users.Add(
                new User
                {
                    Username = "Raf Camora",
                    Password = "reviewer5",
                    City = "Vienna",
                    Country = "Austria",
                    Email = "reviewer5@hotmail.com",
                    IsActive = true,
                    EmailConfirmed = true,
                    Role = UserRoles.Reviewer.ToString(),
                    AdminConfirmed = true,
                    Firstname = "Raf",
                    Lastname = "Camora",
                    ScientificAreas = "Earth Science, Engineering"
                });

            dataContext.Users.Add(
                new User
                {
                    Username = "Colby Covington",
                    Password = "reviewer6",
                    Firstname = "Colby",
                    Lastname = "Covington",
                    IsActive = true,
                    EmailConfirmed = true,
                    Role = UserRoles.Reviewer.ToString(),
                    AdminConfirmed = true,
                    City = "Denver",
                    Country = "United States",
                    Email = "reviewer6@hotmail.com",
                    ScientificAreas = "Earth Science, Engineering"
                });

            dataContext.SaveChanges();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseDefaultServiceProvider(options => options.ValidateScopes = false)
                .UseStartup<Startup>();
    }
}
