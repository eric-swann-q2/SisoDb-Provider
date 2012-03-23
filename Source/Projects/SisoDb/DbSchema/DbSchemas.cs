using System;
using System.Linq;
using NCore.Collections;
using PineCone.Structures.Schemas;

namespace SisoDb.DbSchema
{
    public static class DbSchemas
    {
        public static class Suffixes
        {
            public static readonly string[] AllSuffixes;
            public const string StructureTableNameSuffix = "Structure";
            public const string UniquesTableNameSuffix = "Uniques";
            public static readonly string[] IndexesTableNameSuffixes;

            static Suffixes()
            {
                IndexesTableNameSuffixes = Enum.GetNames(typeof(IndexesTypes));
                AllSuffixes = new[] { StructureTableNameSuffix, UniquesTableNameSuffix }.MergeWith(IndexesTableNameSuffixes).ToArray();
            }
        }

        public static class SysTables
        {
            public const string IdentitiesTable = "SisoDbIdentities";
        }

        public static string GetStructureTableName(this IStructureSchema structureSchema)
        {
            return GenerateStructureTableName(structureSchema.Name);
        }

        public static string GenerateStructureTableName(string structureName)
        {
            return string.Concat(DbSchemaNamingPolicy.StructureNameGenerator.Invoke(structureName), Suffixes.StructureTableNameSuffix);
        }

        public static IndexesTableNames GetIndexesTableNames(this IStructureSchema structureSchema)
        {
            return new IndexesTableNames(structureSchema);
        }

        public static string GenerateIndexesTableNameFor(string structureName, IndexesTypes type)
        {
            return string.Concat(DbSchemaNamingPolicy.StructureNameGenerator.Invoke(structureName), type.ToString());
        }

        public static string GetIndexesTableNameFor(this IStructureSchema structureSchema, IndexesTypes type)
        {
            return GenerateIndexesTableNameFor(structureSchema.Name, type);
        }

        public static string GetUniquesTableName(this IStructureSchema structureSchema)
        {
            return GenerateUniquesTableName(structureSchema.Name);
        }

        public static string GenerateUniquesTableName(string structureName)
        {
            return string.Concat(DbSchemaNamingPolicy.StructureNameGenerator.Invoke(structureName), Suffixes.UniquesTableNameSuffix);
        }
    }
}