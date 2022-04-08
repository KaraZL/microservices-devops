FROM mcr.microsoft.com/dotnet/sdk:6.0 AS publish
WORKDIR app/
COPY src/monitoring/WebStatus/*.csproj ./
RUN dotnet restore

COPY src/monitoring/WebStatus/ ./
RUN dotnet publish -c Release -o out/

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR app/
COPY --from=publish app/out/ ./
EXPOSE 80
ENTRYPOINT ["dotnet", "WebStatus.dll"]