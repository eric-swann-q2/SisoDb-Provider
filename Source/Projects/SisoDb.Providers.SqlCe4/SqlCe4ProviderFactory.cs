﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using PineCone.Structures.Schemas;
using SisoDb.Dac;
using SisoDb.Dac.BulkInserts;
using SisoDb.DbSchema;
using SisoDb.Providers;
using SisoDb.Querying;
using SisoDb.Querying.Lambdas.Converters.Sql;
using SisoDb.Querying.Lambdas.Parsers;
using SisoDb.SqlCe4.Dac;
using SisoDb.Structures;

namespace SisoDb.SqlCe4
{
    public class SqlCe4ProviderFactory : ISisoProviderFactory
    {
        private readonly Lazy<ISqlStatements> _sqlStatements;

        private readonly ConcurrentDictionary<string, IDbConnection> _connections; 

        public SqlCe4ProviderFactory()
        {
            _sqlStatements = new Lazy<ISqlStatements>(() => new SqlCe4Statements());
            _connections = new ConcurrentDictionary<string, IDbConnection>();
        }

        ~SqlCe4ProviderFactory()
        {
            var exceptions = new List<Exception>();

            foreach (var key in _connections.Keys)
            {
                try
                {
                    IDbConnection cn;
                    if (_connections.TryRemove(key, out cn))
                    {
                        if (cn != null)
                        {
                            if (cn.State != ConnectionState.Closed)
                                cn.Close();

                            cn.Dispose();
                        }

                        cn = null;
                    }
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Count > 0)
                throw new SisoDbException("Exceptions occured while releasing SqlCe4Connections from the pool.", exceptions);
        }

        public StorageProviders ProviderType
        {
            get { return StorageProviders.SqlCe4; }
        }

        public IDbConnection GetOpenServerConnection(IConnectionString connectionString)
        {
            var cn = new SqlCeConnection(connectionString.PlainString);
            cn.Open();
            
            return cn;
        }

        public void ReleaseServerConnection(IDbConnection dbConnection)
        {
            if (dbConnection == null)
                return;

            if (dbConnection.State != ConnectionState.Closed)
                dbConnection.Close();

            dbConnection.Dispose();
        }

        public IDbConnection GetOpenConnection(IConnectionString connectionString)
        {
            Func<string, IDbConnection> cnFactory = (cnString) =>
            {
                var cn = new SqlCeConnection(cnString);
                cn.Open();
                return cn;
            };

            return _connections.GetOrAdd(connectionString.PlainString, cnFactory);
        }

        public void ReleaseConnection(IDbConnection dbConnection)
        {
        }

        public virtual IServerClient GetServerClient(ISisoConnectionInfo connectionInfo)
        {
            return new SqlCe4ServerClient((SqlCe4ConnectionInfo)connectionInfo);
        }

        public IDbClient GetTransactionalDbClient(ISisoConnectionInfo connectionInfo)
        {
            return new SqlCe4DbClient(connectionInfo, true);
        }

        public IDbClient GetNonTransactionalDbClient(ISisoConnectionInfo connectionInfo)
        {
            return new SqlCe4DbClient(connectionInfo, false);
        }

        public virtual IDbSchemaManager GetDbSchemaManager()
        {
            return new DbSchemaManager();
        }

        public virtual IDbSchemaUpserter GetDbSchemaUpserter(IDbClient dbClient)
        {
            return new SqlDbSchemaUpserter(dbClient);
        }

        public virtual ISqlStatements GetSqlStatements()
        {
            return _sqlStatements.Value;
        }

        public virtual IdentityStructureIdGenerator GetIdentityStructureIdGenerator(IDbClient dbClient)
        {
            return new IdentityStructureIdGenerator(dbClient);
        }

        public virtual IDbStructureInserter GetDbStructureInserter(IDbClient dbClient)
        {
            return new DbStructureInserter(dbClient);
        }

        public virtual IDbQueryGenerator GetDbQueryGenerator()
        {
            return new SqlCe4QueryGenerator(
                new LambdaToSqlWhereConverter(),
                new LambdaToSqlSortingConverter(),
                new LambdaToSqlIncludeConverter());
        }

        public virtual IGetCommandBuilder<T> CreateGetCommandBuilder<T>() where T : class
        {
            return new GetCommandBuilder<T>(
                new SortingParser(),
                new IncludeParser());
        }

        public virtual IQueryCommandBuilder<T> CreateQueryCommandBuilder<T>(IStructureSchema structureSchema) where T : class
        {
            return new QueryCommandBuilder<T>(
                structureSchema,
                new WhereParser(),
                new SortingParser(),
                new IncludeParser());
        }
    }
}