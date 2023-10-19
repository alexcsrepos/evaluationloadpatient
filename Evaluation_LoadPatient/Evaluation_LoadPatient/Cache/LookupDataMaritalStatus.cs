using Evaluation_LoadPatient.Lookups;
using System.Collections.Generic;

namespace Evaluation_LoadPatient.Cache
{
    internal class LookupDataMaritalStatus : Lookup
    {
        protected override string LookupType => "MARITALSTATUS";

        protected override Dictionary<string, string> LookupDescriptionDictionary => new Dictionary<string, string>
        {
            { "Divorce", "Divorced" },
            { "Marié", "Married" },
            { "Conjoint de fait", "Common-Law Union" },
            { "Voeuf", "Widowed" }
        };
    }
}
