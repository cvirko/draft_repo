FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Auth.Api/Auth.Api.csproj", "Auth.Api/"]
COPY ["Auth.Api.Infrastructure.Logic.Write/*.csproj", "Auth.Api.Infrastructure.Logic.Write/"]
COPY ["Auth.Api.Infrastructure.Logic.Validation/*.csproj", "Auth.Api.Infrastructure.Logic.Validation/"]
COPY ["Auth.Api.Infrastructure.Logic.Read/*.csproj", "Auth.Api.Infrastructure.Logic.Read/"]
COPY ["Auth.Api.Infrastructure.Logic.External/*.csproj", "Auth.Api.Infrastructure.Logic.External/"]
COPY ["Auth.Api.Infrastructure.Logic.Notification/*.csproj", "Auth.Api.Infrastructure.Logic.Notification/"]
COPY ["Auth.Api.Infrastructure.Data/*.csproj", "Auth.Api.Infrastructure.Data/"]
COPY ["Auth.Api.Domain.Interface.Logic.Read/*.csproj", "Auth.Api.Domain.Interface.Logic.Read/"]
COPY ["Auth.Api.Domain.Interface.Logic.External/*.csproj", "Auth.Api.Domain.Interface.Logic.External/"]
COPY ["Auth.Api.Domain.Interface.Logic.Notification/*.csproj", "Auth.Api.Domain.Interface.Logic.Notification/"]
COPY ["Auth.Api.Domain.Interface.Data.Write/*.csproj", "Auth.Api.Domain.Interface.Data.Write/"]
COPY ["Auth.Api.Domain.Interface.Data.Read/*.csproj", "Auth.Api.Domain.Interface.Data.Read/"]
COPY ["Auth.Api.Domain.Core.Logic/*.csproj", "Auth.Api.Domain.Core.Logic/"]
COPY ["Auth.Api.Domain.Core.Data/*.csproj", "Auth.Api.Domain.Core.Data/"]
COPY ["Auth.Api.Domain.Core.Common/*.csproj", "Auth.Api.Domain.Core.Common/"]
COPY ["Library.CommandMediator/*.csproj", "Library.CommandMediator/"]

RUN dotnet restore "./Auth.Api/Auth.Api.csproj"
COPY . .
WORKDIR "/src/Auth.Api"
RUN dotnet build "./Auth.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Auth.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Auth.Api.dll"]