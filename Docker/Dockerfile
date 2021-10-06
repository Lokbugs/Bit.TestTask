FROM docker-gorgona.spr.sd.itmh.ru/mcr.microsoft.com/dotnet/sdk:5.0 AS build

ARG SERVICE=CameraTiltAngle

WORKDIR /$SERVICE
COPY $SERVICE/ .

RUN dotnet build "$SERVICE.csproj" -c Release -o /app/build
RUN dotnet publish "$SERVICE.csproj" -c Release -o /app/publish

WORKDIR /app/publish

ENTRYPOINT ["dotnet", "CameraTiltAngle.dll"]