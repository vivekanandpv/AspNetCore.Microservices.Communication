FROM mcr.microsoft.com/dotnet/aspnet:3.1
COPY bin/Release/netcoreapp3.1 app/
WORKDIR /app
ENTRYPOINT ["dotnet", "Microservice.B.dll"]