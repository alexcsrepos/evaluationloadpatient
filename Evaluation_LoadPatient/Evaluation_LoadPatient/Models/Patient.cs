using System;

namespace Evaluation_LoadPatient.Models
{
    internal class Patient
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Sex { get; set; }
        public string Language { get; set; }
        public string MaritalStatus { get; set; }
        public string Status { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string NAM { get; set; }
        public DateTime? NamExpiryDate { get; set; }
        public string Note { get; set; }
        public string MotherFirstName { get; set; }
        public string MotherLastName { get; set; }
        public string FatherFirstName { get; set; }
        public string FatherLastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string Id03 { get; set; }
        public string PhoneNumber1 { get; set; }
        public string PhoneNumber2 { get; set; }
        public string PhoneNumber3 { get; set; }
        public string PhoneNumberNote1 { get; set; }
        public string PhoneNumberNote2 { get; set; }
        public string PhoneNumberNote3 { get; set; }
        public Guid? GenderLookup { get; set; }
        public Guid? LanguageLookup { get; set; }
        public Guid? MaritalStatusLookup { get; set; }
        public Guid? StatusLookup { get; set; }
        public Guid? PhoneNumberTypeLookup1 { get; set; }
        public Guid? PhoneNumberTypeLookup2 { get; set; }
        public Guid? PhoneNumberTypeLookup3 { get; set; }
        public string Smoke {  get; set; }
        public string SmokeInThePast { get; set; }
        public string TakeDrugs { get; set; }

    }
}
