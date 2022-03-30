FROM mcr.microsoft.com/dotnet/sdk:6.0 AS publish
WORKDIR app/
COPY src/services/Series/Series.API/*.csproj ./Series.API/
COPY src/services/Series/Series.Application/*.csproj ./Series.Application/
COPY src/services/Series/Series.Domain/*.csproj ./Series.Domain/
COPY src/services/Series/Series.Infrastructure/*.csproj ./Series.Infrastructure/
RUN dotnet restore Series.API/*.csproj

COPY src/services/Series/ ./
RUN dotnet publish Series.API/*.csproj -c Release -o out/

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR app/
COPY --from=publish app/out/ ./
EXPOSE 80
ENTRYPOINT ["dotnet", "Series.API.dll"]


