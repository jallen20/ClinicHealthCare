using ClinicHealthcareSystem.Models;
using ClinicHealthcareSystem.Views;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace ClinicHealthcareSystem.DAL
{
    public class PatientDAL
    {
        //private DbConnection connection;

        public PatientDAL()
        {
            //connection = new DbConnection();
        }

        public async Task<List<Patient>> GetAllPatients()
        {
            var allPatients = new List<Patient>();

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                //Open the connection
                System.Text.EncodingProvider ppp;
                ppp = System.Text.CodePagesEncodingProvider.Instance;
                Encoding.RegisterProvider(ppp);
                await conn.OpenAsync();
                string query = "select id, fname, lname, ssn, DATE(dob), sex, address, city, state, zip, phone from patient";
                using (MySqlCommand comm = new MySqlCommand(query, conn))
                {
                    using (var reader = await comm.ExecuteReaderAsync())
                    {
                        int idOrdinal = reader.GetOrdinal("id");
                        int fnameOrdinal = reader.GetOrdinal("fname");
                        int lnameOrdinal = reader.GetOrdinal("lname");
                        int ssnOrdinal = reader.GetOrdinal("ssn");
                        int dobOrdinal = reader.GetOrdinal("DATE(dob)");
                        int sexOrdinal = reader.GetOrdinal("sex");
                        int addressOrdinal = reader.GetOrdinal("address");
                        int cityOrdinal = reader.GetOrdinal("city");
                        int stateOrdinal = reader.GetOrdinal("state");
                        int zipOrdinal = reader.GetOrdinal("zip");
                        int phoneOrdinal = reader.GetOrdinal("phone");

                        while (await reader.ReadAsync())
                        {
                            int id = !reader.IsDBNull(idOrdinal) ? reader.GetInt32(idOrdinal) : 1;
                            string fname = !reader.IsDBNull(fnameOrdinal) ? reader.GetString(fnameOrdinal) : null;
                            string lname = !reader.IsDBNull(lnameOrdinal) ? reader.GetString(lnameOrdinal) : null;
                            string ssn = !reader.IsDBNull(ssnOrdinal) ? reader.GetString(ssnOrdinal) : null;
                            DateTime dob = !reader.IsDBNull(dobOrdinal) ? reader.GetDateTime(dobOrdinal).Date : new DateTime(2001, 1, 1).Date;
                            string sex = !reader.IsDBNull(sexOrdinal) ? reader.GetString(sexOrdinal) : null;
                            string address = !reader.IsDBNull(addressOrdinal) ? reader.GetString(addressOrdinal) : null;
                            string city = !reader.IsDBNull(cityOrdinal) ? reader.GetString(cityOrdinal) : null;
                            string state = !reader.IsDBNull(stateOrdinal) ? reader.GetString(stateOrdinal) : null;
                            int zip = !reader.IsDBNull(zipOrdinal) ? reader.GetInt32(zipOrdinal) : 0;
                            string phone = !reader.IsDBNull(phoneOrdinal) ? reader.GetString(phoneOrdinal) : null;

                            var patient = new Patient
                            {
                                Id = id,
                                FirstName = fname,
                                LastName = lname,
                                SSN = ssn,
                                DOB = dob,
                                Sex = sex,
                                Address = address,
                                City = city,
                                State = state,
                                Zip = zip,
                                Phone = phone
                            };

                            allPatients.Add(patient);
                        }
                    }
                }
            }
            return allPatients;
        }

        public async Task AddPatientAsync(Patient patient)
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                //Open the connection
                await conn.OpenAsync();
                string query = "insert into `patient` values " +
                                "(null,@fname, @lname, @ssn, @dob, @sex, @address, @city, @state, @zip, @phone);";

              
                    using (MySqlCommand comm = new MySqlCommand(query, conn))
                    {
                        comm.Parameters.Add("@fname", (DbType)MySqlDbType.VarChar).Value = patient.FirstName;

                        comm.Parameters.Add("@lname", (DbType)MySqlDbType.VarChar).Value = patient.LastName;

                        comm.Parameters.Add("@ssn", (DbType)MySqlDbType.String).Value = patient.SSN;

                        comm.Parameters.Add("@dob", (DbType)MySqlDbType.DateTime).Value = patient.DOB;

                        comm.Parameters.Add("@sex", (DbType)MySqlDbType.VarChar).Value = patient.Sex;

                        comm.Parameters.Add("@address", (DbType)MySqlDbType.VarChar).Value = patient.Address;

                        comm.Parameters.Add("@city", (DbType)MySqlDbType.VarChar).Value = patient.City;

                        comm.Parameters.Add("@state", (DbType)MySqlDbType.String).Value = patient.State;

                        comm.Parameters.Add("@zip", (DbType)MySqlDbType.VarChar).Value = patient.Zip;

                        comm.Parameters.Add("@phone", (DbType)MySqlDbType.String).Value = patient.Phone;

                        await comm.ExecuteNonQueryAsync();
                    }
             
            }
        }


        public IList<Patient> GetPatientsByName(string fname, string lname)
        {
            var patients = new List<Patient>();
            var qry = "select * from patient where fname = @fname or lname = @lname";
            var conn = DbConnection.GetConnection();
            conn.Open();
            using (var command = new MySqlCommand(qry, conn))
            {
                command.Parameters.Add("@fname", (DbType)MySqlDbType.VarChar).Value = fname;
                command.Parameters.Add("@lname", (DbType)MySqlDbType.VarChar).Value = lname;
                using (var reader = command.ExecuteReader())
                {
                    int idOrdinal = reader.GetOrdinal("id");
                    int fnameOrdinal = reader.GetOrdinal("fname");
                    int lnameOrdinal = reader.GetOrdinal("lname");
                    int ssnOrdinal = reader.GetOrdinal("ssn");
                    int dobOrdinal = reader.GetOrdinal("dob");
                    int sexOrdinal = reader.GetOrdinal("sex");
                    int addressOrdinal = reader.GetOrdinal("address");
                    int cityOrdinal = reader.GetOrdinal("city");
                    int stateOrdinal = reader.GetOrdinal("state");
                    int zipOrdinal = reader.GetOrdinal("zip");
                    int phoneOrdinal = reader.GetOrdinal("phone");

                    while (reader.Read())
                    {
                        int id = !reader.IsDBNull(idOrdinal) ? reader.GetInt32(idOrdinal) : 1;
                        string _fname = !reader.IsDBNull(fnameOrdinal) ? reader.GetString(fnameOrdinal) : null;
                        string _lname = !reader.IsDBNull(lnameOrdinal) ? reader.GetString(lnameOrdinal) : null;
                        string ssn = !reader.IsDBNull(ssnOrdinal) ? reader.GetString(ssnOrdinal) : null;
                        DateTime dob = !reader.IsDBNull(dobOrdinal)
                            ? reader.GetDateTime(dobOrdinal)
                            : new DateTime(2001, 1, 1);
                        string sex = !reader.IsDBNull(sexOrdinal) ? reader.GetString(sexOrdinal) : null;
                        string address = !reader.IsDBNull(addressOrdinal) ? reader.GetString(addressOrdinal) : null;
                        string city = !reader.IsDBNull(cityOrdinal) ? reader.GetString(cityOrdinal) : null;
                        string state = !reader.IsDBNull(stateOrdinal) ? reader.GetString(stateOrdinal) : null;
                        int zip = !reader.IsDBNull(zipOrdinal) ? reader.GetInt32(zipOrdinal) : 0;
                        string phone = !reader.IsDBNull(phoneOrdinal) ? reader.GetString(phoneOrdinal) : null;

                        var patient = new Patient
                        {
                            Id = id,
                            FirstName = _fname,
                            LastName = _lname,
                            SSN = ssn,
                            DOB = dob,
                            Sex = sex,
                            Address = address,
                            City = city,
                            State = state,
                            Zip = zip,
                            Phone = phone
                        };

                        patients.Add(patient);
                    }
                }
            }

            conn.Close();
            return patients;
        }

        public async Task EditPatientInfo(int id, string fname, string lname, string ssn, DateTime dob, string sex, string address, string city, string state, int zip, string phone)
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                //Open the connection
                await conn.OpenAsync();
                string query = "update `patient` set ssn=@ssn, fname=@fname, lname=@lname, dob=@dob, sex=@sex, " +
                                "address=@address, city=@city, state=@state, zip=@zip, phone=@phone where id=@id";

               
                    using (MySqlCommand comm = new MySqlCommand(query, conn))
                    {
                        comm.Parameters.Add("@id", (DbType)MySqlDbType.Int32).Value = id;

                        comm.Parameters.Add("@fname", (DbType)MySqlDbType.VarChar).Value = fname;

                        comm.Parameters.Add("@lname", (DbType)MySqlDbType.VarChar).Value = lname;

                        comm.Parameters.Add("@ssn", (DbType)MySqlDbType.String).Value = ssn;

                        comm.Parameters.Add("@dob", (DbType)MySqlDbType.DateTime).Value = dob;

                        comm.Parameters.Add("@sex", (DbType)MySqlDbType.VarChar).Value = sex;

                        comm.Parameters.Add("@address", (DbType)MySqlDbType.VarChar).Value = address;

                        comm.Parameters.Add("@city", (DbType)MySqlDbType.VarChar).Value = city;

                        comm.Parameters.Add("@state", (DbType)MySqlDbType.String).Value = state;

                        comm.Parameters.Add("@zip", (DbType)MySqlDbType.VarChar).Value = zip;

                        comm.Parameters.Add("@phone", (DbType)MySqlDbType.String).Value = phone;

                       await comm.ExecuteNonQueryAsync();
                    }
                
           
            }
        }

        public async Task<Patient> GetPatientById(int id)
        {
            Patient patient = null;
            var qry = "SELECT * FROM patient WHERE id = @id";
            var conn = DbConnection.GetConnection();
            await conn.OpenAsync();
            using (var command = new MySqlCommand(qry, conn))
            {
                command.Parameters.Add("@id", (DbType)MySqlDbType.Int16).Value = id;

                using (var reader = await command.ExecuteReaderAsync())
                {


                    int idOrdinal = reader.GetOrdinal("id");
                    int fnameOrdinal = reader.GetOrdinal("fname");
                    int lnameOrdinal = reader.GetOrdinal("lname");
                    int ssnOrdinal = reader.GetOrdinal("ssn");
                    int dobOrdinal = reader.GetOrdinal("dob");
                    int sexOrdinal = reader.GetOrdinal("sex");
                    int addressOrdinal = reader.GetOrdinal("address");
                    int cityOrdinal = reader.GetOrdinal("city");
                    int stateOrdinal = reader.GetOrdinal("state");
                    int zipOrdinal = reader.GetOrdinal("zip");
                    int phoneOrdinal = reader.GetOrdinal("phone");

                    while (await reader.ReadAsync())
                    {
                        int _id = !reader.IsDBNull(idOrdinal) ? reader.GetInt32(idOrdinal) : 1;
                        string _fname = !reader.IsDBNull(fnameOrdinal) ? reader.GetString(fnameOrdinal) : null;
                        string _lname = !reader.IsDBNull(lnameOrdinal) ? reader.GetString(lnameOrdinal) : null;
                        string ssn = !reader.IsDBNull(ssnOrdinal) ? reader.GetString(ssnOrdinal) : null;
                        DateTime dob = !reader.IsDBNull(dobOrdinal)
                            ? reader.GetDateTime(dobOrdinal)
                            : new DateTime(2001, 1, 1);
                        string sex = !reader.IsDBNull(sexOrdinal) ? reader.GetString(sexOrdinal) : null;
                        string address = !reader.IsDBNull(addressOrdinal) ? reader.GetString(addressOrdinal) : null;
                        string city = !reader.IsDBNull(cityOrdinal) ? reader.GetString(cityOrdinal) : null;
                        string state = !reader.IsDBNull(stateOrdinal) ? reader.GetString(stateOrdinal) : null;
                        int zip = !reader.IsDBNull(zipOrdinal) ? reader.GetInt32(zipOrdinal) : 0;
                        string phone = !reader.IsDBNull(phoneOrdinal) ? reader.GetString(phoneOrdinal) : null;

                        patient = new Patient()
                        {
                            Id = _id,
                            FirstName = _fname,
                            LastName = _lname,
                            SSN = ssn,
                            DOB = dob,
                            Sex = sex,
                            Address = address,
                            City = city,
                            State = state,
                            Zip = zip,
                            Phone = phone
                        };
                    }
                }
            }

            conn.Close();
            return patient;
        }

        public async Task<List<Patient>> SearchPatient(string dobinput, string fnameinput, string lnameinput)
        {
            try
            {
                var allPatients = new List<Patient>();
            
            DateTime? dt;

            if (string.IsNullOrEmpty(dobinput) || string.IsNullOrWhiteSpace(dobinput))
            {
                dt = null;
            }
            else
            {
                dt = Convert.ToDateTime(dobinput).Date;
            }
            
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    //Open the connection
                    System.Text.EncodingProvider ppp;
                    ppp = System.Text.CodePagesEncodingProvider.Instance;
                    Encoding.RegisterProvider(ppp);
                    conn.Open();
                    string query = "select id, fname, lname, ssn, dob, sex, address, city, state, zip, phone from `patient` ";

                    if (dt != null && !(string.IsNullOrEmpty(fnameinput) || string.IsNullOrWhiteSpace(fnameinput)) && !(string.IsNullOrEmpty(lnameinput) || string.IsNullOrWhiteSpace(lnameinput)))
                    {
                        query += "where `dob`=@dob and `fname`=@fname and `lname`=@lname;";
                    }
                    else if (dt != null && !(string.IsNullOrEmpty(fnameinput) || string.IsNullOrWhiteSpace(fnameinput)) && (string.IsNullOrEmpty(lnameinput) || string.IsNullOrWhiteSpace(lnameinput)))
                    {
                        query += "where `dob`=@dob and `fname`=@fname;";
                    }
                    else if (dt != null && (string.IsNullOrEmpty(fnameinput) || string.IsNullOrWhiteSpace(fnameinput)) && !(string.IsNullOrEmpty(lnameinput) || string.IsNullOrWhiteSpace(lnameinput)))
                    {
                        query += "where `dob`=@dob and `lname`=@lname;";
                    }
                    else if (dt == null && !(string.IsNullOrEmpty(fnameinput) || string.IsNullOrWhiteSpace(fnameinput)) && !(string.IsNullOrEmpty(lnameinput) || string.IsNullOrWhiteSpace(lnameinput)))
                    {
                        query += "where `fname`=@fname and `lname`=@lname;";
                    }
                    else if (dt != null && (string.IsNullOrEmpty(fnameinput) || string.IsNullOrWhiteSpace(fnameinput)) && (string.IsNullOrEmpty(lnameinput) || string.IsNullOrWhiteSpace(lnameinput)))
                    {
                        query += "where `dob`=@dob;";
                    }
                    else if (dt == null && !(string.IsNullOrEmpty(fnameinput) || string.IsNullOrWhiteSpace(fnameinput)) && (string.IsNullOrEmpty(lnameinput) || string.IsNullOrWhiteSpace(lnameinput)))
                    {
                        query += "where `fname`=@fname;";
                    }
                    else if (dt == null && (string.IsNullOrEmpty(fnameinput) || string.IsNullOrWhiteSpace(fnameinput)) && !(string.IsNullOrEmpty(lnameinput) || string.IsNullOrWhiteSpace(lnameinput)))
                    {
                        query += "where `lname`=@lname;";
                    }
                    else
                    {
                        query += "";
                    }
                    //DateTime dt;
                    using (MySqlCommand comm = new MySqlCommand(query, conn))
                    {
                        comm.Parameters.Add("@dob", (DbType)MySqlDbType.DateTime).Value = dt;
                        comm.Parameters.Add("@fname", (DbType)MySqlDbType.VarChar).Value = fnameinput;
                        comm.Parameters.Add("@lname", (DbType)MySqlDbType.VarChar).Value = lnameinput;

                        using (var reader = await comm.ExecuteReaderAsync())
                        {
                            int idOrdinal = reader.GetOrdinal("id");
                            int fnameOrdinal = reader.GetOrdinal("fname");
                            int lnameOrdinal = reader.GetOrdinal("lname");
                            int ssnOrdinal = reader.GetOrdinal("ssn");
                            int dobOrdinal = reader.GetOrdinal("dob");
                            int sexOrdinal = reader.GetOrdinal("sex");
                            int addressOrdinal = reader.GetOrdinal("address");
                            int cityOrdinal = reader.GetOrdinal("city");
                            int stateOrdinal = reader.GetOrdinal("state");
                            int zipOrdinal = reader.GetOrdinal("zip");
                            int phoneOrdinal = reader.GetOrdinal("phone");

                            while (await reader.ReadAsync())
                            {
                                int id = !reader.IsDBNull(idOrdinal) ? reader.GetInt32(idOrdinal) : 1;
                                string fname = !reader.IsDBNull(fnameOrdinal) ? reader.GetString(fnameOrdinal) : null;
                                string lname = !reader.IsDBNull(lnameOrdinal) ? reader.GetString(lnameOrdinal) : null;
                                string ssn = !reader.IsDBNull(ssnOrdinal) ? reader.GetString(ssnOrdinal) : null;
                                DateTime dob = !reader.IsDBNull(dobOrdinal) ? reader.GetDateTime(dobOrdinal).Date : new DateTime(2001, 1, 1).Date;
                                string sex = !reader.IsDBNull(sexOrdinal) ? reader.GetString(sexOrdinal) : null;
                                string address = !reader.IsDBNull(addressOrdinal) ? reader.GetString(addressOrdinal) : null;
                                string city = !reader.IsDBNull(cityOrdinal) ? reader.GetString(cityOrdinal) : null;
                                string state = !reader.IsDBNull(stateOrdinal) ? reader.GetString(stateOrdinal) : null;
                                int zip = !reader.IsDBNull(zipOrdinal) ? reader.GetInt32(zipOrdinal) : 0;
                                string phone = !reader.IsDBNull(phoneOrdinal) ? reader.GetString(phoneOrdinal) : null;

                                var patient = new Patient
                                {
                                    Id = id,
                                    FirstName = fname,
                                    LastName = lname,
                                    SSN = ssn,
                                    DOB = dob,
                                    Sex = sex,
                                    Address = address,
                                    City = city,
                                    State = state,
                                    Zip = zip,
                                    Phone = phone
                                };

                                allPatients.Add(patient);
                            }
                        }
                    }
                }
                return allPatients;
            } catch (Exception)
            {
                var dialog = new MessageDialog("Please enter a valid Date: YYYY-MM-DD");
                await dialog.ShowAsync();
                return await this.GetAllPatients();
            }
        }

    }
}
