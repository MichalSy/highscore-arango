#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/nightly/aspnet:latest AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/nightly/sdk:latest AS build
WORKDIR /src
COPY ["Misy.Highscore.Service/Misy.Highscore.Service.csproj", "Misy.Highscore.Service/"]
RUN dotnet restore "Misy.Highscore.Service/Misy.Highscore.Service.csproj"
COPY . .
WORKDIR "/src/Misy.Highscore.Service"
RUN dotnet build "Misy.Highscore.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Misy.Highscore.Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Misy.Highscore.Service.dll"]