FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["PropostaContratacao.sln", "./"]

COPY . .

RUN dotnet restore "PropostaContratacao.sln" --verbosity detailed

WORKDIR "/src/ContratacaoApi/api"

RUN dotnet build "./api.csproj" -c $BUILD_CONFIGURATION -o /app/build --verbosity detailed

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
RUN dotnet publish "ContratacaoApi/api/api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "api.dll"]
CMD []
