FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TaskManager.DockerApp/TaskManager.DockerApp.csproj", "TaskManager.DockerApp/"]
COPY ["TaskManager.Application/TaskManager.Application.csproj", "TaskManager.Application/"]
COPY ["TaskManager.Shared/TaskManager.Shared.csproj", "TaskManager.Shared/"]
COPY ["TaskManager.Domain/TaskManager.Domain.csproj", "TaskManager.Domain/"]
COPY ["TaskManager.Integration/TaskManager.Integration.csproj", "TaskManager.Integration/"]
COPY ["TaskManager.Persistence/TaskManager.Persistence.csproj", "TaskManager.Persistence/"]
COPY ["TaskManager.Web/TaskManager.Web.csproj", "TaskManager.Web/"]
RUN dotnet restore "TaskManager.DockerApp/TaskManager.DockerApp.csproj"
COPY . .
WORKDIR "/src/TaskManager.DockerApp"
RUN dotnet build "TaskManager.DockerApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TaskManager.DockerApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskManager.DockerApp.dll"]