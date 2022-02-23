FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app
COPY *.csproj ./
#pas besoin de tout le projet pour récupérer les dépendences
RUN dotnet restore

COPY . ./
#copie le reste des fichiers
RUN dotnet publish -c Release -o /out
#on publish dans app/out

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /out
#nouveau dossier out dans une nouvelle image
COPY --from=build /out ./
#copie le dossier out de BUILD (app/out) dans out de cette image
EXPOSE 80
ENTRYPOINT ["dotnet", "Courses.API.dll"]
#dotnet name.dll lance une app net

