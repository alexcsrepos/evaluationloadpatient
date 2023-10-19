using Evaluation_LoadPatient.Lookups;
using System.Collections.Generic;

namespace Evaluation_LoadPatient.Cache
{
    internal class LookupDataLanguage : Lookup
    {
        protected override string LookupType => "LANGUAGE";

        protected override Dictionary<string, string> LookupDescriptionDictionary => new Dictionary<string, string>();
    }
}
