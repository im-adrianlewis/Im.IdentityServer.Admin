FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Im.Access.GraphPortal/Im.Access.GraphPortal.csproj" "Im.Access.GraphPortal/"]
RUN dotnet restore "Im.Access.GraphPortal/Im.Access.GraphPortal.csproj"
COPY . .
WORKDIR /src/Im.Access.GraphPortal
RUN dotnet build "Im.Access.GraphPortal.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Im.Access.GraphPortal.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Im.Access.GraphPortal.dll"]