
FROM mcr.microsoft.com/dotnet/sdk:9.0@sha256:3fcf6f1e809c0553f9feb222369f58749af314af6f063f389cbd2f913b4ad556 AS build
WORKDIR /src
ENV ASPNETCORE_URLS=http://+:5000;https://+:5001

COPY ["Warehouse.csproj", "./"]
RUN dotnet restore "Warehouse.csproj" 
COPY . .
WORKDIR "/src/"
RUN dotnet build "Warehouse.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Warehouse.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0@sha256:b4bea3a52a0a77317fa93c5bbdb076623f81e3e2f201078d89914da71318b5d8
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 5000
EXPOSE 5001
ENTRYPOINT ["dotnet", "Warehouse.dll"]
