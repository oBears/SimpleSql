using SimpleSql.Entity;
using System;
using System.Collections.Generic;
using System.Text;


namespace SimpleSql.Test.Model
{
    public class Student
    {
        [SqlColumn(Increment = true, Key = true)]
        public int Id { set; get; }
        public string Name { set; get; }
        public Gender Gender { set; get; }
        public int Age { set; get; }
        public DateTime? Birth { set; get; }
        public int ClassId { set; get; }
        [SqlIgnore]
        public ClassRoom ClassRoom { set; get; }
    }

    public enum Gender
    {
        男,
        女
    }

}
