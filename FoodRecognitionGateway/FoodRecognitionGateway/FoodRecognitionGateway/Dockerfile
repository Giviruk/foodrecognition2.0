FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FoodRecognitionGateway/FoodRecognitionGateway.csproj", "FoodRecognitionGateway/"]
RUN dotnet restore "FoodRecognitionGateway/FoodRecognitionGateway.csproj"
COPY . .
WORKDIR "/src/FoodRecognitionGateway"
RUN dotnet build "FoodRecognitionGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FoodRecognitionGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FoodRecognitionGateway.dll"]
