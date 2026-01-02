using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataAccessLibrary
{
    public class SqliteDataAccess
    {

        public List<T> LoadData<T, U>(string sqlStatements, U parameters, string connectionString)
        {
            using (IDbConnection connection = new SqliteConnection(connectionString))
            {
                List<T> rows = connection.Query<T>(sqlStatements, parameters).ToList();

                return rows;
            }
        }
        public void SaveData<T>(string sqlStatements, T parameters, string connectionString)
        {
            using (IDbConnection connection = new SqliteConnection(connectionString))
            {
                connection.Execute(sqlStatements, parameters);
            }
        }
    }
}
