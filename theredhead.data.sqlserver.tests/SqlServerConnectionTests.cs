using Microsoft.Data.SqlClient;
using System.Data;

namespace theredhead.data.sqlserver.tests;

public class SqlServerConnectionTests
{
    private const string ConnectionString = "Data source=localhost; Initial catalog=test; User=sa; Password=PPC750cx; MultipleActiveResultSets=True; TrustServerCertificate=True";

    [SetUp]
    public void Setup()
    {
    }
    
    [Test]
    public void CanCreateAndOpenConnection()
    {
        using var connection = new SqlConnection(ConnectionString);
        Assert.That(connection.State, Is.EqualTo(ConnectionState.Closed));
        connection.Open();
        Assert.That(connection.State, Is.EqualTo(ConnectionState.Open));
        connection.Close();
        Assert.That(connection.State, Is.EqualTo(ConnectionState.Closed));
    }

    [Test]
    public void CreateCommandAssignsConnectionAndCommandTextAndParameters()
    {
        using var connection = new SqlConnection(ConnectionString);
        connection.Open();

        var command = connection.CreateCommand(
            "SELECT * FROM INFORMNATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'base table'",
            new CommandArguments("@",
                new Dictionary<string, object>() {
                    { "answer", 42 }
                }
            )
        );

        Assert.That(command.Connection, Is.SameAs(connection), "Connection is different");
        Assert.That(command.CommandText, Is.EqualTo("SELECT * FROM INFORMNATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'base table'"), "CommandText is different");
        Assert.That(command.Parameters.Count, Is.EqualTo(1), "Wrong number of parameters");
        if(command.Parameters[0] is IDbDataParameter parameter)
        {
            Assert.That(parameter.ParameterName, Is.EqualTo("@answer"));
            Assert.That(parameter.Value, Is.EqualTo(42));
        } else {
            Assert.Fail("Parameter is not of IDbDataParameterType");
        }
        connection.Close();
    }

    [Test]
    public void GetCommandFactoryCreatesSqliteCommandFactory()
    {
        var connection = new SqlConnection(ConnectionString);
        var factory = connection.GetCommandFactory();
        Assert.That(factory, Is.InstanceOf<SqlServerCommandFactory>());
    }

    [Test]
    public void CanSelectSomeData() {
        var connection = new SqlConnection(ConnectionString);
        connection.Open();
        var table = connection.ExecuteDataTable("SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_CATALOG=DB_NAME()");
        connection.Close();
        Assert.That(table, Is.InstanceOf<DataTable>());
    }

    [Test]
    public void CanSelectSomeDataThroughFactory() {
        var connection = new SqlConnection(ConnectionString);
        connection.Open();
        var command = connection.GetCommandFactory().CreateSelectCommand("Hero", 
            new Dictionary<string, object>()
            {
                {"name", "steve"}
            }
        );
        var table = command.ExecuteDataTable();
        connection.Close();
        Assert.That(table, Is.InstanceOf<DataTable>());
    }
}
