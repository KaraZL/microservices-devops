FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app
COPY src/services/Courses.API/*.csproj ./services/Courses.API/
#pas besoin de tout le projet pour récupérer les dépendences
COPY src/buildingblocks/Common.Logging/*.csproj ./buildingblocks/Common.Logging/
#necessite common.logging csproj apres l'ajout du distrubuted logging
RUN dotnet restore services/Courses.API/*.csproj

COPY src/services/Courses.API/ ./services/Courses.API/
#copie le reste des fichiers
COPY src/buildingblocks/Common.Logging/ ./buildingblocks/Common.Logging/
#copie le reste du logging aussi
RUN dotnet publish services/Courses.API/*.csproj -c Release -o out/
#on publish dans app/out

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /out
#nouveau dossier out dans une nouvelle image
COPY --from=build app/out/ ./
#copie le dossier out de BUILD (app/out) dans out de cette image
EXPOSE 80
ENTRYPOINT ["dotnet", "Courses.API.dll"]
#dotnet name.dll lance une app net

