FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
LABEL AUTHOR="LUCIO FLORES"
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY ./wizeline/*.csproj ./wizeline/
COPY ./wizeline.tests/*.csproj ./wizeline.tests/

RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .

ENTRYPOINT [ "dotnet", "wizeline.dll"]
