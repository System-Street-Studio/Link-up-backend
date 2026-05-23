# 1. Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# සියලුම .csproj ගොනු වෙන වෙනම copy කරගෙන Dependencies restore කරගන්නවා
# මේකෙන් Docker caching ධාරිතාවය උපරිම වෙනවා (Code වෙනස් කරත් restore එක ඉක්මන් වෙනවා)
COPY ["WebAPI/WebAPI.csproj", "WebAPI/"]
COPY ["Infrasturcture/Infrasturcture.csproj", "Infrasturcture/"]
COPY ["Core/Core.csproj", "Core/"]
RUN dotnet restore "WebAPI/WebAPI.csproj"

# මුළු Source code එකම Docker container එක ඇතුලට copy කරනවා
COPY . .

# WebAPI folder එකට ගිහින් project එක publish කරනවා
WORKDIR "/src/WebAPI"
RUN dotnet publish "WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 2. Runtime Stage (සැහැල්ලු ධාවන පරිසරය)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000

ENTRYPOINT ["dotnet", "WebAPI.dll"]