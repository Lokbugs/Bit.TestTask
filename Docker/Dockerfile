FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

ARG SERVICE=CameraTiltAngle

WORKDIR /$SERVICE
COPY $SERVICE/ .

WORKDIR /$SERVICE .

RUN dotnet build "$SERVICE.csproj" -c Release -o /app/build
RUN dotnet publish "$SERVICE.csproj" -c Release -o /app/publish

WORKDIR /app/publish

ENTRYPOINT ["dotnet", "gc.provisioning_cs.dll"]