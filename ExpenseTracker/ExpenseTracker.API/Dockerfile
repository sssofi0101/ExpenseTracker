FROM mcr.microsoft.com/dotnet/sdk:8.0 as build

WORKDIR /src

COPY ["ExpenseTracker.API/ExpenseTracker.API.csproj","ExpenseTracker.API/"]

RUN dotnet restore "ExpenseTracker.API/ExpenseTracker.API.csproj"

COPY . .

WORKDIR /src/ExpenseTracker.API

RUN dotnet publish "ExpenseTracker.API.csproj" -c Release -o /app/publish /p:UseAppHost=false




FROM mcr.microsoft.com/dotnet/aspnet:8.0 as final
WORKDIR /app

USER app

COPY --from=build /app/publish .

ENTRYPOINT [ "dotnet", "ExpenseTracker.API.dll" ]