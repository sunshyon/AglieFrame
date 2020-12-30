using AglieFrame.Dapper;
using AglieFrame.Service.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AglieFrame.Service
{
    [EnableCors("any")]//跨域特性
    [Authorize]
    [ActionFilter]
    public class BaseService
    {
        public IDbService _dbService { get; set; }
    }
}
