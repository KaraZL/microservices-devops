FROM mcr.microsoft.com/dotnet/sdk:6.0 AS publish
WORKDIR app/
COPY src/services/Series/Series.API/*.csproj ./services/Series/Series.API/
COPY src/services/Series/Series.Application/*.csproj ./services/Series/Series.Application/
COPY src/services/Series/Series.Domain/*.csproj ./services/Series/Series.Domain/
COPY src/services/Series/Series.Infrastructure/*.csproj ./services/Series/Series.Infrastructure/
COPY src/buildingblocks/Common.Logging/*csproj ./buildingblocks/Common.Logging/
RUN dotnet restore ./services/Series/Series.API/*.csproj

COPY src/services/Series/ ./services/Series/
COPY src/buildingblocks/Common.Logging/ ./buildingblocks/Common.Logging/
RUN dotnet publish ./services/Series/Series.API/*.csproj -c Release -o out/

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR app/
COPY --from=publish app/out/ ./
EXPOSE 80
ENTRYPOINT ["dotnet", "Series.API.dll"]


