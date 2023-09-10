#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Accounting.DockerApp/Accounting.DockerApp.csproj", "Accounting.DockerApp/"]
COPY ["Accounting.Application/Accounting.Application.csproj", "Accounting.Application/"]
COPY ["Accounting.Shared/Accounting.Shared.csproj", "Accounting.Shared/"]
COPY ["Accounting.Domain/Accounting.Domain.csproj", "Accounting.Domain/"]
COPY ["Accounting.Integration/Accounting.Integration.csproj", "Accounting.Integration/"]
COPY ["Accounting.Persistence/Accounting.Persistence.csproj", "Accounting.Persistence/"]
COPY ["Accounting.Web/Accounting.Web.csproj", "Accounting.Web/"]
RUN dotnet restore "Accounting.DockerApp/Accounting.DockerApp.csproj"
COPY . .
WORKDIR "/src/Accounting.DockerApp"
RUN dotnet build "Accounting.DockerApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Accounting.DockerApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Accounting.DockerApp.dll"]