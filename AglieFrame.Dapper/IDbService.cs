using System;
using System.Collections.Generic;
using System.Data;

namespace AglieFrame.Dapper
{
    public interface  IDbService
    {
        long Insert<T>(T param) where T : class;

        bool Update<T>(T param) where T : class;

        bool Delete<T>(T param) where T : class;

        IEnumerable<T> Query<T>() where T : class;

        IDbConnection GetDbConnection();
    }
}
