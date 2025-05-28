FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TurnstilePoC.csproj", "./"]
RUN dotnet restore "TurnstilePoC.csproj"
COPY . .
RUN dotnet build "TurnstilePoC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TurnstilePoC.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TurnstilePoC.dll"]