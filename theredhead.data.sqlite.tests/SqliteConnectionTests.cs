using Microsoft.Data.Sqlite;
using System.Data;
using theredhead.data.sql;
using theredhead.data.sqlite;

namespace theredhead.data.sqlite.tests;

public class SqliteConnectionTests
{
    [SetUp]
    public void Setup()
    {
    }
    [Test]
    public void CanCreateAndOpenConnection()
    {
        using var connection = new SqliteConnection("Datasource=:memory:");
        Assert.That(connection.State, Is.EqualTo(ConnectionState.Closed));
        connection.Open();
        Assert.That(connection.State, Is.EqualTo(ConnectionState.Open));
        connection.Close();
        Assert.That(connection.State, Is.EqualTo(ConnectionState.Closed));
    }

    [Test]
    public void CreateCommandAssignsConnectionAndCommandTextAndParameters()
    {
        using var connection = new SqliteConnection("Datasource=:memory:");
        var command = connection.CreateCommand(
            "select * from sqlite_master where type = 'table'",
            new()
            {
                { "answer", 42 }
            }
        );

        Assert.That(command.Connection, Is.SameAs(connection), "Connection is different");
        Assert.That(command.CommandText, Is.EqualTo("select * from sqlite_master where type = 'table'"), "CommandText is different");
        Assert.That(command.Parameters.Count, Is.EqualTo(1), "Wrong number of parameters");
        if(command.Parameters[0] is IDbDataParameter parameter)
        {
            Assert.That(parameter.ParameterName, Is.EqualTo("answer"));
            Assert.That(parameter.Value, Is.EqualTo(42));
        } else {
            Assert.Fail("Parameter is not of IDbDataParameterType");
        }
    }

    [Test]
    public void GetCommandFactoryCreatesSqliteCommandFactory()
    {
        var connection = new SqliteConnection("Datasource=:memory:");
        var factory = connection.GetCommandFactory();
        Assert.That(factory, Is.InstanceOf<SqliteCommandFactory>());
    }
}
