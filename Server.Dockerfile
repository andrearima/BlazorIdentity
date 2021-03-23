FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine3.13-amd64 AS build-env

WORKDIR /build 

COPY . ./

RUN dotnet restore --disable-parallel ./Arima/Server/Arima.Server.csproj -s "https://api.nuget.org/v3/index.json;https://www.myget.org/F/nuget;http://nexus.maximasistemas.com.br/repository/nuget-hosted/"

RUN dotnet publish ./Arima/Server/Arima.Server.csproj -c Release -o /saida

FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine3.13-amd64 as BlazorServer

############## TIMEZONE, CULTURE e LOCALES ###################
RUN apk add tzdata && apk add --no-cache icu-libs
RUN cp /usr/share/zoneinfo/America/Sao_Paulo /etc/localtime
RUN echo "America/Sao_Paulo" > /etc/timezone
RUN apk del tzdata
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV COMPlus_ThreadPool_ForceMinWorkerThreads=300

RUN apk --no-cache add ca-certificates wget
RUN wget -q -O /etc/apk/keys/sgerrand.rsa.pub https://alpine-pkgs.sgerrand.com/sgerrand.rsa.pub
RUN wget https://github.com/sgerrand/alpine-pkg-glibc/releases/download/2.30-r0/glibc-2.30-r0.apk
RUN wget https://github.com/sgerrand/alpine-pkg-glibc/releases/download/2.30-r0/glibc-bin-2.30-r0.apk
RUN wget https://github.com/sgerrand/alpine-pkg-glibc/releases/download/2.30-r0/glibc-i18n-2.30-r0.apk
RUN apk add glibc-2.30-r0.apk && apk add glibc-bin-2.30-r0.apk && apk add glibc-i18n-2.30-r0.apk 
RUN /usr/glibc-compat/bin/localedef -i pt_BR -f UTF-8 pt_BR.UTF-8
ENV LANG=pt_BR.UTF-8
ENV LANGUAGE=pt_BR.UTF-8
ENV LC_ALL=pt_BR.UTF-8
##############################################################


WORKDIR /app/maxima/

COPY --from=build-env /saida . 

ENV ASPNETCORE_URLS=http://+:80

EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "Arima.Server.dll", "--environment=Docker"]