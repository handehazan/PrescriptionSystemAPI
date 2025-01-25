using MongoDB.Bson;
using MongoDB.Driver;
using prescriptionSystemApi.context;
using prescriptionSystemApi.model;

namespace prescriptionSystemApi.source.db
{
    public class MedicineAccess
    {
        private readonly IMongoCollection<Medicine> _medicineCollection;
        public MedicineAccess(noSqlContext context)
        {
            _medicineCollection = context.GetCollection<Medicine>("medicine");

        }

        public async Task<List<Medicine>> GetAllMedicinesAsync()
        {
            return await _medicineCollection.Find(_ => true).ToListAsync();
        }
        public async Task DeleteAllMedicinesAsync()
        {
            await _medicineCollection.DeleteManyAsync(FilterDefinition<Medicine>.Empty);
        }

        public async Task InsertMedicinesAsync(List<Medicine> medicines)
        {
            if (medicines?.Any() == true)
            {
                await _medicineCollection.InsertManyAsync(medicines);
            }
        }

        public async Task<List<string>> SearchMedicineNamesAsync(string query)
        {
            var filter = Builders<Medicine>.Filter.Regex(m => m.IlacAdi,
                new BsonRegularExpression(query, "i"));

            return await _medicineCollection.Find(filter)
                .Project(m => m.IlacAdi)
                .ToListAsync();
        }
    }
}
