using Evaluation_LoadPatient.Lookups;
using System.Collections.Generic;

namespace Evaluation_LoadPatient.Cache
{
    internal class LookupDataGender : Lookup
    {
        protected override string LookupType => "GENDER";

        protected override Dictionary<string, string> LookupDescriptionDictionary => new Dictionary<string, string>
        {
            { "F", "Female" },
            { "Féminin", "Female" },
            { "Female", "Female" },
            { "Feminin", "Female" },
            { "Femmme", "Female" }
        };
    }
}
