FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
# ENV ASPNETCORE_URLS=http://+:80
# ENV ASPNETCORE_HTTPS_PORT=https://+:80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["EnglishVibes.API/EnglishVibes.API.csproj", "EnglishVibes.API/"]
COPY ["EnglishVibes.Core/EnglishVibes.Core.csproj", "EnglishVibes.Core/"]
COPY ["EnglishVibes.Data/EnglishVibes.Data.csproj", "EnglishVibes.Data/"]
COPY ["EnglishVibes.Infrastructure/EnglishVibes.Infrastructure.csproj", "EnglishVibes.Infrastructure/"]
COPY ["EnglishVibes.Service/EnglishVibes.Service.csproj", "EnglishVibes.Service/"]
RUN dotnet restore "EnglishVibes.API/EnglishVibes.API.csproj"
COPY . .
WORKDIR "/src/EnglishVibes.API"
RUN dotnet build "EnglishVibes.API.csproj" -c Release -o /app/build
# ENV ASPNETCORE_URLS=http://+:80
# ENV ASPNETCORE_HTTPS_PORT=https://+:80



FROM build AS publish 
RUN dotnet publish "EnglishVibes.API.csproj" -c Release -o /app/publish /p:UseAppHost=false
# ENV ASPNETCORE_URLS=http://+:80
# ENV ASPNETCORE_HTTPS_PORT=https://+:80



FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EnglishVibes.API.dll"]
# ENV ASPNETCORE_URLS=http://+:80
# ENV ASPNETCORE_HTTPS_PORT=https://+:80


