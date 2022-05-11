#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WithoutDocker/WithoutDocker.csproj", "WithoutDocker/"]
RUN dotnet restore "WithoutDocker/WithoutDocker.csproj"
COPY . .
WORKDIR "/src/WithoutDocker"
RUN dotnet build "WithoutDocker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WithoutDocker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WithoutDocker.dll"]