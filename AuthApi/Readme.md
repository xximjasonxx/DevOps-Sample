# Movie App Auth API

### Setup
#### Database
Run the following Docker command to setup the local database
`docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=Password01!' -p 1433:1433 -d mcr.microsoft.com/mssql/server:2017-CU8-ubuntu`

This will create a running SQL Server instance we can connect to locally

#### Auth
The `JwtKey` values in the Release pipeline are just Base64 encoded GUIDs