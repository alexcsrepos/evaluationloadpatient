namespace Evaluation_LoadPatient.Constants
{
    internal static class SqlConstants
    {
		public const string CreatePatientsTempTable = @"CREATE TABLE [#Patient](
			[Id] [nvarchar](64),
			[FirstName] [nvarchar](2000),
			[LastName] [nvarchar](2000),
			[DateOfBirth] [datetime],
			[NAM] [nvarchar](200),
            [NamExpiryDate] [datetime],
			[Identifier01] [nvarchar](2000),
			[Identifier02] [nvarchar](2000),
			[Identifier03] [nvarchar](2000),
			[Note] [nvarchar](max),
			[MotherFirstName] [nvarchar](2000),
			[MotherLastName] [nvarchar](2000),
			[FatherFirstName] [nvarchar](2000),
			[FatherLastName] [nvarchar](2000),
			[Email] [nvarchar](2000),
			[Address] [nvarchar](2000),
			[Country] [nvarchar](2000),
			[ZipCode] [nvarchar](2000),
			[PhoneNumber1] [nvarchar](2000),
			[PhoneNumber2] [nvarchar](2000),
			[PhoneNumber3] [nvarchar](2000),
			[PhoneNumberNote1] [nvarchar](2000),
			[PhoneNumberNote2] [nvarchar](2000),
			[PhoneNumberNote3] [nvarchar](2000),
			[GenderLookup] [nvarchar](64),
			[LanguageLookup] [nvarchar](64),
			[MaritalStatusLookup] [nvarchar](64),
			[StatusLookup] [nvarchar](64),
			[PhoneNumberTypeLookup1] [nvarchar](64),
			[PhoneNumberTypeLookup2] [nvarchar](64),
			[PhoneNumberTypeLookup3] [nvarchar](64),
            [Smoke] [nvarchar](max),
            [SmokeInThePast] [nvarchar](max),
            [TakeDrugs] [nvarchar](max)
		)";

        public const string InsertIntoPacientsTable = @"
            SELECT *
            INTO #NewPatients
            FROM #Patient
            WHERE [Id] NOT IN (SELECT Id FROM [Patient])

            INSERT INTO [dbo].[Patient]
               ([Id]
               , [FirstName]
               , [LastName]
               , [DateOfBirth]
               , [GenderLookup]
               , [LanguageLookup]
               , [MaritalStatusLookup]
               , [NAM]
               , [NAMExpiryDate]
               , [Identifier01]
               , [Identifier02]
               , [Identifier03]
               , [Note]
               , [Mother_FirstName]
               , [Mother_LastName]
               , [Father_FirstName]
               , [Father_LastName]
               , [Email]
               , [Address]
               , [Country]
               , [ZipCode]
               , [StatusLookup])
            SELECT
              [Id]
              ,[FirstName]
              ,[LastName]
              ,[DateOfBirth]
              ,[GenderLookup]
              ,[LanguageLookup]
              ,[MaritalStatusLookup]
              ,[NAM]
              ,[NAMExpiryDate]
              ,[Identifier01]
              ,[Identifier02]
              ,[Identifier03]
              ,[Note]
              ,[MotherFirstName]
              ,[MotherLastName]
              ,[FatherFirstName]
              ,[FatherLastName]
              ,[Email]
              ,[Address]
              ,[Country]
              ,[ZipCode]
              ,[StatusLookup]
            from #NewPatients";

        public const string InsertPhoneNumbers = @"
            CREATE TABLE #PhoneNumber(
	            [PatientId] [uniqueidentifier],
	            [Number] [nvarchar](2000),
	            [TypeLookup] [uniqueidentifier],
	            [Note] [nvarchar](max),
	            [IsPreferred] [bit]
            )
            INSERT INTO #PhoneNumber (PatientId, Number, TypeLookup, Note, IsPreferred)
            SELECT [Id], [PhoneNumber1], [PhoneNumberTypeLookup1], [PhoneNumberNote1], 1 FROM #NewPatients WHERE ISNULL([PhoneNumber1], '') <> ''
            UNION SELECT [Id], [PhoneNumber2], [PhoneNumberTypeLookup2], [PhoneNumberNote2], 0 FROM #NewPatients WHERE ISNULL([PhoneNumber2], '') <> ''
            UNION SELECT [Id], [PhoneNumber3], [PhoneNumberTypeLookup3], [PhoneNumberNote3], 0 FROM #NewPatients WHERE ISNULL([PhoneNumber3], '') <> ''

            INSERT INTO PhoneNumber(Id, PatientId, Number, TypeLookup, Note, IsPreferred)
            SELECT NEWID(), PatientId, Number, TypeLookup, Note, IsPreferred FROM #PhoneNumber";

        public const string InsertRiskFactors = @"
            CREATE TABLE #RiskFactor (PatientId UNIQUEIDENTIFIER, RiskFactor NVARCHAR(MAX))
            INSERT INTO #RiskFactor (PatientId, RiskFactor)
            SELECT Id, Smoke FROM #NewPatients WHERE ISNULL(Smoke, '') <> ''
            UNION SELECT Id, SmokeInThePast FROM #NewPatients WHERE ISNULL(SmokeInThePast, '') <> ''
            UNION SELECT Id, TakeDrugs FROM #NewPatients  WHERE ISNULL(TakeDrugs, '') <> ''

            INSERT INTO RiskFactor(Id, PatientId, RiskFactor)
            SELECT NEWID(), PatientId, RiskFactor FROM #RiskFactor";
    }
}
