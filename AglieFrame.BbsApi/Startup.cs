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

                // ��̬apiչʾSwagger Ҫ��,��Ȼ���Ե���,���ǲ�չʾ
                c.DocInclusionPredicate((docName, description) => true);
            });
            // ע�붯̬api����Ҫ����Service��Ŀ
            services.AddDynamicWebApi();

            //�������
            services.AddCors(opt => opt.AddPolicy("any", o => o.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));

            #region JWT HS256 �ԳƼ���
            //����token����
            services.Configure<JwtOptions>(this.Configuration.GetSection("JwtOptions"));
            services.AddScoped<IJwtService, JwtHS256Service>();
            //��֤token����
            JwtOptions jwtOptions = new JwtOptions();
            Configuration.Bind("JwtOptions", jwtOptions);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)//Scheme
                .AddJwtBearer(opts =>
                {
                    opts.TokenValidationParameters = new TokenValidationParameters
                    {
                        //JWT��һЩĬ�ϵ����ԣ����Ǹ���Ȩʱ�Ϳ���ɸѡ��
                        ValidateIssuer = false,//�Ƿ���֤Issuer
                        ValidateAudience = false,//�Ƿ���֤Audience
                        ValidateLifetime = false,//�Ƿ���֤ʧЧʱ��
                        ValidateIssuerSigningKey = false,//�Ƿ���֤SecurityKey
                        ValidAudience = jwtOptions.Audience,//
                        ValidIssuer = jwtOptions.Issuer,//Issuer���������ǰ��ǩ��jwt������һ��
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecurityKey)),//�õ�SecurityKey
                    };
                });
            //��Ȩ����
            services.AddAuthorization(opts =>
            {
                opts.AddPolicy("AdminPolicy",
                    policyBuilder => policyBuilder.RequireUserName("sun"));
            });
            #endregion

            #region Mysql + Dapper
            // ����DbConnectFactory��IOptionsMonitor
            services.Configure<MySqlConnOption>(this.Configuration.GetSection("MySqlConn"));
            // ע��DbConnectFactory
            services.AddScoped<DbConnectFactory, DbConnectFactory>();
            // ע��DbService
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

            app.UseCors();//ʹ�ÿ���

            app.UseAuthentication();//�м���ܵ�ִ�������֤

            app.UseAuthorization();//��Ȩ

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
