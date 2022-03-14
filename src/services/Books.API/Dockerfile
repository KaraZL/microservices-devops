FROM mcr.microsoft.com/dotnet/sdk:5.0 as publish
WORKDIR app/
COPY src/services/Books.API/*.csproj ./
RUN dotnet restore

COPY src/services/Books.API/ ./
RUN dotnet publish -c Release -o out/

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR app/
COPY --from=publish app/out/ ./
EXPOSE 80
ENTRYPOINT ["dotnet", "Books.API.dll"]
