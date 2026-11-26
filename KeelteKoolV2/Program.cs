using KeelteKoolV2.ApplicationServices.Services;
using KeelteKoolV2.Core.Domain;
using KeelteKoolV2.Core.ServiceInterface;
using KeelteKoolV2.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KeelteKoolV2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            //services
            builder.Services.AddScoped<IEmailingServices, EmailingServices>();
            builder.Services.AddScoped<ILanguageCoursesServices, LanguageCoursesServices>();
            builder.Services.AddScoped<IFilesServices, FilesServices>();
            builder.Services.AddScoped<ILecturersServices, LecturersServices>();
            builder.Services.AddScoped<IStudentsServices, StudentsService>();

            //swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //dbcontext
            builder.Services.AddDbContext<KeelteKoolV2Context>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            //identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
                options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                    options.Password.RequiredLength = 8;
                    options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";
                    options.Lockout.MaxFailedAccessAttempts = 3;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                })
                .AddEntityFrameworkStores<KeelteKoolV2Context>()
                .AddDefaultTokenProviders()
                .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>("CustomEmailConfirmation");

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<KeelteKoolV2Context>();
                db.Database.Migrate();

                if (!db.Students.Any())
                {
                    db.Students.AddRange(
                        new Student
                        {
                            Id = Guid.NewGuid(),
                            FirstName = "Mari",
                            LastName = "Maasikas",
                            Email = "mari.maasikas@tthk.ee",
                            CreatedAt = DateTime.Now,
                            ModifiedAt = DateTime.Now
                        },
                        new Student
                        {
                            Id = Guid.NewGuid(),
                            FirstName = "Jaan",
                            LastName = "Tamm",
                            Email = "jaan.tamm@tthk.ee",
                            CreatedAt = DateTime.Now,
                            ModifiedAt = DateTime.Now
                        }
                    );
                }

                if (!db.Lecturers.Any())
                {
                    db.Lecturers.AddRange(
                        new Lecturer
                        {
                            Id = Guid.NewGuid(),
                            FirstName = "Kati",
                            LastName = "Kask",
                            Qualifications = "Magistrikraad, C2 tase",
                            CreatedAt = DateTime.Now,
                            ModifiedAt = DateTime.Now
                        },
                        new Lecturer
                        {
                            Id = Guid.NewGuid(),
                            FirstName = "Peeter",
                            LastName = "Põld",
                            Qualifications = "Bakalaureus, B2 tase",
                            CreatedAt = DateTime.Now,
                            ModifiedAt = DateTime.Now
                        }
                    );
                }

                if (!db.LanguageCourses.Any())
                {
                    db.LanguageCourses.AddRange(
                        new LanguageCourse
                        {
                            Id = Guid.NewGuid(),
                            Nimetus = "Eesti keel algajatele",
                            Keel = "Eesti",
                            Tase = "A2",
                            Kirjeldus = "Põhikursus igapäevaseks suhtluseks.",
                            CreatedAt = DateTime.Now,
                            ModifiedAt = DateTime.Now
                        },
                        new LanguageCourse
                        {
                            Id = Guid.NewGuid(),
                            Nimetus = "Inglise ärikeel",
                            Keel = "Inglise",
                            Tase = "B2",
                            Kirjeldus = "Ärisuhtlus ja kirjavahetus.",
                            CreatedAt = DateTime.Now,
                            ModifiedAt = DateTime.Now
                        }
                    );
                }

                db.SaveChanges();
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = "docs";
            });

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}