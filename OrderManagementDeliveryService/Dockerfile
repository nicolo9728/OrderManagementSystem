FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080


FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src


COPY ["OrderManagementDeliveryService/OrderManagementDeliveryService.csproj", "OrderManagementDeliveryService/"]
COPY ["OrderManagementCommon/OrderManagementCommon.csproj", "OrderManagementCommon/"]
COPY ["OrderManagementViewmodels/OrderManagementViewmodels.csproj", "OrderManagementViewmodels/"]


RUN dotnet restore "OrderManagementDeliveryService/OrderManagementDeliveryService.csproj"


COPY ["OrderManagementDeliveryService/", "OrderManagementDeliveryService/"]
COPY ["OrderManagementCommon/", "OrderManagementCommon/"]
COPY ["OrderManagementViewmodels/", "OrderManagementViewmodels/"]


WORKDIR "/src/OrderManagementDeliveryService"
RUN dotnet build "OrderManagementDeliveryService.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "OrderManagementDeliveryService.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderManagementDeliveryService.dll"]
