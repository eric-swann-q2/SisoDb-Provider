﻿using SisoDb.Dac;
using SisoDb.Structures.Schemas;

namespace SisoDb.DbSchema
{
    public interface IDbSchemaManager
    {
    	void ClearCache();
        void RemoveFromCache(string structureSchemaName);
    	void RemoveFromCache(IStructureSchema structureSchema);

        void DropStructureSet(IStructureSchema structureSchema, IDbClient dbClient);
        void UpsertStructureSet(IStructureSchema structureSchema, IDbClient dbClient);
    }
}