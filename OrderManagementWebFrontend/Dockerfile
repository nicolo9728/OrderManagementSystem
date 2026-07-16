FROM nginx:alpine AS base
EXPOSE 8080
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["OrderManagementWebFrontend/OrderManagementWebFrontend.csproj", "OrderManagementWebFrontend/"]
COPY ["OrderManagementViewmodels/OrderManagementViewmodels.csproj", "OrderManagementViewmodels/"]

RUN dotnet restore "OrderManagementWebFrontend/OrderManagementWebFrontend.csproj"

COPY ["OrderManagementWebFrontend/", "OrderManagementWebFrontend/"]
COPY ["OrderManagementViewmodels/", "OrderManagementViewmodels/"]

WORKDIR /src/OrderManagementWebFrontend

RUN dotnet build "OrderManagementWebFrontend.csproj" -c Release -o /app/build
RUN dotnet publish "OrderManagementWebFrontend.csproj" -c Release -o /app/publish

FROM base as final

WORKDIR /usr/share/nginx/html

RUN rm -rf ./*

COPY --from=build /app/publish/wwwroot .

COPY OrderManagementWebFrontend/nginx.conf /etc/nginx/nginx.conf

CMD ["nginx", "-g", "daemon off;"]
