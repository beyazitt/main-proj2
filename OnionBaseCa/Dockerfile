#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
#ENV ConnectionStrings__DefaultConnection ""

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . ./
WORKDIR "/src/Presentation/OnionBase.Presentation"
RUN dotnet build "OnionBase.Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OnionBase.Presentation.csproj" -c Release -o /app/publish /p:UseAppHost=true

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OnionBase.Presentation.dll"]
