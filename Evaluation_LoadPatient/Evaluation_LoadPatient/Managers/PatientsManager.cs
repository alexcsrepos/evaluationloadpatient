using Evaluation_LoadPatient.Cache;
using Evaluation_LoadPatient.Constants;
using Evaluation_LoadPatient.Helpers;
using Evaluation_LoadPatient.Models;
using Evaluation_LoadPatient.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Evaluation_LoadPatient.Managers
{
    internal class PatientsManager
    {
        private readonly PatientsRepository _patientsRepository;
        private readonly LookupDataGender _lookupDataGender;
        private readonly LookupDataLanguage _lookupDataLanguage;
        private readonly LookupDataMaritalStatus _lookupDataMaritalStatus;
        private readonly LookupDataPhoneType _lookupDataPhoneType;
        private readonly LookupDataStatus _lookupDataStatus;
        public PatientsManager()
        {
            _patientsRepository = new PatientsRepository();
            _lookupDataGender = new LookupDataGender();
            _lookupDataLanguage = new LookupDataLanguage();
            _lookupDataMaritalStatus = new LookupDataMaritalStatus();
            _lookupDataPhoneType = new LookupDataPhoneType();
            _lookupDataStatus = new LookupDataStatus();
        }

        public async Task<MigrationResponse> MigratePatientsAsync(string fileName, string dataSource, string database)
        {
            var response = new MigrationResponse();
            try
            {
                DefaultConnectionStringCache.SetConnectionString(dataSource, database);
                List<Patient> patients = _patientsRepository.GetPatients(fileName);
                var doctorRamqIds = patients
                    .Where(patient => int.TryParse(patient.Id03, out int tmp))
                    .Select(patient => patient.Id03)
                    .Distinct()
                    .ToArray();

                List<Doctor> doctors = await _patientsRepository
                    .GetDoctorsByRamqIdAsync(doctorRamqIds);

                response.Warnings.AddRange(await ValidatePatientRecord(patients, doctors));

                await _patientsRepository.InsertPatientsAsync(patients);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Errors = new string[] { ex.Message };
            }
            return response;
        }

        private async Task<List<string>> ValidatePatientRecord(List<Patient> patients, List<Doctor> doctors)
        {
            var warnings = new List<string>();
            var doctorsDictionary = doctors.ToDictionary(doctor => doctor.RamqId.ToString(), doctor => string.Format("{0} {1}", doctor.FirstName, doctor.LastName));
            foreach (Patient patient in patients)
            {
                if (!DataValidationHelper.ValidateDate(patient.DateOfBirth))
                {
                    patient.DateOfBirth = DateTime.Now;
                    warnings.Add(string.Format(ErrorMessages.GenericError + ErrorMessages.FieldCastingError + ErrorMessages.ManualValidationRequired, patient.Id, nameof(patient.DateOfBirth)));
                }

                if (!string.IsNullOrEmpty(patient.Id03))
                {
                    if (doctorsDictionary.ContainsKey(patient.Id03))
                        patient.Id03 = doctorsDictionary[patient.Id03];
                    else
                        warnings.Add(string.Format(ErrorMessages.GenericError + ErrorMessages.DoctorNotFound, patient.Id, patient.Id03));
                }

                RiskFactorsHelper.SetRiskFactor(patient);

                if (!string.IsNullOrEmpty(patient.Sex))
                {
                    var genderLookup = await _lookupDataGender.GetLookupIdByDescriptionAsync(patient.Sex);
                    if (genderLookup.HasValue)
                        patient.GenderLookup = genderLookup;
                    else
                        warnings.Add(string.Format(ErrorMessages.GenericError + ErrorMessages.LookupNotFound, patient.Id, nameof(patient.Sex)));
                }

                if (!string.IsNullOrEmpty(patient.Language))
                {
                    var languageLookup = await _lookupDataLanguage.GetLookupIdByDescriptionAsync(patient.Language);
                    if (languageLookup.HasValue)
                        patient.LanguageLookup = languageLookup;
                    else
                        warnings.Add(string.Format(ErrorMessages.GenericError + ErrorMessages.LookupNotFound, patient.Id, nameof(patient.Language)));
                }

                if (!string.IsNullOrEmpty(patient.MaritalStatus))
                {
                    var maritalStatusLookup = await _lookupDataMaritalStatus.GetLookupIdByDescriptionAsync(patient.MaritalStatus);
                    if (maritalStatusLookup.HasValue)
                        patient.MaritalStatusLookup = maritalStatusLookup;
                    else
                        warnings.Add(string.Format(ErrorMessages.GenericError + ErrorMessages.LookupNotFound, patient.Id, nameof(patient.MaritalStatus)));
                }

                if (!string.IsNullOrEmpty(patient.Status))
                {
                    var statusLookup = await _lookupDataStatus.GetLookupIdByDescriptionAsync(patient.Status);
                    if (statusLookup.HasValue)
                        patient.StatusLookup = statusLookup;
                    else
                        warnings.Add(string.Format(ErrorMessages.GenericError + ErrorMessages.LookupNotFound, patient.Id, nameof(patient.Status)));
                }

                if (!string.IsNullOrEmpty(patient.PhoneNumber1) && !string.IsNullOrEmpty(patient.PhoneNumberNote1))
                {
                    var phoneNote = SanitizePhoneNote(patient.PhoneNumberNote1);
                    var statusLookup = await _lookupDataPhoneType.GetLookupIdByDescriptionAsync(phoneNote);
                    if (statusLookup.HasValue)
                        patient.PhoneNumberTypeLookup1 = statusLookup;
                    else
                        warnings.Add(string.Format(ErrorMessages.GenericError + ErrorMessages.LookupNotFound, patient.Id, nameof(patient.PhoneNumberNote1)));
                }

                if (!string.IsNullOrEmpty(patient.PhoneNumber2) && !string.IsNullOrEmpty(patient.PhoneNumberNote2))
                {
                    var phoneNote = SanitizePhoneNote(patient.PhoneNumberNote2);
                    var statusLookup = await _lookupDataPhoneType.GetLookupIdByDescriptionAsync(phoneNote);
                    if (statusLookup.HasValue)
                        patient.PhoneNumberTypeLookup2 = statusLookup;
                    else
                        warnings.Add(string.Format(ErrorMessages.GenericError + ErrorMessages.LookupNotFound, patient.Id, nameof(patient.PhoneNumberNote2)));
                }

                if (!string.IsNullOrEmpty(patient.PhoneNumber3) && !string.IsNullOrEmpty(patient.PhoneNumberNote3))
                {
                    var phoneNote = SanitizePhoneNote(patient.PhoneNumberNote3);
                    var statusLookup = await _lookupDataPhoneType.GetLookupIdByDescriptionAsync(phoneNote);
                    if (statusLookup.HasValue)
                        patient.PhoneNumberTypeLookup3 = statusLookup;
                    else
                        warnings.Add(string.Format(ErrorMessages.GenericError + ErrorMessages.LookupNotFound, patient.Id, nameof(patient.PhoneNumberNote3)));
                }
            }

            return warnings;
        }

        private string SanitizePhoneNote(string phoneNote)
        {
            if (string.IsNullOrEmpty(phoneNote))
                return phoneNote;

            var sanitizerRegex = new Regex("[A-zÀ-ú]+");
            return sanitizerRegex.Match(phoneNote).Value;
        }
    }
}
