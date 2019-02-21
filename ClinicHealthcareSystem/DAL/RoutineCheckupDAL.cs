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

    public class RoutineCheckupDAL
    {
        private NurseDAL nurseDal;
        private AppointmentDAL appntDal;
        private PatientDAL patientDal;

        public RoutineCheckupDAL()
        {
            this.nurseDal = new NurseDAL();
            this.appntDal = new AppointmentDAL();
            this.patientDal = new PatientDAL();
        }

        public async Task AddCheckp(RoutineCheck checkup)
        {
            var qry =
                "insert into `routine_check`(aId, nId, date, systolicReading, diastolicBloodPressure, weight,  temp, pId, init_diagnosis) VALUES(@aid, @nid, @date, @systolicReading, @diastolicBloodPressure, @weight, @temp, @pId, @init); delete from `incomplete_appointments` where aId = @aID; insert into `complete_appointments` values(@aId);";
            var conn = DbConnection.GetConnection();
            await conn.OpenAsync();
            using (var command = new MySqlCommand(qry, conn))
            {
                var aId = checkup.Appointment.AppointmentId;
                var nId = checkup.Nurse.NurseId;
                var pId = checkup.Patient.Id;
                var date = checkup.Date;
                var systolicReading = checkup.SystolicReading;
                var diastolicBloodPressure = checkup.DiastolicBloodPressure;
                var weight = checkup.Weight;
                var init = checkup.InitialDiagnosis;
                var temp = checkup.Temperature;

                command.Parameters.Add("@aid", (DbType) MySqlDbType.Int16).Value = aId;
                command.Parameters.Add("@nid", (DbType) MySqlDbType.Int16).Value = nId;
                command.Parameters.Add("@pId", (DbType) MySqlDbType.Int16).Value = pId;
                command.Parameters.Add("@date", (DbType) MySqlDbType.DateTime).Value = date;
                command.Parameters.Add("@systolicReading", (DbType) MySqlDbType.Double).Value = systolicReading;
                command.Parameters.Add("@diastolicBloodPressure", (DbType) MySqlDbType.Double).Value =
                    diastolicBloodPressure;
                command.Parameters.Add("@weight", (DbType) MySqlDbType.Double).Value = weight;
                command.Parameters.Add("@init", (DbType) MySqlDbType.VarChar).Value = init;
                command.Parameters.Add("@temp", (DbType) MySqlDbType.Double).Value = temp;

                await command.ExecuteNonQueryAsync();
            }

            conn.Close();
        }

        public async Task<IList<RoutineCheck>> GetPatientRoutineChecks(Patient patient)
        {
            var checkups = new List<RoutineCheck>();
            var qry = "select * from routine_check where pId = @pId";
            var conn = DbConnection.GetConnection();
            await conn.OpenAsync();
            using (var command = new MySqlCommand(qry, conn))
            {
                command.Parameters.Add("@pId", (DbType) MySqlDbType.Int16).Value = patient.Id;

                using (var reader = await command.ExecuteReaderAsync())
                {
                    var aIdOrdinal = reader.GetOrdinal("aId");
                    var pIdOrdinal = reader.GetOrdinal("pId");
                    var nIdOrdinal = reader.GetOrdinal("nId");
                    var dateOrdinal = reader.GetOrdinal("date");
                    var systolicReadingOrdinal = reader.GetOrdinal("systolicReading");
                    var diastolicBloodPressureOrdinal = reader.GetOrdinal("diastolicBloodPressure");
                    var weightOrdinal = reader.GetOrdinal("weight");
                    var initial_diagnosisOrd = reader.GetOrdinal("init_diagnosis");
                    var temperatureOrdinal = reader.GetOrdinal("temp");
                    while (await reader.ReadAsync())
                    {
                        var aId = !reader.IsDBNull(aIdOrdinal) ? reader.GetInt32(aIdOrdinal) : 1;
                        var nId = !reader.IsDBNull(nIdOrdinal) ? reader.GetInt32(nIdOrdinal) : 1;
                        var date = reader.GetDateTime(dateOrdinal);
                        var diastolicBloodPressure = reader.GetDouble(diastolicBloodPressureOrdinal);
                        var weight = reader.GetDouble(weightOrdinal);
                        var systolicReading = reader.GetDouble(systolicReadingOrdinal);
                        var initial_diagnosis = !reader.IsDBNull(initial_diagnosisOrd)
                            ? reader.GetString(initial_diagnosisOrd)
                            : string.Empty;
                        var temperature = reader.GetDouble(temperatureOrdinal);

                        var nurse = await this.nurseDal.GetNurseByNurseId(nId);
                        var appnt = await this.appntDal.GetAppointmentById(aId);


                        checkups.Add(new RoutineCheck(appnt, nurse, patient, date, systolicReading,
                            diastolicBloodPressure, weight, temperature, initial_diagnosis));
                    }
                }
            }

            conn.Close();
            return checkups;
        }

        public async Task<IList<RoutineCheck>> GetAllCheckups()
        {
            var checkups = new List<RoutineCheck>();
            var qry = "select * from routine_check";
            var conn = DbConnection.GetConnection();
            await conn.OpenAsync();
            using (var command = new MySqlCommand(qry, conn))
            {
               using (var reader = await command.ExecuteReaderAsync())
                {
                    var aIdOrdinal = reader.GetOrdinal("aId");
                    var pIdOrdinal = reader.GetOrdinal("pId");
                    var nIdOrdinal = reader.GetOrdinal("nId");
                    var dateOrdinal = reader.GetOrdinal("date");
                    var systolicReadingOrdinal = reader.GetOrdinal("systolicReading");
                    var diastolicBloodPressureOrdinal = reader.GetOrdinal("diastolicBloodPressure");
                    var weightOrdinal = reader.GetOrdinal("weight");
                    var initial_diagnosisOrd = reader.GetOrdinal("init_diagnosis");
                    var temperatureOrdinal = reader.GetOrdinal("temp");
                    while (await reader.ReadAsync())
                    {
                        var aId = !reader.IsDBNull(aIdOrdinal) ? reader.GetInt32(aIdOrdinal) : 1;
                        var pId = !reader.IsDBNull(pIdOrdinal) ? reader.GetInt32(pIdOrdinal) : 1;
                        var nId = !reader.IsDBNull(nIdOrdinal) ? reader.GetInt32(nIdOrdinal) : 1;
                        var date = reader.GetDateTime(dateOrdinal);
                        var diastolicBloodPressure = reader.GetDouble(diastolicBloodPressureOrdinal);
                        var weight = reader.GetDouble(weightOrdinal);
                        var systolicReading = reader.GetDouble(systolicReadingOrdinal);
                        var initial_diagnosis = !reader.IsDBNull(initial_diagnosisOrd)
                            ? reader.GetString(initial_diagnosisOrd)
                            : string.Empty;
                        var temperature = reader.GetDouble(temperatureOrdinal);

                        var nurse = await this.nurseDal.GetNurseByNurseId(nId);
                        var appnt = await this.appntDal.GetAppointmentById(aId);
                        var patient = await this.patientDal.GetPatientById(pId);

                        checkups.Add(new RoutineCheck(appnt, nurse, patient, date, systolicReading,
                            diastolicBloodPressure, weight, temperature, initial_diagnosis));
                    }
                }
            }

            conn.Close();
            return checkups;
        }

        public async Task<List<PatientCheckUps>> GetPatientCheckUps(string ssn)
        {
            var checkups = new List<PatientCheckUps>();
            var qry =
                "select `aId`, concat(firstname, ' ',lastname) as fullname, date from `routine_check`,`nurse` as n,`user` as u,`patient` as p where p.`ssn`=@ssn and pId = p.`id` and `nId`=n.`nurseId` and u.`id`=n.`userId`";
            var conn = DbConnection.GetConnection();
            await conn.OpenAsync();
            using (var command = new MySqlCommand(qry, conn))
            {
                command.Parameters.Add("@ssn", (DbType) MySqlDbType.VarString).Value = ssn;

                using (var reader = await command.ExecuteReaderAsync())
                {
                    var aIdOrdinal = reader.GetOrdinal("aId");
                    var nurseOrdinal = reader.GetOrdinal("fullname");
                    var dateOrdinal = reader.GetOrdinal("date");
                    while (await reader.ReadAsync())
                    {
                        var aId = !reader.IsDBNull(aIdOrdinal) ? reader.GetInt32(aIdOrdinal) : 1;
                        var nurse = !reader.IsDBNull(nurseOrdinal) ? reader.GetString(nurseOrdinal) : string.Empty;
                        var date = reader.GetDateTime(dateOrdinal);

                        var checkup = new PatientCheckUps {
                            AppointmentId = aId,
                            NurseFullname = nurse,
                            Date = date
                        };

                        checkups.Add(checkup);
                    }
                }
            }

            return checkups;
        }

        public async Task UpdateCheckup(RoutineCheck checkup)
        {
            var qry =
                "update routine_check set date = @date, systolicReading = @systolicReading, diastolicBloodPressure = @diastolicBloodPressure, weight = @weight, temp = @temp, init_diagnosis = @init_diagnosis where aId = @aID";
            var conn = DbConnection.GetConnection();
            await conn.OpenAsync();
            using (var command = new MySqlCommand(qry, conn))
            {
                command.Parameters.AddWithValue("@date", checkup.Date);
                command.Parameters.AddWithValue("@systolicReading", checkup.SystolicReading);
                command.Parameters.AddWithValue("@diastolicBloodPressure", checkup.DiastolicBloodPressure);
                command.Parameters.AddWithValue("@weight", checkup.Weight);
                command.Parameters.AddWithValue("@temp", checkup.Temperature);
                command.Parameters.AddWithValue("@aID", checkup.Appointment.AppointmentId);
                command.Parameters.AddWithValue("@init_diagnosis", checkup.InitialDiagnosis);
                await command.ExecuteNonQueryAsync();
            }

            conn.Close();
        }
    }
}
