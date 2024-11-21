
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder
WORKDIR /app

COPY agrisynth-backend/*.csproj ./agrisynth-backend/
RUN dotnet restore ./agrisynth-backend/agrisynth-backend.csproj

COPY agrisynth-backend/ ./agrisynth-backend/
WORKDIR /app/agrisynth-backend
RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runner
WORKDIR /app
COPY --from=builder /app/agrisynth-backend/out ./
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 80
EXPOSE 443
EXPOSE 8080

ENTRYPOINT ["dotnet", "agrisynth-backend.dll"]
