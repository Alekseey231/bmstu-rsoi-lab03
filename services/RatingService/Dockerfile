FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
EXPOSE 8050
WORKDIR /app

ENV TZ=Europe/Moscow
RUN ln -snf /usr/share/zoneinfo/Europe/Moscow /etc/localtime && echo Europe/Moscow > /etc/timezone

COPY RatingServicePublish/ .

ENTRYPOINT ["dotnet", "RatingService.Server.dll"]