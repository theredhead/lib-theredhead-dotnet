namespace theredhead.data.mysql.tests;

using MySql.Data.MySqlClient;
using System.Data;

public class MySqlConnectionTests
{
    const string ConnectionString = "Server=localhost;Database=database;Uid=user;Pwd=password;";

    [SetUp]
    public void Setup()
    {
    }
    
    [Test]
    public void CanCreateAndOpenConnection()
    {
        using var connection = new MySqlConnection(ConnectionString);
        Assert.That(connection.State, Is.EqualTo(ConnectionState.Closed));
        connection.Open();
        Assert.That(connection.State, Is.EqualTo(ConnectionState.Open));
        connection.Close();
        Assert.That(connection.State, Is.EqualTo(ConnectionState.Closed));
    }

    [Test]
    public void CreateCommandAssignsConnectionAndCommandTextAndParameters()
    {
        using var connection = new MySqlConnection(ConnectionString);
        var command = connection.CreateCommand(
            "select * from sqlite_master where type = 'table'",
            new CommandArguments("@",
                new Dictionary<string, object>() {
                    { "answer", 42 }
                }
            )
        );

        Assert.That(command.Connection, Is.SameAs(connection), "Connection is different");
        Assert.That(command.CommandText, Is.EqualTo("select * from sqlite_master where type = 'table'"), "CommandText is different");
        Assert.That(command.Parameters.Count, Is.EqualTo(1), "Wrong number of parameters");
        if(command.Parameters[0] is IDbDataParameter parameter)
        {
            Assert.That(parameter.ParameterName, Is.EqualTo("@answer"));
            Assert.That(parameter.Value, Is.EqualTo(42));
        } else {
            Assert.Fail("Parameter is not of IDbDataParameterType");
        }
    }

    [Test]
    public void GetCommandFactoryCreatesSqliteCommandFactory()
    {
        var connection = new MySqlConnection(ConnectionString);
        var factory = connection.GetCommandFactory();
        Assert.That(factory, Is.InstanceOf<MySqlCommandFactory>());
    }

    [Test]
    public void CanSelectSomeData() {
        var connection = new MySqlConnection(ConnectionString);
        connection.Open();
        var table = connection.ExecuteDataTable("SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA=DATABASE()");
        connection.Close();
        Assert.That(table, Is.InstanceOf<DataTable>());
    }
    [Test]
    public void CanSelectSomeDataThroughFactory() {
        var connection = new MySqlConnection(ConnectionString);
        connection.Open();
        var command = connection.GetCommandFactory().CreateSelectCommand("TEST", null, ["TABLE_NAME"]);
        var table = command.ExecuteDataTable();
        connection.Close();
        Assert.That(table, Is.InstanceOf<DataTable>());
    }
}
