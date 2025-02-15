


namespace Va.Developer.Assessment.Api.Helpers.Extensions
{
    public static class SerilogExtensions
    {
        public static WebApplicationBuilder ConfigureSerilog(this WebApplicationBuilder builder, string connectionString)
		{
			var columnOptions = new ColumnOptions
			{
				Exception = { ColumnName = "ExceptionDetails" },
				Level = { ColumnName = "LogLevel", DataType = SqlDbType.NVarChar, DataLength = 50 },
				MessageTemplate = { ColumnName = "MessageTemplate", DataType = SqlDbType.NVarChar, DataLength = -1 },
				Message = { ColumnName = "Message", DataType = SqlDbType.NVarChar, DataLength = -1 }

			};
			var sinkOptions = new MSSqlServerSinkOptions
			{
				AutoCreateSqlTable = true,
				TableName = "Logs",
				SchemaName = "audit",
			};
			builder.Host.UseSerilog((context, services, loggerConfiguration) =>
				loggerConfiguration
					.Enrich.FromLogContext()
					.Enrich.WithProperty("MachineId", Environment.MachineName)
					.WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Debug)
					.WriteTo.MSSqlServer(
						connectionString: connectionString,
						sinkOptions: sinkOptions,
						columnOptions: columnOptions,
						restrictedToMinimumLevel: LogEventLevel.Error
					)
			);

			return builder;
		}
    }
}