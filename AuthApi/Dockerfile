FROM mcr.microsoft.com/dotnet/core/sdk:2.2.402-stretch as build
WORKDIR /code
COPY . .
RUN dotnet publish -o output -c Release

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2.7-stretch-slim as runtime
RUN groupadd -g 999 appuser && \
    useradd -r -u 999 -g appuser appuser

WORKDIR /app
COPY --from=build /code/output .

EXPOSE 80
ENTRYPOINT [ "dotnet", "AuthApi.dll" ]