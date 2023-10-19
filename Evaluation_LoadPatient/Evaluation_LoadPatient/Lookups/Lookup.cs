using Evaluation_LoadPatient.Cache;
using Evaluation_LoadPatient.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Evaluation_LoadPatient.Lookups
{
    internal class Lookup
    {
        protected virtual string LookupType { get; }
        protected virtual Dictionary<string, string> LookupDescriptionDictionary { get; }
        protected Dictionary<string, Guid> LookupData;

        public Guid Id { get; set; }
        public string DescriptionEnglish { get; set; }

        private readonly LookupsRepository<LookupDataStatus> _lookupRepository;

        public Lookup()
        {
            _lookupRepository = new LookupsRepository<LookupDataStatus>();
        }

        public async Task<Guid?> GetLookupIdByDescriptionAsync(string description)
        {
            if (LookupData == null)
            {
                var lookups = await _lookupRepository.GetLookupDataAsync(LookupType);
                LookupData = lookups.ToDictionary(lookup => lookup.DescriptionEnglish, lookup => lookup.Id);
            }

            if (LookupDescriptionDictionary.ContainsKey(description))
                description = LookupDescriptionDictionary[description];

            if (LookupData.ContainsKey(description))
                return LookupData[description];

            return null;
        }
    }
}
