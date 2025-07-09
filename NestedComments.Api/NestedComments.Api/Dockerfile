FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ./ ./
RUN dotnet restore "NestedComments.Api.csproj"
RUN dotnet publish "NestedComments.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
RUN apt-get update && \
    apt-get install -y fonts-liberation && \
    rm -rf /var/lib/apt/lists/*
ENTRYPOINT ["dotnet", "NestedComments.Api.dll"]