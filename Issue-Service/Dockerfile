FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Issue-Service/Issue-Service.csproj", "Issue-Service/"]
RUN dotnet restore "Issue-Service/Issue-Service.csproj"
COPY . .
WORKDIR "/src/Issue-Service"
RUN dotnet build "Issue-Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Issue-Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Issue-Service.dll"]
