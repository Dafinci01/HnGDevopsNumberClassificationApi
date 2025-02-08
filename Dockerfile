# Use the official ASP.NET Core runtime as a base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000

# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

#Copy just the project files and restore dependencies
COPY ClassifyNumber.csproj .
COPY Properties/ Properties/
COPY Controller/ Controller/
COPY appsettings.Development.json .
COPY appsettings.json .
COPY Program.cs .


# Copy the project file and restore dependencies
RUN dotnet restore "ClassifyNumber.csproj"

# Copy the remaining source code
COPY . .
WORKDIR "/src"

# Build the project in Release mode
RUN dotnet build "ClassifyNumber.csproj" -c Release -o /app/build

# Publish the project to the /app/publish folder
FROM build AS publish
RUN dotnet publish "ClassifyNumber.csproj" -c Rlease -o /app/publish

# Final stage: use the runtime image and copy the published output
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set the environment variables:
# Bind the app to all interfaces on port 5000
ENV ASPNETCORE_URLS=http://0.0.0.0:5000
# Set the environment to Production so that development-only features like Swagger are disabled
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "ClassifyNumber.dll"]

