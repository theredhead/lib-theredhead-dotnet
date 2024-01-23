using System;
using System.Data;
using System.Reflection;

namespace theredhead.data.sql;


public static class IDbConnectionSqlExtensions
{
    public static ISqlCommandFactory GetCommandFactory(this IDbConnection connection)
    {
        var typeName = connection.GetType().FullName;
        switch (typeName)   
        {
            default:
                throw new Exception($"Unsupported connection type: {typeName}");
        }
    }
}
