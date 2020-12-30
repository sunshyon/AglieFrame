using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AglieFrame.Dapper
{
    public class DbConnectFactory
    {
        private readonly MySqlConnOption mySqlConnOption;
        public DbConnectFactory(IOptionsMonitor<MySqlConnOption> optionsMonitor)
        {
            this.mySqlConnOption = optionsMonitor.CurrentValue;
        }
        /// <summary>
        /// 默认获取主库
        /// </summary>
        public IDbConnection GetDbConnection()
        {
            return new MySqlConnection(mySqlConnOption.Write);
        }
        /// <summary>
        /// 获取查询库（从库）
        /// </summary>
        public IDbConnection GetDbQueryConnection()
        {
            //随机实现负载均衡
            var index = new Random().Next(1, 1000) % mySqlConnOption.Read.Length;
            return new MySqlConnection(mySqlConnOption.Read[index]);
        }
    }
}
