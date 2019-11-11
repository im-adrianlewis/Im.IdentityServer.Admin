FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-stretch AS build
WORKDIR /src
COPY "Im.Access.GraphPortal.csproj" .
RUN dotnet restore "Im.Access.GraphPortal.csproj"
COPY . .
RUN dotnet build "Im.Access.GraphPortal.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Im.Access.GraphPortal.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Im.Access.GraphPortal.dll"]