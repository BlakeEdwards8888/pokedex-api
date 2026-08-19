using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Pokedex.API;
using Pokedex.API.Contexts;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
WebApplication app = ConfigureServices(builder);

// Configure the HTTP request pipeline.
ConfigurePipeline(app);

app.Run();

static WebApplication ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services.AddControllers(options =>
    {
        options.ReturnHttpNotAcceptable = true;
    }).AddNewtonsoftJson()
    .AddXmlDataContractSerializerFormatters();


    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddApiVersioning(setupAction =>
    {
        setupAction.ReportApiVersions = true;
        setupAction.AssumeDefaultVersionWhenUnspecified = true;
        setupAction.DefaultApiVersion = new ApiVersion(1, 0);
        setupAction.ApiVersionReader = new UrlSegmentApiVersionReader();
    }).AddMvc().AddApiExplorer(setupAction =>
    {
        setupAction.GroupNameFormat = "'v'VVV";
        setupAction.SubstituteApiVersionInUrl = true;
    });

    var apiVersionDescriptionProvider = builder.Services.BuildServiceProvider()
    .GetRequiredService<IApiVersionDescriptionProvider>();

    builder.Services.AddSwaggerGen(setupAction =>
    {
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            setupAction.SwaggerDoc($"{description.GroupName}",
                new OpenApiInfo()
                {
                    Title = "Pokedex API",
                    Description = "A pokedex API for storing and retrieving pokemon data",
                    Version = description.ApiVersion.ToString()
                });

            var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

            setupAction.IncludeXmlComments(xmlCommentsFullPath);
        }
    });

    builder.Services.AddSingleton<FileExtensionContentTypeProvider>();
    builder.Services.AddSingleton<PokemonDataStore>();
    builder.Services.AddDbContext<PokedexContext>(
        dbContextOptions => dbContextOptions.UseSqlite(builder.Configuration["ConnectionStrings:PokedexDBConnectionString"])
        );

    return builder.Build();
}

static void ConfigurePipeline(WebApplication app)
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHttpsRedirection();

    app.UseRouting();

    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });

    app.MapControllers();
}