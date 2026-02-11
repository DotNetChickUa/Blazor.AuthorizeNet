# STAGE 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copy csproj and restore

COPY Blazor.AuthorizeNet/. ./Blazor.AuthorizeNet/
COPY Sample/. ./Sample/
RUN dotnet restore ./Sample/Sample.csproj
WORKDIR /app/Sample

# Build
RUN dotnet publish -c Release -o out

# STAGE 2: Run
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/Sample/out ./
ENTRYPOINT ["dotnet", "Sample.dll"]