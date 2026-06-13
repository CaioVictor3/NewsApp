using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NewsApp.Application.Services;
using NewsApp.Application.Interface;
using NewsApp.Infrastructure.DBContext;
using Microsoft.Data.Sqlite;

namespace NewsApp.Api
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            _environment = environment;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(LibraryEntrypoint).Assembly));
            
            var connection = ResolveSqliteConnectionString(Configuration.GetConnectionString("DefaultConnection"));
            services.AddDbContext<Context>(options => options.UseSqlite(connection));

            services.AddControllers();
            services.AddHttpClient();

            #region Serviços
            services.AddHttpContextAccessor();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IComentarioService, ComentarioService>();
			services.AddScoped<INoticiaService, NoticiaService>();
			services.AddScoped<IFavoritoService, FavoritoService>();
			#endregion

            // Token JWT
            var key = Encoding.ASCII.GetBytes("11ccc561fdbf0dc949f2a7739606973e94d915b971b250d530e43ff651e8db1d");
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NewsApp_API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    new string[] {}
                }});
            });
        }

        internal class LibraryEntrypoint {}

        private string ResolveSqliteConnectionString(string? connectionString)
        {
            var builder = new SqliteConnectionStringBuilder(connectionString ?? "Data Source=newsapp.db");

            if (!string.IsNullOrWhiteSpace(builder.DataSource)
                && builder.DataSource != ":memory:"
                && !Path.IsPathRooted(builder.DataSource))
            {
                builder.DataSource = Path.Combine(FindSolutionRoot(), builder.DataSource);
            }

            return builder.ToString();
        }

        private string FindSolutionRoot()
        {
            var directory = new DirectoryInfo(_environment.ContentRootPath);

            while (directory != null && !File.Exists(Path.Combine(directory.FullName, "NewsApp.sln")))
            {
                directory = directory.Parent;
            }

            return directory?.FullName ?? Directory.GetCurrentDirectory();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            if (!env.IsDevelopment())
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseCors(x => x
                  .AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader());

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
