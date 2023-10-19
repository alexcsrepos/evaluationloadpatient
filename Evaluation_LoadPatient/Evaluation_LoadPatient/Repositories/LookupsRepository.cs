using Evaluation_LoadPatient.Cache;
using Evaluation_LoadPatient.Lookups;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Evaluation_LoadPatient.Repositories
{
    internal class LookupsRepository<T>
        where T: Lookup, new()
    {
        public async Task<List<Lookup>> GetLookupDataAsync(string lookupType)
        {
            var lookupData = new List<Lookup>();
            using (SqlConnection connection = new SqlConnection(DefaultConnectionStringCache.GetConnectionString()))
            {
                await connection.OpenAsync();
                var command = new SqlCommand();
                command.Parameters.AddWithValue("@lookupType", lookupType);
                command.Connection = connection;
                command.CommandText = "SELECT Id, En FROM Lookup WHERE Type = @lookupType";
                var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    lookupData.Add(new T()
                    {
                        Id = (Guid)reader[0],
                        DescriptionEnglish = reader[1].ToString()
                    });
                }
            }

            return lookupData;
        }
    }
}
