var builder = WebApplication.CreateSlimBuilder(args);

builder.Logging.AddConsole();
builder.Services.AddCors();
builder.Configuration.AddEnvironmentVariables()
                     .AddUserSecrets(Assembly.GetExecutingAssembly(), true);

builder.Services.AddSingleton((_) =>
    new ArangoDBClient(HttpApiTransport.UsingBasicAuth(
        new Uri(builder.Configuration["ArangoDB:Host"]!),
        builder.Configuration["ArangoDB:Database"]!,
        builder.Configuration["ArangoDB:Username"]!, 
        builder.Configuration["ArangoDB:Password"]!))
);

var app = builder.Build();
app.UseCors(c => c.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
app.UseScoresApi();
app.Run();