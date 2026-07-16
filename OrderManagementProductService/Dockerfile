FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080


FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src


COPY ["OrderManagementProductService/OrderManagementProductService.csproj", "OrderManagementProductService/"]
COPY ["OrderManagementCommon/OrderManagementCommon.csproj", "OrderManagementCommon/"]
COPY ["OrderManagementViewmodels/OrderManagementViewmodels.csproj", "OrderManagementViewmodels/"]


RUN dotnet restore "OrderManagementProductService/OrderManagementProductService.csproj"


COPY ["OrderManagementProductService/", "OrderManagementProductService/"]
COPY ["OrderManagementCommon/", "OrderManagementCommon/"]
COPY ["OrderManagementViewmodels/", "OrderManagementViewmodels/"]


WORKDIR "/src/OrderManagementProductService"
RUN dotnet build "OrderManagementProductService.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "OrderManagementProductService.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderManagementProductService.dll"]
