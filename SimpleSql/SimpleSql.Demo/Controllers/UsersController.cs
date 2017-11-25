using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleSql.Demo.Models;
using System.Data;
using SimpleSql.Query;

namespace SimpleSql.Demo.Controllers
{
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
            conn.Insert(user);
        }
        [HttpGet]
        public PageInfo<User> GetList()
        {
            return conn.CreateQuery<User>().ToPageList();
        }
    }
}