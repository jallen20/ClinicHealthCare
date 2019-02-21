using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using ClinicHealthcareSystem.Models;
using MySql.Data.MySqlClient;

namespace ClinicHealthcareSystem.DAL
{
    public class AdminDAL : Encryptor
    {

        /// <summary>
        /// Gets the admin by user identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<Administrator> GetAdminByUserId(string id)
        {
            Administrator admin = null;
            var qry = "SELECT * FROM admin WHERE id = @id";
            var conn = DbConnection.GetConnection();
            await conn.OpenAsync();
            using (var command = new MySqlCommand(qry, conn))
            {
                command.Parameters.Add("@id", (DbType)MySqlDbType.VarChar).Value = id;

                using (var reader = await command.ExecuteReaderAsync())
                {
                    var adminIdOrd = reader.GetOrdinal("adminId");
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
                        var adminId = !reader.IsDBNull(adminIdOrd) ? reader.GetInt32(adminIdOrd) : 1;
                        var _id = reader[idOrd] == DBNull.Value ? null : reader.GetString(idOrd);
                        var _fname = reader[fnameOrd] == DBNull.Value ? null : reader.GetString(fnameOrd);
                        var _lname = reader[lnameOrd] == DBNull.Value ? null : reader.GetString(lnameOrd);
                        var _ssn = reader[ssnOrd] == DBNull.Value ? null : reader.GetString(ssnOrd);
                        DateTime _dob = reader.GetDateTime(dobOrd);
                        var _sex = reader[sexOrd] == DBNull.Value ? null : reader.GetString(sexOrd);
                        var _address = reader[addressOrd] == DBNull.Value ? null : reader.GetString(addressOrd);
                        var _phone = reader[phoneOrd] == DBNull.Value ? null : reader.GetString(phoneOrd);

                        admin = new Administrator(_id, _fname, _lname, _ssn, _dob, _sex, _address, _phone)
                        {
                            AdministratorId = adminId
                        };
                    }
                }
            }
            conn.Close();
            return admin;
        }

        public async Task CreateNurse(string uId, string pWord, User nurse)
        {
            var qry =
                $"insert into user values(@uid, @fname, @lname, @ssn, @dob, @sex, @address, @phone); insert into logins values(@uid, AES_ENCRYPT(@pword, '{Key}'));  insert into nurse values(null, @uid);";
            var conn = DbConnection.GetConnection();
            await conn.OpenAsync();
            using (var command = new MySqlCommand(qry, conn))
            {
                command.Parameters.AddWithValue("@uid", uId);
                command.Parameters.AddWithValue("@pword", pWord);
                command.Parameters.AddWithValue("@fname", nurse.FirstName);
                command.Parameters.AddWithValue("@lname", nurse.LastName);
                command.Parameters.AddWithValue("@ssn", nurse.SSN);
                command.Parameters.AddWithValue("@dob", nurse.DOB);
                command.Parameters.AddWithValue("@sex", nurse.Sex);
                command.Parameters.AddWithValue("@address", nurse.Address);
                command.Parameters.AddWithValue("@phone", nurse.Phone);

                await command.ExecuteNonQueryAsync();
            }
            conn.Close();
        }

        /// <summary>
        /// Runs the query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public async Task<DataTable> RunQuery(string query)
        {
            var task = await Task.Run(() =>
            {
                var conn = DbConnection.GetConnection();
                conn.CreateCommand();
                var command = new MySqlCommand(query, conn);
                var adapter = new MySqlDataAdapter() { SelectCommand = command };
                var table = new DataTable();
                adapter.Fill(table);
                return table;
            });
            return task;
        }

        /// <summary>
        /// Runs the non query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public async Task RunNonQuery(string query)
        {
            var conn = DbConnection.GetConnection();
            await conn.OpenAsync();
            using (var command = new MySqlCommand(query, conn))
            {
                await command.ExecuteNonQueryAsync();
            }
            conn.Close();
        }
    }
}
