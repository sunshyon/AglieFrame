using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AglieFrame.Dapper
{

    public class DbService : IDbService
    {
        private readonly DbConnectFactory _dbConnFactory;

        public DbService(DbConnectFactory dbConnectFactory)
        {
            this._dbConnFactory = dbConnectFactory;
        }
        public long Insert<T>(T param) where T : class
        {
            using (IDbConnection conn=_dbConnFactory.GetDbConnection())
            {
                return conn.Insert<T>(param);
            }
        }
        public bool Update<T>(T param) where T : class
        {
            using(IDbConnection conn = _dbConnFactory.GetDbConnection())
            {
                return conn.Update<T>(param);
            }
        }
        public bool Delete<T>(T param) where T : class
        {
            using (IDbConnection conn = _dbConnFactory.GetDbConnection())
            {
                return conn.Delete<T>(param);
            }
        }
        public IEnumerable<T> Query<T>() where T : class
        {
            using (IDbConnection conn = _dbConnFactory.GetDbQueryConnection())
            {
                return conn.GetAll<T>();
            }
        }

        public IDbConnection GetDbConnection()
        {
            return _dbConnFactory.GetDbConnection();
        }
       
    }
}
