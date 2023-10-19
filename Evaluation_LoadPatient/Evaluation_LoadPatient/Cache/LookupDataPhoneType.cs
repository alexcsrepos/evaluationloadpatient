using Evaluation_LoadPatient.Lookups;
using System.Collections.Generic;

namespace Evaluation_LoadPatient.Cache
{
    internal class LookupDataPhoneType : Lookup
    {
        protected override string LookupType => "PhoneType";

        protected override Dictionary<string, string> LookupDescriptionDictionary => new Dictionary<string, string>
        {
            { "cell", "Cell Phone" },
            { "cellulaire", "Cell Phone" },
            { "Cellulaire", "Cell Phone" },
            { "Maison", "Home" },
            { "Travail", "Work" },
            { "bureau",  "Work" }
        };
    }
}
