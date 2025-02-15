using System.Text;

try
{

    var builder = WebApplication.CreateBuilder(args);
    string connection = builder.Configuration.GetConnectionString("DefaultConnection");

    builder.ConfigureSerilog(connection);
    builder.Services.ConfigureDbContext(connection);
    builder.Services.ConfigureRepositories();

    builder.Services.ConfigureServices();
    builder.Services.ConfigureAutoMapperAndValidators();

    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();


    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.Services.EnsurerDatabaseCreate().Wait();
    app.UseExceptionHandler();
    app.UseHttpsRedirection();
    app.MapControllers();


    app.Run();
}
catch (Exception ex) when( ex is not HostAbortedException)
{
    var sb = new StringBuilder();
    sb.Append($"Error: {ex.Message}");
    if(ex.InnerException is not null){
        sb.AppendLine($"Detailed Error: {ex.InnerException.Message}");
    }
    Log.Fatal(exception: ex,"An unexpected fatal error occured. {@Message}. ", sb.ToString());
}
finally {
    Log.CloseAndFlush();
}