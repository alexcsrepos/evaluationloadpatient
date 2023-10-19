using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Evaluation_LoadPatient.Cache
{
    internal static class DefaultConnectionStringCache
    {
        private static string ConnectionString { get; set; }

        public static void SetConnectionString(string server, string dataBase)
        {
            var defaultConnectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            var connectionStringBuilder = new SqlConnectionStringBuilder(defaultConnectionString);
            connectionStringBuilder.InitialCatalog = dataBase;
            connectionStringBuilder.DataSource = server;
            ConnectionString = connectionStringBuilder.ConnectionString;
        }

        public static string GetConnectionString()
        {
            if (string.IsNullOrEmpty(ConnectionString))
                throw new NullReferenceException("The connection string has not been set");
            
            return ConnectionString;
        }
    }
}
