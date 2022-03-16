FROM mcr.microsoft.com/dotnet/sdk:5.0 as publish
WORKDIR app/
COPY src/services/AzureStore/AzureStore.API/*.csproj ./
#COPY *.csproj ./
RUN dotnet restore

COPY src/services/AzureStore/AzureStore.API/ ./
#COPY . ./
RUN dotnet publish -c Release -o out/

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR app/
COPY --from=publish app/out/ ./
EXPOSE 80
ENTRYPOINT ["dotnet", "AzureStore.API.dll"]