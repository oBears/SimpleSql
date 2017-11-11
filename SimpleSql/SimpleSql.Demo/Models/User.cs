using SimpleSql.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleSql.Demo.Models
{
    [SqlTable]
    public class User
    {
        [SqlColumn(Key =true,Increment =true)]
        public int Id { set; get; }
        public string UserName { set; get; }
        public string Gender { set; get; }
        [SqlIgnore]
        public string Email { set; get; }
    }
}
