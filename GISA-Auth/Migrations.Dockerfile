FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src
COPY ["GISA-Auth.csproj", "."]
COPY Setup.sh Setup.sh

RUN dotnet tool install --global dotnet-ef

RUN dotnet restore "GISA-Auth.csproj"
COPY . .
WORKDIR "/src/."

RUN chmod +x ./Setup.sh
CMD /bin/bash ./Setup.sh