FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["HostedServices/TrafficInfoHttpApi/TrafficInfoHttpApi.csproj", "HostedServices/TrafficInfoHttpApi/"]
COPY ["SharedKernel/Contracts/Contracts.csproj", "SharedKernel/Contracts/"]
RUN dotnet restore "HostedServices/TrafficInfoHttpApi/TrafficInfoHttpApi.csproj"
COPY . .
WORKDIR "/src/HostedServices/TrafficInfoHttpApi"
RUN dotnet build "TrafficInfoHttpApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TrafficInfoHttpApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TrafficInfoHttpApi.dll", "--environment=Docker"]