#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ClasesGeneradas/hercules.csproj", "./ClasesGeneradas/"]
COPY ["Hercules.CommonsEDMA.Journals/Hercules.CommonsEDMA.Journals.csproj", "./Hercules.CommonsEDMA.Journals/"]
RUN dotnet restore "./Hercules.CommonsEDMA.Journals/Hercules.CommonsEDMA.Journals.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Hercules.CommonsEDMA.Journals/Hercules.CommonsEDMA.Journals.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Hercules.CommonsEDMA.Journals/Hercules.CommonsEDMA.Journals.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hercules.CommonsEDMA.Journals.dll"]
