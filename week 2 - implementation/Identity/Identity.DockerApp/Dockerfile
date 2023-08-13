FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Identity.DockerApp/Identity.DockerApp.csproj", "Identity.DockerApp/"]
COPY ["Identity.Application/Identity.Application.csproj", "Identity.Application/"]
COPY ["Identity.Shared/Identity.Shared.csproj", "Identity.Shared/"]
COPY ["Identity.Domain/Identity.Domain.csproj", "Identity.Domain/"]
COPY ["Identity.Integration/Identity.Integration.csproj", "Identity.Integration/"]
COPY ["Identity.Persistence/Identity.Persistence.csproj", "Identity.Persistence/"]
COPY ["Identity.Web/Identity.Web.csproj", "Identity.Web/"]
RUN dotnet restore "Identity.DockerApp/Identity.DockerApp.csproj"
COPY . .
WORKDIR "/src/Identity.DockerApp"
RUN dotnet build "Identity.DockerApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Identity.DockerApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.DockerApp.dll"]