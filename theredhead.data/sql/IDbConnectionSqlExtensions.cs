using System;
using System.Data;
using System.Reflection;

namespace theredhead.data.sql;


public static class IDbConnectionSqlExtensions
{
    public static ISqlCommandFactory GetCommandFactory(this IDbConnection connection)
    {
        return switch (connection.GetType().FullName)
        {
            default:
                throw new Exception("Unsupported connection type!");
                break;
        }
        throw new Exception(connection.GetType().FullName);
    }
}
