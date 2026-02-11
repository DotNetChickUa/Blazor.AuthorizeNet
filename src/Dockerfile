# Use the .NET 8 SDK image to build the Blazor app
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the rest of the source code into the container
COPY . ./

# Publish the app in Release mode to the 'out' folder
RUN dotnet publish Sample/Sample.csproj -c Release -o out

# Use the Nginx base image to serve the published app
FROM nginx

# Set a working directory (optional, for consistency)
WORKDIR /app

# Expose port 80 (the default port Nginx listens on)
EXPOSE 80

# Copy the custom Nginx configuration file into the container
COPY default.conf /etc/nginx/conf.d/default.conf

# Copy the published Blazor files to the Nginx web root
COPY --from=build /app/out/wwwroot /usr/share/nginx/html