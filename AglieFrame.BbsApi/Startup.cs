using AglieFrame.Dapper;
using AglieFrame.JWT;
using AglieFrame.NoSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Panda.DynamicWebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AglieFrame.BbsApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AglieFrame.BbsApi", Version = "v1" });

                // 动态api展示Swagger 要有,不然可以调用,但是不展示
                c.DocInclusionPredicate((docName, description) => true);
            });
            // 注入动态api，还要引入Service项目
            services.AddDynamicWebApi();

            //跨域服务
            services.AddCors(opt => opt.AddPolicy("any", o => o.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));

            #region JWT HS256 对称加密
            //生成token服务
            services.Configure<JwtOptions>(this.Configuration.GetSection("JwtOptions"));
            services.AddScoped<IJwtService, JwtHS256Service>();
            //验证token服务
            JwtOptions jwtOptions = new JwtOptions();
            Configuration.Bind("JwtOptions", jwtOptions);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)//Scheme
                .AddJwtBearer(opts =>
                {
                    opts.TokenValidationParameters = new TokenValidationParameters
                    {
                        //JWT有一些默认的属性，就是给鉴权时就可以筛选了
                        ValidateIssuer = false,//是否验证Issuer
                        ValidateAudience = false,//是否验证Audience
                        ValidateLifetime = false,//是否验证失效时间
                        ValidateIssuerSigningKey = false,//是否验证SecurityKey
                        ValidAudience = jwtOptions.Audience,//
                        ValidIssuer = jwtOptions.Issuer,//Issuer，这两项和前面签发jwt的设置一致
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecurityKey)),//拿到SecurityKey
                    };
                });
            //授权策略
            services.AddAuthorization(opts =>
            {
                opts.AddPolicy("AdminPolicy",
                    policyBuilder => policyBuilder.RequireUserName("sun"));
            });
            #endregion

            #region Mysql + Dapper
            // 配置DbConnectFactory的IOptionsMonitor
            services.Configure<MySqlConnOption>(this.Configuration.GetSection("MySqlConn"));
            // 注册DbConnectFactory
            services.AddScoped<DbConnectFactory, DbConnectFactory>();
            // 注册DbService
            services.AddScoped<IDbService, DbService>();
            #endregion

            #region Redis, ElasticSearch
            services.AddScoped<RedisService>();
            services.AddScoped<ElasticSearchService>();
            #endregion

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddLog4Net();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AglieFrame.BbsApi v1"));
            }

            app.UseRouting();

            app.UseCors();//使用跨域

            app.UseAuthentication();//中间件管道执行身份认证

            app.UseAuthorization();//授权

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
