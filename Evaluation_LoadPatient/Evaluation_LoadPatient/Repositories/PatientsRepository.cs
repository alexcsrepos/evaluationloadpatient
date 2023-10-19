using Evaluation_LoadPatient.Cache;
using Evaluation_LoadPatient.Constants;
using Evaluation_LoadPatient.Extensions;
using Evaluation_LoadPatient.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Evaluation_LoadPatient.Repositories
{
    internal class PatientsRepository
    {
        public async Task InsertPatientsAsync(List<Patient> patients)
        {
            using (SqlConnection conn = new SqlConnection(DefaultConnectionStringCache.GetConnectionString()))
            {
                conn.Open();
                var createTempTableCommand = new SqlCommand(SqlConstants.CreatePatientsTempTable, conn);
                await createTempTableCommand.ExecuteNonQueryAsync();
                DataTable patientsTable = MakeTable(patients);
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
                {
                    bulkCopy.DestinationTableName = "#Patient";
                    foreach (DataColumn column in patientsTable.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(column.ColumnName, column.ColumnName));
                    }
                    await bulkCopy.WriteToServerAsync(patientsTable);
                    var insertCommand = new SqlCommand(SqlConstants.InsertIntoPacientsTable, conn);
                    await insertCommand.ExecuteNonQueryAsync();
                    var insertCommandPhones = new SqlCommand(SqlConstants.InsertPhoneNumbers, conn);
                    await insertCommandPhones.ExecuteNonQueryAsync();
                    var insertCommandRiskFactors = new SqlCommand(SqlConstants.InsertRiskFactors, conn);
                    await insertCommandRiskFactors.ExecuteNonQueryAsync();
                }
            }
        }

        public List<Patient> GetPatients(string textBoxText)
        {

            var patients = new List<Patient>();
            using (var reader = new System.IO.StreamReader(textBoxText))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    try { patients.Add(MapCVLineToPatient(line)); }
                    catch(Exception ex) {
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            return patients;
        }
        public Patient MapCVLineToPatient(string line)
        {
            var splitLine = Regex.Split(line, ";(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)").Select(x => x.Trim('"')).ToArray();
            Patient patient = new Patient();

            patient.Id = new Guid(splitLine[1]);
            patient.FirstName = splitLine[2];
            patient.LastName = splitLine[3];
            if (string.IsNullOrEmpty(patient.FirstName) && string.IsNullOrEmpty(patient.LastName) && !string.IsNullOrEmpty(splitLine[4]))
            {
                if (splitLine[4].Contains(","))
                {
                    var names = splitLine[4].Split(',');
                    patient.FirstName = names[1];
                    patient.LastName = names[0];
                }
                else if(splitLine[4].Contains(" "))
                {
                    var names = splitLine[4].Split(' ');
                    patient.FirstName = names[0];
                    patient.LastName = names[1];
                }
                else
                {
                    patient.FirstName = splitLine[4];
                }
            }
            patient.DateOfBirth = DateTime.ParseExact(splitLine[5], "yyyy-M-d", System.Globalization.CultureInfo.InvariantCulture);
            patient.Sex = splitLine[6];
            patient.Language = splitLine[7];
            patient.NAM = splitLine[8];
            try
            {
                patient.NamExpiryDate = new DateTime(Convert.ToInt32(splitLine[9]), Convert.ToInt32(splitLine[10]), 01);
            }
            catch // Try to cast the experity date
            {
            }
            patient.MaritalStatus = splitLine[12];
            patient.Note = splitLine[13];
            patient.MotherFirstName = splitLine[14];
            patient.MotherLastName = splitLine[15];
            patient.FatherFirstName = splitLine[16];
            patient.FatherLastName = splitLine[17];

            patient.Email = splitLine[18];

            patient.Address = splitLine[25];
            patient.Country = splitLine[26];
            patient.Id03 = splitLine[36];
            patient.PhoneNumber1 = splitLine[19];
            patient.PhoneNumber2 = splitLine[21];
            patient.PhoneNumber3 = splitLine[23];
            patient.PhoneNumberNote1 = splitLine[20];
            patient.PhoneNumberNote2 = splitLine[22];
            patient.PhoneNumberNote3 = splitLine[24];
            patient.Smoke = splitLine[31];
            patient.SmokeInThePast = splitLine[32];
            patient.TakeDrugs = splitLine[33];
            return patient;
        }

        private static DataTable MakeTable(List<Patient> patients)
        {
            DataTable patientsTable = new DataTable("Patients");

            DataColumn id = new DataColumn();
            id.DataType = Type.GetType("System.String");
            id.ColumnName = "Id";
            patientsTable.Columns.Add(id);

            DataColumn firstName = new DataColumn();
            firstName.DataType = Type.GetType("System.String");
            firstName.ColumnName = "FirstName";
            patientsTable.Columns.Add(firstName);

            DataColumn lastName = new DataColumn();
            lastName.DataType = Type.GetType("System.String");
            lastName.ColumnName = "LastName";
            patientsTable.Columns.Add(lastName);

            DataColumn smoke = new DataColumn();
            smoke.DataType = Type.GetType("System.String");
            smoke.ColumnName = "Smoke";
            patientsTable.Columns.Add(smoke);

            DataColumn smokeInThePast = new DataColumn();
            smokeInThePast.DataType = Type.GetType("System.String");
            smokeInThePast.ColumnName = "SmokeInThePast";
            patientsTable.Columns.Add(smokeInThePast);

            DataColumn takeDrugs = new DataColumn();
            takeDrugs.DataType = Type.GetType("System.String");
            takeDrugs.ColumnName = "TakeDrugs";
            patientsTable.Columns.Add(takeDrugs);

            DataColumn dateOfBirth = new DataColumn();
            dateOfBirth.DataType = Type.GetType("System.DateTime");
            dateOfBirth.ColumnName = "DateOfBirth";
            patientsTable.Columns.Add(dateOfBirth);

            DataColumn nam = new DataColumn();
            nam.DataType = Type.GetType("System.String");
            nam.ColumnName = "NAM";
            patientsTable.Columns.Add(nam);

            DataColumn namExpiryDate = new DataColumn();
            namExpiryDate.DataType = Type.GetType("System.DateTime");
            namExpiryDate.ColumnName = "NamExpiryDate";
            patientsTable.Columns.Add(namExpiryDate);
          

            DataColumn note = new DataColumn();
            note.DataType = Type.GetType("System.String");
            note.ColumnName = "Note";
            patientsTable.Columns.Add(note);

            DataColumn motherFirstName = new DataColumn();
            motherFirstName.DataType = Type.GetType("System.String");
            motherFirstName.ColumnName = "MotherFirstName";
            patientsTable.Columns.Add(motherFirstName);

            DataColumn motherLastName = new DataColumn();
            motherLastName.DataType = Type.GetType("System.String");
            motherLastName.ColumnName = "MotherLastName";
            patientsTable.Columns.Add(motherLastName);

            DataColumn fatherFirstName = new DataColumn();
            fatherFirstName.DataType = Type.GetType("System.String");
            fatherFirstName.ColumnName = "FatherFirstName";
            patientsTable.Columns.Add(fatherFirstName);

            DataColumn fatherLastName = new DataColumn();
            fatherLastName.DataType = Type.GetType("System.String");
            fatherLastName.ColumnName = "FatherLastName";
            patientsTable.Columns.Add(fatherLastName);

            DataColumn email = new DataColumn();
            email.DataType = Type.GetType("System.String");
            email.ColumnName = "Email";
            patientsTable.Columns.Add(email);

            DataColumn address = new DataColumn();
            address.DataType = Type.GetType("System.String");
            address.ColumnName = "Address";
            patientsTable.Columns.Add(address);

            DataColumn country = new DataColumn();
            country.DataType = Type.GetType("System.String");
            country.ColumnName = "Country";
            patientsTable.Columns.Add(country);

            DataColumn zipCode = new DataColumn();
            zipCode.DataType = Type.GetType("System.String");
            zipCode.ColumnName = "ZipCode";
            patientsTable.Columns.Add(zipCode);

            DataColumn phoneNumber1 = new DataColumn();
            phoneNumber1.DataType = Type.GetType("System.String");
            phoneNumber1.ColumnName = "PhoneNumber1";
            patientsTable.Columns.Add(phoneNumber1);

            DataColumn phoneNumber2 = new DataColumn();
            phoneNumber2.DataType = Type.GetType("System.String");
            phoneNumber2.ColumnName = "PhoneNumber2";
            patientsTable.Columns.Add(phoneNumber2);

            DataColumn phoneNumber3 = new DataColumn();
            phoneNumber3.DataType = Type.GetType("System.String");
            phoneNumber3.ColumnName = "PhoneNumber3";
            patientsTable.Columns.Add(phoneNumber3);

            DataColumn phoneNumberNote1 = new DataColumn();
            phoneNumberNote1.DataType = Type.GetType("System.String");
            phoneNumberNote1.ColumnName = "PhoneNumberNote1";
            patientsTable.Columns.Add(phoneNumberNote1);

            DataColumn phoneNumberNote2 = new DataColumn();
            phoneNumberNote2.DataType = Type.GetType("System.String");
            phoneNumberNote2.ColumnName = "PhoneNumberNote2";
            patientsTable.Columns.Add(phoneNumberNote2);

            DataColumn phoneNumberNote3 = new DataColumn();
            phoneNumberNote3.DataType = Type.GetType("System.String");
            phoneNumberNote3.ColumnName = "PhoneNumberNote3";
            patientsTable.Columns.Add(phoneNumberNote3);

            DataColumn genderLookup = new DataColumn();
            genderLookup.DataType = Type.GetType("System.String");
            genderLookup.ColumnName = "GenderLookup";
            patientsTable.Columns.Add(genderLookup);

            DataColumn languageLookup = new DataColumn();
            languageLookup.DataType = Type.GetType("System.String");
            languageLookup.ColumnName = "LanguageLookup";
            patientsTable.Columns.Add(languageLookup);

            DataColumn maritalStatusLookup = new DataColumn();
            maritalStatusLookup.DataType = Type.GetType("System.String");
            maritalStatusLookup.ColumnName = "MaritalStatusLookup";
            patientsTable.Columns.Add(maritalStatusLookup);

            DataColumn statusLookup = new DataColumn();
            statusLookup.DataType = Type.GetType("System.String");
            statusLookup.ColumnName = "StatusLookup";
            patientsTable.Columns.Add(statusLookup);

            DataColumn phoneNumberTypeLookup1 = new DataColumn();
            phoneNumberTypeLookup1.DataType = Type.GetType("System.String");
            phoneNumberTypeLookup1.ColumnName = "PhoneNumberTypeLookup1";
            patientsTable.Columns.Add(phoneNumberTypeLookup1);

            DataColumn phoneNumberTypeLookup2 = new DataColumn();
            phoneNumberTypeLookup2.DataType = Type.GetType("System.String");
            phoneNumberTypeLookup2.ColumnName = "PhoneNumberTypeLookup2";
            patientsTable.Columns.Add(phoneNumberTypeLookup2);

            DataColumn phoneNumberTypeLookup3 = new DataColumn();
            phoneNumberTypeLookup3.DataType = Type.GetType("System.String");
            phoneNumberTypeLookup3.ColumnName = "PhoneNumberTypeLookup3";
            patientsTable.Columns.Add(phoneNumberTypeLookup3);

            foreach (var item in patients)
            {
                DataRow row = patientsTable.NewRow();
                row["Id"] = item.Id;
                row["FirstName"] = item.FirstName;
                row["LastName"] = item.LastName;
                row["DateOfBirth"] = item.DateOfBirth;
                row["NAM"] = item.NAM;
                row["Note"] = item.Note;
                row["MotherFirstName"] = item.MotherFirstName;
                row["MotherLastName"] = item.MotherLastName;
                row["FatherFirstName"] = item.FatherFirstName;
                row["FatherLastName"] = item.FatherLastName;
                row["Email"] = item.Email;
                row["Address"] = item.Address;
                row["Country"] = item.Country;
                row["ZipCode"] = item.ZipCode;
                row["PhoneNumber1"] = item.PhoneNumber1;
                row["PhoneNumber2"] = item.PhoneNumber2;
                row["PhoneNumber3"] = item.PhoneNumber3;
                row["PhoneNumberNote1"] = item.PhoneNumberNote1;
                row["PhoneNumberNote2"] = item.PhoneNumberNote2;
                row["PhoneNumberNote3"] = item.PhoneNumberNote3;
                row["GenderLookup"] = item.GenderLookup;
                row["LanguageLookup"] = item.LanguageLookup;
                row["MaritalStatusLookup"] = item.MaritalStatusLookup;
                row["StatusLookup"] = item.StatusLookup;
                row["PhoneNumberTypeLookup1"] = item.PhoneNumberTypeLookup1;
                row["PhoneNumberTypeLookup2"] = item.PhoneNumberTypeLookup2;
                row["PhoneNumberTypeLookup3"] = item.PhoneNumberTypeLookup3;
                row["Smoke"] = item.Smoke;
                row["SmokeInThePast"] = item.SmokeInThePast;
                row["TakeDrugs"] = item.TakeDrugs;

                patientsTable.Rows.Add(row);
            }

            patientsTable.AcceptChanges();

            return patientsTable;
        }

        public async Task<List<Doctor>> GetDoctorsByRamqIdAsync(string[] ramqIds)
        {
            var doctors = new List<Doctor>();
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["dictionaries"].ConnectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand();
                var parameters = command.AddArrayParameters(ramqIds, "ramqId");
                command.Connection = connection;
                command.CommandText = string.Format("SELECT RamqId, FirstName, LastName FROM Doctors WHERE RamqId IN ({0})", parameters);
                var reader = await command.ExecuteReaderAsync();
                while (reader.Read()) 
                {
                    doctors.Add(new Doctor()
                    {
                        RamqId = (int)reader[0],
                        FirstName = reader[1].ToString(),
                        LastName = reader[2].ToString(),
                    });
                }
            }
            
            return doctors;
        }
    }
}
