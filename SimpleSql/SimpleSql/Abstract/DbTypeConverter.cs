using SimpleSql.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleSql.Abstract
{
    public abstract class DbTypeConverter
    {
        public static Dictionary<Type, KeyValuePair<string, int>> dictionary = new Dictionary<Type, KeyValuePair<string, int>>()
        {
            { typeof(int),new KeyValuePair<string, int>("int",11) },
            { typeof(string),new KeyValuePair<string, int>("varchar",255) },
            { typeof(DateTime),new KeyValuePair<string, int>("datetime",0) },
            { typeof(double),new KeyValuePair<string, int>("double",0) },
            { typeof(float),new KeyValuePair<string, int>("float",0) },
            { typeof(bool),new KeyValuePair<string, int>("bit",1) },
        };
        public static KeyValuePair<string,int> Lookup(Type type)
        {
            if (dictionary.TryGetValue(type, out KeyValuePair<string, int> p))
                return p;
            throw new Exception($"无法找到类型{type.ToString()}所对用的数据库类型");
        }
    }
}
