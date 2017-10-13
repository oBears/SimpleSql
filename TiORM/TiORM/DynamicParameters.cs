using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SimpleSql
{
    public class DynamicParameters
    {
        public readonly Dictionary<string, ParamInfo> parameters = new Dictionary<string, ParamInfo>();
        public object this[string name]
        {
            get
            {
                return parameters[name];
            }
        }
        public void Add(string name, object value, DbType? dbType=null, ParameterDirection? direction=null)
        {
            parameters.Add(Clean(name), new ParamInfo()
            {
                Name = name,
                DbType = dbType,
                Value=value,
                Direction = direction
            });
        }
        private static string Clean(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                switch (name[0])
                {
                    case '@':
                    case ':':
                    case '?':
                        return name.Substring(1);
                }
            }
            return name;
        }
    }
}
