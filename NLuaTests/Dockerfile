FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["NluaTests/NluaTests.csproj", "NluaTests/"]
RUN dotnet restore "NluaTests/NluaTests.csproj"
COPY . .
WORKDIR "/src/NluaTests"
RUN dotnet build "NluaTests.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NluaTests.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NluaTests.dll"]
