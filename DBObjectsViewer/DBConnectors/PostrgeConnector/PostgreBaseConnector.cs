using Npgsql;


namespace DBObjectsViewer
{
    abstract public class BasePostgreDBConnector : PostgreAdapters
    {
        private string Server { get; }
        private string Port { get; }
        private string Database { get; }
        private string Login { get; }
        private string Password { get; }
        private string Schema { get; }
        private string ConnectionString { get; }
        private NpgsqlConnection Connection { get; }
        private NpgsqlTransaction Transaction { get; set; }

        public BasePostgreDBConnector(string server, string port, string database, string login, string password, string schema)
        {
            Server = server;
            Port = port;
            Database = database;
            Login = login;
            Password = password;
            Schema = schema;
            ConnectionString = $@"Server={Server};Port={Port};Database={Database};User ID={Login};Password={Password};";
            Connection = new NpgsqlConnection(ConnectionString);
            Transaction = null;
        }

        public void OpenConnection()
        {
            Connection.Open();
        }

        public void CloseConnection()
        {
            Connection.Close();
        }

        protected NpgsqlConnection ReturnConnection()
        {
            return Connection;
        }

        public void StartTransaction()
        {
            Transaction = Connection.BeginTransaction();
        }

        protected NpgsqlTransaction ReturnTransaction()
        {
            return Transaction;
        }

        public void CommitTransaction()
        {
            Transaction.Commit();
        }

        public void RollbackTransaction()
        {
            Transaction.Rollback();
        }

        public string ReturnDatabaseName()
        {
            return Database;
        }

        public string ReturnSchemaName()
        {
            return Schema;
        }
    }
}
