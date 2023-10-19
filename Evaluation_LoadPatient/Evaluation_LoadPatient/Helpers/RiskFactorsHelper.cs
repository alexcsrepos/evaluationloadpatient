using Evaluation_LoadPatient.Constants;
using Evaluation_LoadPatient.Models;

namespace Evaluation_LoadPatient.Helpers
{
    internal static class RiskFactorsHelper
    {
        public static void SetRiskFactor(Patient patient)
        {
            if (patient == null)
                return;

            if (patient.Smoke?.ToLower() == RiskFactors.AffirmativeResponse)
                patient.Smoke = RiskFactors.Smoker;
            else
                patient.Smoke = "";

            if (patient.SmokeInThePast?.ToLower() == RiskFactors.AffirmativeResponse)
                patient.SmokeInThePast = RiskFactors.SmokedInThePast;
            else
                patient.SmokeInThePast = "";

            if (patient.TakeDrugs?.ToLower() == RiskFactors.AffirmativeResponse)
                patient.TakeDrugs = RiskFactors.TakeDrugs;
            else
                patient.TakeDrugs = "";
        }
    }
}
