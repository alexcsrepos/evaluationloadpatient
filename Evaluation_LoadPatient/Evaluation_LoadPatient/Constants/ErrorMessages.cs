namespace Evaluation_LoadPatient.Constants
{
    internal static class ErrorMessages
    {
        public const string GenericError = "Error while processing the patient with PatientGuid: {0}, ";
        public const string FieldCastingError = "It was not possible to cast the value of the field: {1}";
        public const string ManualValidationRequired = "The record has been migrated but manual validation is required";
        public const string DoctorNotFound = "Doctor with RamqId {1} not found";
        public const string LookupNotFound = "Lookup related to the field {1} not found";
    }
}
