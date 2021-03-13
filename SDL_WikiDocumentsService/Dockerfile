#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["SDL_WikiDocumentsService/SDL_WikiDocumentsService.csproj", "SDL_WikiDocumentsService/"]
RUN dotnet restore "SDL_WikiDocumentsService/SDL_WikiDocumentsService.csproj"
COPY . .
WORKDIR "/src/SDL_WikiDocumentsService"
RUN dotnet build "SDL_WikiDocumentsService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SDL_WikiDocumentsService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SDL_WikiDocumentsService.dll"]