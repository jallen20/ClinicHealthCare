using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ClinicHealthcareSystem.DAL
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class DbConnection
    {
        private const string ConString = "server=160.10.25.16;port=3306;uid=cs3230f18g;pwd=MqMQdGgMg1ZRhHIJ;database=cs3230f18g;SslMode=None;";

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <returns></returns>
        public static MySqlConnection GetConnection()
        {
            var conn = new MySqlConnection(ConString);
            return conn;
        }
    }
}
