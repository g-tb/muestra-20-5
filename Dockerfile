FROM mcr.microsoft.com/azure-sql-edge:latest

ENV ACCEPT_EULA=Y
ENV MSSQL_PID=Developer

EXPOSE 1433

HEALTHCHECK --interval=10s --timeout=5s --start-period=30s --retries=12 \
    CMD /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$MSSQL_SA_PASSWORD" -Q "SELECT 1" || exit 1
