FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080


FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src


COPY ["OrderManagementUserService/OrderManagementUserService.csproj", "OrderManagementUserService/"]
COPY ["OrderManagementCommon/OrderManagementCommon.csproj", "OrderManagementCommon/"]
COPY ["OrderManagementViewmodels/OrderManagementViewmodels.csproj", "OrderManagementViewmodels/"]


RUN dotnet restore "OrderManagementUserService/OrderManagementUserService.csproj"


COPY ["OrderManagementUserService/", "OrderManagementUserService/"]
COPY ["OrderManagementCommon/", "OrderManagementCommon/"]
COPY ["OrderManagementViewmodels/", "OrderManagementViewmodels/"]


WORKDIR "/src/OrderManagementUserService"
RUN dotnet build "OrderManagementUserService.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "OrderManagementUserService.csproj" -c Release -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderManagementUserService.dll"]
