using Evaluation_LoadPatient.Lookups;
using System.Collections.Generic;

namespace Evaluation_LoadPatient.Cache
{
    internal class LookupDataStatus : Lookup
    {
        protected override string LookupType => "STATUS";
        protected override Dictionary<string, string> LookupDescriptionDictionary => new Dictionary<string, string>();
    }
}
