using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicHealthcareSystem.Models;
using MySql.Data.MySqlClient;

namespace ClinicHealthcareSystem.DAL
{
    public class UserDAL
    {
        public async Task<User> GetUser(string id)
        {
            User user = null;
            var qry = "SELECT * FROM user WHERE id = @id";
            var conn = DbConnection.GetConnection();
            await conn.OpenAsync();
            using (var command = new MySqlCommand(qry, conn))
            {
                command.Parameters.Add("@id", (DbType) MySqlDbType.VarChar).Value = id;
              
                using (var reader = await command.ExecuteReaderAsync())
                {
                    var idOrd = reader.GetOrdinal("id");
                    var fnameOrd = reader.GetOrdinal("firstname");
                    var lnameOrd = reader.GetOrdinal("lastname");
                    var ssnOrd = reader.GetOrdinal("ssn");
                    var dobOrd = reader.GetOrdinal("dob");
                    var sexOrd = reader.GetOrdinal("sex");
                    var addressOrd = reader.GetOrdinal("address");
                    var phoneOrd = reader.GetOrdinal("phone");
                    while (await reader.ReadAsync())
                    {
                        var _id = reader[idOrd] == DBNull.Value ? null : reader.GetString(idOrd);
                        var _fname = reader[fnameOrd] == DBNull.Value ? null : reader.GetString(fnameOrd);
                        var _lname = reader[lnameOrd] == DBNull.Value ? null : reader.GetString(lnameOrd);
                        var _ssn = reader[ssnOrd] == DBNull.Value ? null : reader.GetString(ssnOrd);
                        DateTime _dob = reader.GetDateTime(dobOrd);
                        var _sex = reader[sexOrd] == DBNull.Value ? null : reader.GetString(sexOrd);
                        var _address = reader[addressOrd] == DBNull.Value ? null : reader.GetString(addressOrd);
                        var _phone = reader[phoneOrd] == DBNull.Value ? null : reader.GetString(phoneOrd);

                        user = new User(_id, _fname, _lname, _ssn, _dob, _sex, _address, _phone);
                    }
                }
            }
            conn.Close();
            return user;
        }

    }
}
