# SimpleSql

[![Build Status](https://travis-ci.org/oBears/SimpleSql.svg?branch=master)](https://travis-ci.org/oBears/SimpleSql)

dotnet core 2.0 ORM


nuget

``Install-Package SimpleSql-Core``


目前只支持mysql

asp.net core 



        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSimpleSql(opts =>opts.UseMysql(Configuration.GetConnectionString("default")).UseMirgation());
        }
    

``UseMysql`` 使用mysql 数据库

``UseMirgation`` 启用数据迁移功能（code frist） 目前不支持字段的修改，只支持没有的表创建

简单示例：


     [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : Controller
    {
        public IDbConnection conn;

        public UsersController(IDbConnection connection)
        {
            conn = connection;
        }
        [HttpGet("{id}")]
        public User Get(int id)
        {
            return conn.CreateQuery<User>().FirstOrDefault(x => x.Id == id);
        }
        [HttpPost]
        public void Create(User user)
        {
            conn.Insert(user).Execute();
        }

        [HttpGet]
        public List<User> GetList()
        {
            return conn.CreateQuery<User>().ToList();
        }
    }

