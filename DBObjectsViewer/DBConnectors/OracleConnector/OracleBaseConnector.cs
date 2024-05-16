using System.Data.OracleClient;


namespace DBObjectsViewer.DBConnectors.OracleConnector
{
    abstract public class BaseOracleDBConnector : OracleAdapters
    {
        private string Server { get; }
        private string Port { get; }
        private string Database { get; }
        private string Login { get; }
        private string Password { get; }
        private string ConnectionString { get; }
        private OracleConnection Connection { get; }
        private OracleTransaction Transaction { get; set; }

        public BaseOracleDBConnector(string server, string port, string database, string login, string password)
        {
            Server = server;
            Port = port;
            Database = database;
            Login = login;
            Password = password;
            // Connection string format: User Id=[username];Password=[password];Data Source=[hostname]:[port]/[DB service name];
            ConnectionString = $@"User ID={Login};Password={Password};Data Source={Server}:{Port}/{Database};";
            Connection = new OracleConnection(ConnectionString);
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

        protected OracleConnection ReturnConnection()
        {
            return Connection;
        }

        public void StartTransaction()
        {
            Transaction = Connection.BeginTransaction();
        }

        protected OracleTransaction ReturnTransaction()
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
    }
}
