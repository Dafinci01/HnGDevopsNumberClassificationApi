# Use the official ASP.NET Core runtime as a base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["HnGDevopsNumberClassificationApi.csproj", "."]
RUN dotnet restore "HnGDevopsNumberClassificationApi.csproj"

# Copy the remaining source code
COPY . .
WORKDIR "/src"

# Build the project in Release mode
RUN dotnet build "HnGDevopsNumberClassificationApi.csproj" -c Release -o /app/build

# Publish the project to the /app/publish folder
FROM build AS publish
RUN dotnet publish "HnGDevopsNumberClassificationApi.csproj" -c Release -o /app/publish

# Final stage: use the runtime image and copy the published output
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://0.0.0.0:5000
ENTRYPOINT ["dotnet", "HnGDevopsNumberClassificationApi.dll"]

