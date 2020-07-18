#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Neruko/Server/Neruko.Server.csproj", "Neruko/Server/"]
COPY ["Neruko/Shared/Neruko.Shared.csproj", "Neruko/Shared/"]
COPY ["Neruko/Client/Neruko.Client.csproj", "Neruko/Client/"]
RUN dotnet restore "Neruko/Server/Neruko.Server.csproj"
COPY . .
WORKDIR "/src/Neruko/Server"
RUN dotnet build "Neruko.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Neruko.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Neruko.Server.dll"]