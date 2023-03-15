FROM mcr.microsofrt.com/dotnet/aspnet:7.0-alpine as basic
ENV NG_CLI_ANALYTICS=false
RUN dotnet tool install --global dotnet-ef && apk add --update nodejs nodejs-npm && npm install -g @angular/cli
