#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine3.15 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine3.15 AS build
WORKDIR /src
COPY ["src/AM.OneTimeMessages.Web/AM.OneTimeMessages.Web.csproj", "src/AM.OneTimeMessages.Web/"]
COPY ["src/AM.OneTimeMessages.Core/AM.OneTimeMessages.Core.csproj", "src/AM.OneTimeMessages.Core/"]
RUN dotnet restore "src/AM.OneTimeMessages.Web/AM.OneTimeMessages.Web.csproj"
COPY . .
WORKDIR "/src/src/AM.OneTimeMessages.Web"
RUN dotnet build "AM.OneTimeMessages.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AM.OneTimeMessages.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AM.OneTimeMessages.Web.dll"]
