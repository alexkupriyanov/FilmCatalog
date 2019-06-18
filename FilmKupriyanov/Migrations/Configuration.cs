using FilmKupriyanov.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FilmKupriyanov.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FilmKupriyanov.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(FilmKupriyanov.Models.ApplicationDbContext context)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            if (!context.Users.Any(u => u.UserName == "admin@admin.net"))
            {
                var PasswordHash = new PasswordHasher();
                var user = new ApplicationUser
                {
                    UserName = "admin@admin.net",
                    Email = "admin@admin.net",
                    PasswordHash = PasswordHash.HashPassword("asdQWE1@3")
                };
                userManager.Create(user);

            }
            if (!context.Users.Any(u => u.UserName == "moderator@admin.net"))
            {
                var PasswordHash = new PasswordHasher();
                var user = new ApplicationUser
                {
                    UserName = "moderator@admin.net",
                    Email = "moderator@admin.net",
                    PasswordHash = PasswordHash.HashPassword("asdQWE1@3")
                };
                userManager.Create(user);
            }
            context.Films.AddOrUpdate(x => x.Id,
                new Film()
                {
                    Id = Guid.NewGuid(),
                    Name ="Titanic",
                    Description = "A seventeen-year-old aristocrat falls in love with a kind but poor artist aboard the luxurious, ill-fated R.M.S. Titanic.",
                    Year = 1997,
                    Director = "James Cameron", 
                    CreatorUserId = userManager.Users.First(x => x.UserName == "admin@admin.net").Id,
                    Poster = "/Images/2019-06-18cd200f2c-1016-46b5-b58e-aa4fe7103d22.jpg"
                },
                new Film()
                {
                    Id = Guid.NewGuid(),
                    Name = "The Terminator",
                    Description = "A seemingly indestructible robot is sent from 2029 to 1984 to assassinate a young waitress, whose unborn son will lead humanity in a war against sentient machines, while a human soldier from the same war is sent to protect her at all costs.",
                    Year = 1984,
                    Director = "James Cameron",
                    CreatorUserId = userManager.Users.First(x => x.UserName == "admin@admin.net").Id,
                    Poster = "/Images/2019-06-187c8e1d4e-a6ad-4ad5-be7a-4a8bcc32843c.jpg"
                },
                new Film()
                {
                    Id = Guid.NewGuid(),
                    Name = "The Matrix",
                    Description = "A computer hacker learns from mysterious rebels about the true nature of his reality and his role in the war against its controllers.",
                    Year = 1999,
                    Director = "The Wachowski Brothers",
                    CreatorUserId = userManager.Users.First(x => x.UserName == "moderator@admin.net").Id,
                    Poster = "/Images/2019-06-18cf921dea-823a-4e3e-b539-b36c754c164f.jpg"
                },
                new Film()
                {
                    Id = Guid.NewGuid(),
                    Name = "Deadpool",
                    Description = "A wisecracking mercenary gets experimented on and becomes immortal but ugly, and sets out to track down the man who ruined his looks.",
                    Year = 2016,
                    Director = "Tim Miller",
                    CreatorUserId = userManager.Users.First(x => x.UserName == "moderator@admin.net").Id,
                    Poster = "/Images/2019-06-18072a3d14-3771-4b42-ad6a-f7808e505cfd.jpg"

                }
                );
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
