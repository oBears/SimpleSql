using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace TiORM
{
    public class DynamicParameters
    {
        private readonly Dictionary<string, ParamInfo> parameters = new Dictionary<string, ParamInfo>();
        public object this[string name]
        {
            get
            {
                return parameters[name];
            }
        }
        public void Add(string name, object value, DbType? dbType, ParameterDirection? direction)
        {
            parameters.Add(Clean(name), new ParamInfo()
            {
                ParameterName = name,
                DbType = dbType.GetValueOrDefault(DbType.String),
                Direction = direction.GetValueOrDefault(ParameterDirection.Input)
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
