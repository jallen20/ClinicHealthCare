using ClinicHealthcareSystem.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicHealthcareSystem.DAL
{
    public class LabTestDAL
    {
        public LabTestDAL()
        {

        }

        public async Task<List<LabTest>> GetAllLabTests()
        {
            var allTests = new List<LabTest>();

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                //Open the connection
                System.Text.EncodingProvider ppp;
                ppp = System.Text.CodePagesEncodingProvider.Instance;
                Encoding.RegisterProvider(ppp);
                await conn.OpenAsync();
                string query = "select `code`, `name` from `lab_test`";
                using (MySqlCommand comm = new MySqlCommand(query, conn))
                {
                    using (var reader = await comm.ExecuteReaderAsync())
                    {
                        int codeOrdinal = reader.GetOrdinal("code");
                        int nameOrdinal = reader.GetOrdinal("name");

                        while (reader.Read())
                        {
                            string code = !reader.IsDBNull(codeOrdinal) ? reader.GetString(codeOrdinal) : null;
                            string name = !reader.IsDBNull(nameOrdinal) ? reader.GetString(nameOrdinal) : null;

                            var labtest = new LabTest
                            {
                                Code = code,
                                Name = name
                            };

                            allTests.Add(labtest);
                        }
                    }
                }
                conn.Close();
            }
            return allTests;
        }

        public async Task OrderLabTest(int patientId, DateTime orderDate, DateTime dueDate)
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                //Open the connection
                System.Text.EncodingProvider ppp;
                ppp = System.Text.CodePagesEncodingProvider.Instance;
                Encoding.RegisterProvider(ppp);
                await conn.OpenAsync();
                string query1 = "insert into `ordered_test` (`patientID`, `order_date`, `due_date`) values " +
                                "(@patientId, @orderDate, @dueDate);";

                using (MySqlCommand comm = new MySqlCommand(query1, conn))
                {
                    comm.Parameters.Add("@patientId", (DbType)MySqlDbType.Int32).Value = patientId;
                    
                    comm.Parameters.Add("@orderDate", (DbType)MySqlDbType.DateTime).Value = orderDate.Date;

                    comm.Parameters.Add("@dueDate", (DbType)MySqlDbType.DateTime).Value = dueDate.Date;

                   await  comm.ExecuteNonQueryAsync();
                }
                conn.Close();
            }
        }

        public async Task EnterTestResult(string code, string name, decimal? result)
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                //Open the connection
                System.Text.EncodingProvider ppp;
                ppp = System.Text.CodePagesEncodingProvider.Instance;
                Encoding.RegisterProvider(ppp);
                await conn.OpenAsync();
                string query = "insert into `ordered_test_components` (`orderID`, `code`, `name`, `result`) values " +
                                "((select MAX(`id`) from `ordered_test`), @code, @name, @result);";

                using (MySqlCommand comm = new MySqlCommand(query, conn))
                {
                    comm.Parameters.Add("@code", (DbType)MySqlDbType.VarChar).Value = code;

                    comm.Parameters.Add("@name", (DbType)MySqlDbType.VarChar).Value = name;

                    comm.Parameters.Add("@result", (DbType)MySqlDbType.Decimal).Value = result;

                    await comm.ExecuteNonQueryAsync();
                }
                conn.Close();
            }
        }

        /// <summary>
        /// Gets the patient lab tests.
        /// </summary>
        /// <param name="ssn">The SSN.</param>
        /// <returns></returns>
        public async Task<List<PatientLabTests>> GetPatientLabTests(string ssn)
        {
            //TODO MAKE ME ASYNC
            var labtests = new List<PatientLabTests>();

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                //Open the connection
                await conn.OpenAsync();
                string query = "select o.`id`, `order_date`, `due_date`, `code`, `name`, `result` from `ordered_test` as o, `ordered_test_components` as oc, `patient` as p where p.`ssn`=@ssn and `patientID`=p.`id` and oc.`orderID`=o.`id`";
                using (MySqlCommand comm = new MySqlCommand(query, conn))
                {
                    comm.Parameters.Add("@ssn", (DbType)MySqlDbType.String).Value = ssn;

                    using (var reader = await comm.ExecuteReaderAsync())
                    {
                        int idOrdinal = reader.GetOrdinal("id");
                        int orderDateOrdinal = reader.GetOrdinal("order_date");
                        int dueDateOrdinal = reader.GetOrdinal("due_date");
                        int codeOrdinal = reader.GetOrdinal("code");
                        int nameOrdinal = reader.GetOrdinal("name");
                        int resultOrdinal = reader.GetOrdinal("result");

                        while (await reader.ReadAsync())
                        {
                            var id = !reader.IsDBNull(idOrdinal) ? reader.GetInt32(idOrdinal) : 1;
                            var orderDate = !reader.IsDBNull(orderDateOrdinal) ? reader.GetDateTime(orderDateOrdinal) : new DateTime();
                            var dueDate = !reader.IsDBNull(dueDateOrdinal) ? reader.GetDateTime(dueDateOrdinal) : new DateTime();
                            var code = !reader.IsDBNull(codeOrdinal) ? reader.GetString(codeOrdinal) : string.Empty;
                            var name = !reader.IsDBNull(nameOrdinal) ? reader.GetString(nameOrdinal) : string.Empty;
                            var result = !reader.IsDBNull(resultOrdinal) ? reader.GetString(resultOrdinal) : string.Empty;

                            var labTest = new PatientLabTests
                            {
                                OrderId = id,
                                Order_Date = orderDate,
                                Due_Date = dueDate,
                                Code = code,
                                Name = name,
                                Result = result
                            };

                            labtests.Add(labTest);
                        }
                    }
                }
                conn.Close();
            }
            return labtests;
        }

        }
}
