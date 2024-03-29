FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["DoctorsData.Web/DoctorsData.Web.csproj", "DoctorsData.Web/"]
COPY ["DoctorsData.Infrastructure/DoctorsData.Infrastructure.csproj", "DoctorsData.Infrastructure/"]
COPY ["DoctorsData.Domain/DoctorsData.Domain.csproj", "DoctorsData.Domain/"]
RUN dotnet restore "DoctorsData.Web/DoctorsData.Web.csproj"
COPY . .
WORKDIR "/src/DoctorsData.Web"
RUN dotnet build "DoctorsData.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DoctorsData.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DoctorsData.Web.dll"]