#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
ENV ASPNETCORE_URLS=http://*:5000
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["GISA-Auth.csproj", "."]
RUN dotnet restore "./GISA-Auth.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "GISA-Auth.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GISA-Auth.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GISA-Auth.dll"]