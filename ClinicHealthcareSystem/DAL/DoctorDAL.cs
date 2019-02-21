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
    public class DoctorDAL
    {
        public async Task<IList<Doctor>> GetDoctorsByName(string fname, string lname)
        {
            var doctors = new List<Doctor>();
            var qry = "SELECT * FROM doctor WHERE firstname = @fname or lastname = @lname";
            var conn = DbConnection.GetConnection();
            await conn.OpenAsync();
            using (var command = new MySqlCommand(qry, conn))
            {
                command.Parameters.Add("@fname", (DbType) MySqlDbType.VarChar).Value = fname;
                command.Parameters.Add("@lname", (DbType) MySqlDbType.VarChar).Value = lname;
              
                using (var reader = await command.ExecuteReaderAsync())
                {
                    var dIdOrdinal = reader.GetOrdinal("doctorId");
                    var idOrd = reader.GetOrdinal("id");
                    var fnameOrd = reader.GetOrdinal("firstname");
                    var lnameOrd = reader.GetOrdinal("lastname");
                    var ssnOrd = reader.GetOrdinal("ssn");
                    var dobOrd = reader.GetOrdinal("dob");
                    var sexOrd = reader.GetOrdinal("sex");
                    var addressOrd = reader.GetOrdinal("address");
                    var phoneOrd = reader.GetOrdinal("phone");
                    while (reader.Read())
                    {
                        var dId = !reader.IsDBNull(dIdOrdinal) ? reader.GetInt32(dIdOrdinal) : 1;
                        var _id = reader[idOrd] == DBNull.Value ? null : reader.GetString(idOrd);
                        var _fname = reader[fnameOrd] == DBNull.Value ? null : reader.GetString(fnameOrd);
                        var _lname = reader[lnameOrd] == DBNull.Value ? null : reader.GetString(lnameOrd);
                        var _ssn = reader[ssnOrd] == DBNull.Value ? null : reader.GetString(ssnOrd);
                        DateTime _dob = reader.GetDateTime(dobOrd);
                        var _sex = reader[sexOrd] == DBNull.Value ? null : reader.GetString(sexOrd);
                        var _address = reader[addressOrd] == DBNull.Value ? null : reader.GetString(addressOrd);
                        var _phone = reader[phoneOrd] == DBNull.Value ? null : reader.GetString(phoneOrd);
                         
                        doctors.Add(new Doctor(_id,_fname, _lname, _ssn, _dob, _sex, _address, _phone)
                        {
                            DoctorId = dId
                        });
                    }
                }
            }
            conn.Close();
            return doctors;
        }

        public async Task<Doctor> GetDoctorByDoctorId(int id)
        {
            Doctor doctor = null;
            var qry = "SELECT * FROM doctor WHERE doctorId = @id";
            var conn = DbConnection.GetConnection();
            await conn.OpenAsync();
            using (var command = new MySqlCommand(qry, conn))
            {
                command.Parameters.Add("@id", (DbType)MySqlDbType.VarChar).Value = id;

                using (var reader = await command.ExecuteReaderAsync())
                {
                    var doctorIdOrd = reader.GetOrdinal("doctorId");
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
                        var nurseId = !reader.IsDBNull(doctorIdOrd) ? reader.GetInt32(doctorIdOrd) : 1;
                        var _id = reader[idOrd] == DBNull.Value ? null : reader.GetString(idOrd);
                        var _fname = reader[fnameOrd] == DBNull.Value ? null : reader.GetString(fnameOrd);
                        var _lname = reader[lnameOrd] == DBNull.Value ? null : reader.GetString(lnameOrd);
                        var _ssn = reader[ssnOrd] == DBNull.Value ? null : reader.GetString(ssnOrd);
                        DateTime _dob = reader.GetDateTime(dobOrd);
                        var _sex = reader[sexOrd] == DBNull.Value ? null : reader.GetString(sexOrd);
                        var _address = reader[addressOrd] == DBNull.Value ? null : reader.GetString(addressOrd);
                        var _phone = reader[phoneOrd] == DBNull.Value ? null : reader.GetString(phoneOrd);

                        doctor = new Doctor(_id, _fname, _lname, _ssn, _dob, _sex, _address, _phone)
                        {
                            DoctorId = nurseId
                        };
                    }
                }
            }
            conn.Close();
            return doctor;
        }

        public async Task<IList<Doctor>> GetAllDoctors()
        {
            var doctors = new List<Doctor>();
            var qry = "select * from doctor";
            var conn = DbConnection.GetConnection();
            await conn.OpenAsync();
            using (var command = new MySqlCommand(qry, conn))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    var dIdOrdinal = reader.GetOrdinal("doctorId");
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
                        var dId = !reader.IsDBNull(dIdOrdinal) ? reader.GetInt32(dIdOrdinal) : 1;
                        var _id = reader[idOrd] == DBNull.Value ? null : reader.GetString(idOrd);
                        var _fname = reader[fnameOrd] == DBNull.Value ? null : reader.GetString(fnameOrd);
                        var _lname = reader[lnameOrd] == DBNull.Value ? null : reader.GetString(lnameOrd);
                        var _ssn = reader[ssnOrd] == DBNull.Value ? null : reader.GetString(ssnOrd);
                        DateTime _dob = reader.GetDateTime(dobOrd);
                        var _sex = reader[sexOrd] == DBNull.Value ? null : reader.GetString(sexOrd);
                        var _address = reader[addressOrd] == DBNull.Value ? null : reader.GetString(addressOrd);
                        var _phone = reader[phoneOrd] == DBNull.Value ? null : reader.GetString(phoneOrd);


                        doctors.Add(new Doctor(_id, _fname, _lname, _ssn, _dob, _sex, _address, _phone)
                        {
                            DoctorId = dId
                        });
                    }
                }
            }
            conn.Close();
            return doctors;
        }
    }
}
