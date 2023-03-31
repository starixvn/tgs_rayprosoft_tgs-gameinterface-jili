#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BuildConfiguration=Release
WORKDIR /src
COPY ["NuGet.config", "."]
COPY ["Tgs.GameInterface.JiliService/Tgs.GameInterface.JiliService.csproj", "Tgs.GameInterface.JiliService/"]
COPY ["Tgs.GameInterface.JiliService.Core/Tgs.GameInterface.JiliService.Core.csproj", "Tgs.GameInterface.JiliService.Core/"]
RUN dotnet restore "Tgs.GameInterface.JiliService/Tgs.GameInterface.JiliService.csproj"
COPY . .
WORKDIR "/src/Tgs.GameInterface.JiliService"
RUN dotnet build "Tgs.GameInterface.JiliService.csproj" -c $BuildConfiguration -o /app/build

FROM build AS publish
ARG BuildConfiguration
RUN dotnet publish "Tgs.GameInterface.JiliService.csproj" -c $BuildConfiguration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.GameInterface.JiliService.dll"]