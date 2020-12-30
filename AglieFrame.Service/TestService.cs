using AglieFrame.Dapper;
using AglieFrame.Interface;
using AglieFrame.JWT;
using AglieFrame.Model.DbModels;
using AglieFrame.NoSql;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Utility;

namespace AglieFrame.Service
{
    [AllowAnonymous]
    public class TestService : BaseService, ITestService
    {
        private readonly IJwtService _jwtService;
        private readonly RedisService _redis;
        private readonly ILogger<TestService> _logger;
        private readonly ElasticSearchService _es;

        public TestService(
            ILogger<TestService> logger,
            IDbService dbService,
            IJwtService jwtService,
            RedisService redisService,
            ElasticSearchService es)
        {
            this._logger = logger;
            base._dbService = dbService;
            this._jwtService = jwtService;
            this._redis = redisService;
            this._es = es;
        }
        [HttpGet]
        public void TestLog4()
        {
            _logger.LogError("Test");
        }
        [HttpGet]
        public bool TestEs()
        {
            var users= _es.Search<User>();
            var res = _es.Delete<User>(users[0]);
            var user = new User
            {
                Name = "张三",
                Phone = "1311255454554"
            };
             //_es.IndexDoc(user);
            return true;
        }
        [HttpGet]
        public bool TestRedis(string key,string value)
        {
            return _redis.StringSet(key, value,new TimeSpan(0,0,60));
        }

        [HttpGet]
        public string GetToken(string username)
        {
            var token = _jwtService.GetToken(username);
            return token;
        }
        [HttpGet]
        [Authorize(Policy = "AdminPolicy")]
        public List<User> GetUsers()
        {
            var a= _dbService.Query<User>().Where(x => x.Id == 1);
            var b = a.ToList();
            var users = _dbService.Query<User>().ToList();
            return users;
        }
        [HttpGet]
        public long AddUser(string name)
        {
            var u = new User
            {
                Name = name,
            };
            var res= _dbService.Insert<User>(u);
            return res;
        }
        [HttpDelete]
        public bool User(int id)
        {
            var u = _dbService.Query<User>().Where(x => x.Id == id).FirstOrDefault();
            return _dbService.Delete<User>(u);
        }
    }
}
