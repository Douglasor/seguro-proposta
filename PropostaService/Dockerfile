FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["PropostaService.csproj", "."]
RUN dotnet restore "./PropostaService.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "PropostaService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PropostaService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .



# Copy migration script
COPY entrypoint.sh /entrypoint.sh
RUN chmod +x /entrypoint.sh

ENTRYPOINT ["/entrypoint.sh"]

