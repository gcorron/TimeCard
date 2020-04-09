﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeCard.Domain;

namespace TimeCard.Repo.Repos
{
    public class LookupRepo:BaseRepo
    {
        public LookupRepo(string connectionString) : base(connectionString)
        {

        }

        public Lookup GetLookupByDescr(string group, string descr)
        {
            using(var conn=GetOpenConnection())
            {
                return conn.QuerySingleOrDefault<Lookup>("sLookup", new { group, descr }, null, null, CommandType.StoredProcedure);
            }
        }

        public Lookup GetLookupByVal(string group, string val)
        {
            using (var conn = GetOpenConnection())
            {
                return conn.QuerySingleOrDefault<Lookup>("sLookup", new { group, val }, null, null, CommandType.StoredProcedure);
            }
        }

        public IEnumerable<Lookup> GetLookups(string group)
        {
            using (var conn = GetOpenConnection())
            {
                return conn.Query<Lookup>("sLookup", new { group }, null, true, null, CommandType.StoredProcedure);
            }
        }

    }
}