using ClinicHealthcareSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ClinicHealthcareSystem.DAL
{
    /// <summary>
    /// 
    /// </summary>
    public class NurseDAL
    {
        private UserDAL userDal;

        public NurseDAL()
        {
            this.userDal = new UserDAL();
        }

        public async Task<Nurse> GetNurse(string id)
        {
            Nurse nurse = null;
            var qry = "SELECT * FROM nurse WHERE userId = @id";
            var conn = DbConnection.GetConnection();
            await conn.OpenAsync();
            using (var command = new MySqlCommand(qry, conn))
            {
                command.Parameters.Add("@id", (DbType) MySqlDbType.VarChar).Value = id;
              
                using (var reader = await command.ExecuteReaderAsync())
                {
                    var nurseIdOrd = reader.GetOrdinal("nurseId");
                    var idOrd = reader.GetOrdinal("userId");
                   
                    while (await reader.ReadAsync())
                    {
                        var nurseId = !reader.IsDBNull(nurseIdOrd) ? reader.GetInt32(nurseIdOrd) : 1;
                        var _id = reader[idOrd] == DBNull.Value ? null : reader.GetString(idOrd);
                        
                        var user = await this.userDal.GetUser(_id);
                        nurse = new Nurse(user.Id,user.FirstName, user.LastName, user.SSN, user.DOB, user.Sex, user.Address, user.Phone)
                        {
                            NurseId = nurseId
                        };
                    }
                }
            }
            conn.Close();
            return nurse;
        }

        public async Task<Nurse> GetNurseByNurseId(int id)
        {
            Nurse nurse = null;
            var qry = "SELECT * FROM nurse WHERE nurseId = @id";
            var conn = DbConnection.GetConnection();
            await conn.OpenAsync();
            using (var command = new MySqlCommand(qry, conn))
            {
                command.Parameters.Add("@id", (DbType) MySqlDbType.VarChar).Value = id;
              
                using (var reader = await command.ExecuteReaderAsync())
                {
                    var nurseIdOrd = reader.GetOrdinal("nurseId");
                    var idOrd = reader.GetOrdinal("userId");
                    
                    while (await reader.ReadAsync())
                    {
                        var nurseId = !reader.IsDBNull(nurseIdOrd) ? reader.GetInt32(nurseIdOrd) : 1;
                        var _id = reader[idOrd] == DBNull.Value ? null : reader.GetString(idOrd);
                        var user = await this.userDal.GetUser(_id);
                        nurse = new Nurse(user.Id,user.FirstName, user.LastName, user.SSN, user.DOB, user.Sex, user.Address, user.Phone)
                        {
                            NurseId = nurseId
                        };
                    }
                }
            }
            conn.Close();
            return nurse;
        }

        public async Task<IList<Nurse>> GetAllNurses()
        {
            var nurses = new List<Nurse>();
            var qry = "select * from nurse";
            var conn = DbConnection.GetConnection();
           await conn.OpenAsync();
            using (var command = new MySqlCommand(qry, conn))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    var nIdOrdinal = reader.GetOrdinal("nurseId");
                    var idOrd = reader.GetOrdinal("userId");
                    
                    while (await reader.ReadAsync())
                    {
                        var nurseId = !reader.IsDBNull(nIdOrdinal) ? reader.GetInt32(nIdOrdinal) : 1;
                        var _id = reader[idOrd] == DBNull.Value ? null : reader.GetString(idOrd);
                       
                        var user = await this.userDal.GetUser(_id);
                        nurses.Add(new Nurse(user.Id, user.FirstName, user.LastName, user.SSN, user.DOB, user.Sex,
                            user.Address, user.Phone)
                        {
                            NurseId = nurseId
                        });
                    }
                }
            }
            conn.Close();
            return nurses;
        }

    }
}
