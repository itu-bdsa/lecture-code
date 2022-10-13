# Notes

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=<YourStrong@Passw0rd>" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest

CONNECTION_STRING="Server=localhost;Database=Comics;User Id=sa;Password=<YourStrong@Passw0rd>;Trusted_Connection=False;Encrypt=False"

dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:Comics" "$CONNECTION_STRING"
```
