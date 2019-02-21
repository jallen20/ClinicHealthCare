using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using ClinicHealthcareSystem.Models;
using MySql.Data.MySqlClient;

namespace ClinicHealthcareSystem.DAL
{

    public class AppointmentDAL
    {
        private PatientDAL patientDal;
        private DoctorDAL doctorDal;

        public AppointmentDAL()
        {
            this.patientDal = new PatientDAL();
            this.doctorDal = new DoctorDAL();
        }
        public async Task AddAppointment(Appointment appointment)
        {
            var pId = appointment.Patient.Id;
            var dId = appointment.Doctor.DoctorId;
            var date = appointment.Date;
            var description = appointment.Description;
            var qry =
                "INSERT INTO `appointment`(pId,  dId, date, description) VALUES(@pId, @dId, @date, @description); INSERT INTO incomplete_appointments(`aId`) values((select aId from appointment where dId = @dId and date = @date));";
            var conn = DbConnection.GetConnection();
            await conn.OpenAsync();
            
                using (var command = new MySqlCommand(qry, conn))
                {
                    command.Parameters.Add("@pId", (DbType) MySqlDbType.Int16).Value = pId;
                    command.Parameters.Add("@dId", (DbType) MySqlDbType.VarChar).Value = dId;
                    command.Parameters.Add("@date", (DbType) MySqlDbType.DateTime).Value = date;
                    command.Parameters.Add("@description", (DbType) MySqlDbType.VarChar).Value = description;

                    await command.ExecuteNonQueryAsync();
                    conn.Close();
                }
        }

        public async Task<IList<Appointment>> GetAllInCompleteAppointments()
        {
            var appointments = new List<Appointment>();
            var qry = "select * from appointment where aId in(select * from incomplete_appointments)";
            var conn = DbConnection.GetConnection();
            await conn.OpenAsync();
            using (var command = new MySqlCommand(qry, conn))
            {
                using(var reader = await command.ExecuteReaderAsync())
                {
                    var aIdOrdinal = reader.GetOrdinal("aId");
                    var pIdOrdinal = reader.GetOrdinal("pId");
                    var dIdOrdinal = reader.GetOrdinal("dId");
                    var dateOrdinal = reader.GetOrdinal("date");
                    var descrOrdinal = reader.GetOrdinal("description");
                    while (await reader.ReadAsync())
                    {
                        var aId = !reader.IsDBNull(aIdOrdinal) ? reader.GetInt32(aIdOrdinal) : 1;
                        var pId = !reader.IsDBNull(pIdOrdinal) ? reader.GetInt32(pIdOrdinal) : 1;
                        var dId = !reader.IsDBNull(dIdOrdinal) ? reader.GetInt32(dIdOrdinal) : 1;
                        var date = reader.GetDateTime(dateOrdinal);
                        var description = !reader.IsDBNull(descrOrdinal) ? reader.GetString(descrOrdinal) : string.Empty;

                        var patient = await this.patientDal.GetPatientById(pId);
                        var doctor = await this.doctorDal.GetDoctorByDoctorId(dId);

                        var appointment = new Appointment(patient, doctor, date, description)
                        {
                            AppointmentId = aId
                        };
                        appointments.Add(appointment);
                    }

                }
            }

            conn.Close();
            return appointments;
        }

        public async Task<IList<Appointment>> GetAllCompleteAppointments()
        {
            var appointments = new List<Appointment>();
            var qry = "select * from appointment where aId in(select * from complete_appointments)";
            var conn = DbConnection.GetConnection();
            await conn.OpenAsync();
            using (var command = new MySqlCommand(qry, conn))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    var aIdOrdinal = reader.GetOrdinal("aId");
                    var pIdOrdinal = reader.GetOrdinal("pId");
                    var dIdOrdinal = reader.GetOrdinal("dId");
                    var dateOrdinal = reader.GetOrdinal("date");
                    var descrOrdinal = reader.GetOrdinal("description");
                    while (await reader.ReadAsync())
                    {
                        var aId = !reader.IsDBNull(aIdOrdinal) ? reader.GetInt32(aIdOrdinal) : 1;
                        var pId = !reader.IsDBNull(pIdOrdinal) ? reader.GetInt32(pIdOrdinal) : 1;
                        var dId = !reader.IsDBNull(dIdOrdinal) ? reader.GetInt32(dIdOrdinal) : 1;
                        var date = reader.GetDateTime(dateOrdinal);
                        var description = !reader.IsDBNull(descrOrdinal) ? reader.GetString(descrOrdinal) : string.Empty;

                        var patient = await this.patientDal.GetPatientById(pId);
                        var doctor = await this.doctorDal.GetDoctorByDoctorId(dId);

                        var appointment = new Appointment(patient, doctor, date, description)
                        {
                            AppointmentId = aId
                        };
                        appointments.Add(appointment);
                    }

                }
            }

            conn.Close();
            return appointments;
        }

        public async Task<IList<Appointment>> GetAllAppointments()
        {
            var appointments = new List<Appointment>();
            var qry = "select * from appointment";
            var conn = DbConnection.GetConnection();
            await conn.OpenAsync();
            using (var command = new MySqlCommand(qry, conn))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    var aIdOrdinal = reader.GetOrdinal("aId");
                    var pIdOrdinal = reader.GetOrdinal("pId");
                    var dIdOrdinal = reader.GetOrdinal("dId");
                    var dateOrdinal = reader.GetOrdinal("date");
                    var descrOrdinal = reader.GetOrdinal("description");
                    while (await reader.ReadAsync())
                    {
                        var aId = !reader.IsDBNull(aIdOrdinal) ? reader.GetInt32(aIdOrdinal) : 1;
                        var pId = !reader.IsDBNull(pIdOrdinal) ? reader.GetInt32(pIdOrdinal) : 1;
                        var dId = !reader.IsDBNull(dIdOrdinal) ? reader.GetInt32(dIdOrdinal) : 1;
                        var date = reader.GetDateTime(dateOrdinal);
                        var description = !reader.IsDBNull(descrOrdinal)
                            ? reader.GetString(descrOrdinal)
                            : string.Empty;

                        var patient = await this.patientDal.GetPatientById(pId);
                        var doctor = await this.doctorDal.GetDoctorByDoctorId(dId);

                        var appointment = new Appointment(patient, doctor, date, description) {
                            AppointmentId = aId
                        };
                        appointments.Add(appointment);
                    }

                }
            }
            conn.Close();
            return appointments;
        }

       
        public async Task<Appointment> GetAppointmentById(int id)
        {
            Appointment appointment = null;
            var qry = "select * from appointment where aId = @id";
            var conn = DbConnection.GetConnection();
            await conn.OpenAsync();
            using (var command = new MySqlCommand(qry, conn))
            {
                command.Parameters.Add("@id", (DbType) MySqlDbType.Int16).Value = id;
                using (var reader = await command.ExecuteReaderAsync())
                {
                    var aIdOrdinal = reader.GetOrdinal("aId");
                    var pIdOrdinal = reader.GetOrdinal("pId");
                    var dIdOrdinal = reader.GetOrdinal("dId");
                    var dateOrdinal = reader.GetOrdinal("date");
                    var descrOrdinal = reader.GetOrdinal("description");
                    while (await reader.ReadAsync())
                    {
                        var aId = !reader.IsDBNull(aIdOrdinal) ? reader.GetInt32(aIdOrdinal) : 1;
                        var pId = !reader.IsDBNull(pIdOrdinal) ? reader.GetInt32(pIdOrdinal) : 1;
                        var dId = !reader.IsDBNull(dIdOrdinal) ? reader.GetInt32(dIdOrdinal) : 1;
                        var date = reader.GetDateTime(dateOrdinal);
                        var description = !reader.IsDBNull(descrOrdinal) ? reader.GetString(descrOrdinal) : string.Empty;

                        var patient = await this.patientDal.GetPatientById(pId);
                        var doctor = await this.doctorDal.GetDoctorByDoctorId(dId);

                        appointment = new Appointment(patient, doctor, date, description)
                        {
                            AppointmentId = aId
                        };
                     
                    }

                }
            }

            conn.Close();
            return appointment;
        }

        public async Task<IList<Appointment>> GetAppointmentsByPatient(int id)
        {
            var appointments = new List<Appointment>();
            var qry = "select * from appointment where pId = @id";
            var conn = DbConnection.GetConnection();
            await conn.OpenAsync();
            using (var command = new MySqlCommand(qry, conn))
            {
                command.Parameters.Add("@id", (DbType) DbType.Int16).Value = id;
                using (var reader = await command.ExecuteReaderAsync())
                {
                    var aIdOrdinal = reader.GetOrdinal("aId");
                    var pIdOrdinal = reader.GetOrdinal("pId");
                    var dIdOrdinal = reader.GetOrdinal("dId");
                    var dateOrdinal = reader.GetOrdinal("date");
                    var descrOrdinal = reader.GetOrdinal("description");
                    while (reader.Read())
                    {
                        var aId = !reader.IsDBNull(aIdOrdinal) ? reader.GetInt32(aIdOrdinal) : 1;
                        var pId = !reader.IsDBNull(pIdOrdinal) ? reader.GetInt32(pIdOrdinal) : 1;
                        var dId = !reader.IsDBNull(dIdOrdinal) ? reader.GetInt32(dIdOrdinal) : 1;
                        var date = reader.GetDateTime(dateOrdinal);
                        var description = !reader.IsDBNull(descrOrdinal) ? reader.GetString(descrOrdinal) : string.Empty;

                        var patient = await this.patientDal.GetPatientById(pId);
                        var doctor = await this.doctorDal.GetDoctorByDoctorId(dId);

                        var appointment = new Appointment(patient, doctor, date, description)
                        {
                            AppointmentId = aId
                        };
                        appointments.Add(appointment);
                    }

                }
            }
            conn.Close();
            return appointments;
        }

        public async Task EditAppointment(Appointment appnt)
        {
            var qry =
                $"update `appointment` set pId = @pId, dId = @dId, date = @date, description = @description where aId = @aId";
            var conn = DbConnection.GetConnection();
            await conn.OpenAsync();
            using (var command = new MySqlCommand(qry, conn))
            {
                command.Parameters.Add("@pId", (DbType)MySqlDbType.Int16).Value = appnt.Patient.Id;
                command.Parameters.AddWithValue("@dId", (DbType)MySqlDbType.Int16).Value = appnt.Doctor.DoctorId;
                command.Parameters.AddWithValue("@date", (DbType)MySqlDbType.DateTime).Value = appnt.Date;
                command.Parameters.Add("@description", (DbType)MySqlDbType.VarChar).Value = appnt.Description;
                command.Parameters.Add("@aId", (DbType)MySqlDbType.Int16).Value = appnt.AppointmentId;

                await command.ExecuteNonQueryAsync();
                conn.Close();
            }
           
            
        }


    }
}
