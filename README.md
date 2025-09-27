# OpenTelemetry Example Project

This is a sample Web API application developed using .NET 9.0, demonstrating OpenTelemetry integration. The project serves as an example implementing modern observability practices.

## Features

- ✨ .NET 9.0 Web API
- 📊 OpenTelemetry integration
  - Traces
  - Metrics
  - Logs
- 🛢️ PostgreSQL database integration
- 🐳 Docker support
- 🔄 Entity Framework Core
- 📚 Swagger/OpenAPI support

## Getting Started

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/install/)
- [SigNoz](https://signoz.io/docs/install/docker/) - For telemetry data visualization and analysis


### Running the Project

1. Clone the repository:
```bash
git clone https://github.com/seyfullahcuhadar/OpenTelemetryExample
cd OpenTelemetryExample
```

2. Start the application using Docker Compose:
```bash
docker compose up -d
```

The application will be available at http://localhost:5038.

### Endpoints

- **API**: http://localhost:5038
- **Swagger UI**: http://localhost:5038/swagger
- **PostgreSQL**: localhost:5432

## OpenTelemetry Configuration

The project includes the following OpenTelemetry components:

### Traces
- ASP.NET Core instrumentation
- Entity Framework Core instrumentation
- Npgsql (PostgreSQL) instrumentation
- HTTP Client instrumentation

### Metrics
- ASP.NET Core metrics
- HTTP Client metrics

### Logs
- OpenTelemetry log provider integration

The application is configured to send telemetry data using the OTLP (OpenTelemetry Protocol) over HTTP. This project is configured to work with SigNoz for visualizing and analyzing telemetry data.

Before running the application, please install and set up SigNoz by following the official documentation:
[SigNoz Installation Guide](https://signoz.io/docs/install/docker/)

When SigNoz is installed locally using Docker, it configures its OTEL Collector to listen on port 4318 for OTLP HTTP protocol. Therefore, this application is configured to send telemetry data to the following default endpoints:

- Traces: http://localhost:4318/v1/traces
- Metrics: http://localhost:4318/v1/metrics
- Logs: http://localhost:4318/v1/logs

These are the standard endpoints where SigNoz's OTEL Collector receives telemetry data when running locally. After setting up SigNoz, all your telemetry data will be automatically collected and can be viewed in the SigNoz UI.

## Database

PostgreSQL database runs in a Docker container. Connection details:

- Database: OpenTelemetryExample
- Username: postgres
- Password: postgres
- Port: 5432

## Development

To run the project in development environment:

```bash
cd OpenTelemetryExample
dotnet run
```

## License

This project is licensed under the [MIT License](LICENSE).