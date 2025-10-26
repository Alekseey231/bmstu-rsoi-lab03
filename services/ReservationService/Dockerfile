FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
EXPOSE 8070
WORKDIR /app

ENV TZ=Europe/Moscow
RUN ln -snf /usr/share/zoneinfo/Europe/Moscow /etc/localtime && echo Europe/Moscow > /etc/timezone

COPY ReservationServicePublish/ .

ENTRYPOINT ["dotnet", "ReservationService.Server.dll"]