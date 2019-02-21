using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ClinicHealthcareSystem.DAL
{
   public class LoginDAL : Encryptor
   {
       public async Task<bool> IsValidLogin(string id, string password)
       {
           var logins = new string[2];
           var conn = DbConnection.GetConnection();
           var qry = $"SELECT * FROM logins WHERE id = @id and password = AES_ENCRYPT(@password, '{Key}')";
           await conn.OpenAsync();
           using (var command = new MySqlCommand(qry, conn))
           {
               command.Parameters.Add("@id",(DbType) MySqlDbType.VarChar).Value = id;
               command.Parameters.Add("@password",(DbType) MySqlDbType.VarChar).Value = password;

               using(var reader = await command.ExecuteReaderAsync())
               {
                   var idOrd = reader.GetOrdinal("id");
                   var passwordOrd = reader.GetOrdinal("password");
                   while (await reader.ReadAsync())
                   {
                       var _id = reader[idOrd] == DBNull.Value ? null : reader.GetString(idOrd);
                       var _password = reader[passwordOrd] == DBNull.Value ? null : reader.GetString(passwordOrd);
                       logins[0] = _id;
                       logins[1] = _password;
                   }

                   if (logins[0] is null || logins[1] is null) throw new InvalidCredentialException("Invalid ID or Password");
               }
           }
           conn.Close();
           return true;
       }
   }
}
