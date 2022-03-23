FROM mcr.microsoft.com/dotnet/sdk:5.0 as publish
WORKDIR app/

#dans app/Movies.API/ et app/Movies.DataAccess/
COPY src/services/Movies/Movies.API/*.csproj Movies.API/
COPY src/services/Movies/Movies.DataAccess/*.csproj Movies.DataAccess/
#on publish uniquement api
RUN dotnet restore Movies.API/*.csproj

#tout dans app/Movies.API/ et app/Movies.DataAccess/
COPY src/services/Movies/Movies.API/ Movies.API/
COPY src/services/Movies/Movies.DataAccess/ Movies.DataAccess/

#on publish uniquement api
RUN dotnet publish Movies.API/*.csproj -c Release -o out/

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR app/
COPY --from=publish app/out/ ./
EXPOSE 80
ENTRYPOINT ["dotnet", "Movies.API.dll"]