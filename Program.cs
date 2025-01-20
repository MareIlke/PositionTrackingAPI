    
    var builder = WebApplication.CreateBuilder(args);

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    
    builder.Services.AddScoped<IPositionRepository, PositionRepository>(provider => {
        var logger = provider.GetRequiredService<ILogger<PositionRepository>>(); 
        return new PositionRepository(connectionString, logger); 
    });
    builder.Services.AddScoped<IPositionService, PositionService>();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddLogging();
    

    var app = builder.Build();

    app.UseMiddleware<ExceptionHandling>(); 

    app.UseMiddleware<RequestLogging>(); 

    if (app.Environment.IsDevelopment()) {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();