#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["RandomCappuccino.Server/RandomCappuccino.Server.csproj", "RandomCappuccino.Server/"]
RUN dotnet restore "RandomCappuccino.Server/RandomCappuccino.Server.csproj"
COPY . .
WORKDIR "/src/RandomCappuccino.Server"
RUN dotnet build "RandomCappuccino.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RandomCappuccino.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RandomCappuccino.Server.dll"]