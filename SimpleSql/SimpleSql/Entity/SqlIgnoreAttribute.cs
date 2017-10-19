using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleSql.Entity
{
    [AttributeUsage(AttributeTargets.Property,AllowMultiple =false,Inherited =false)]
    public class SqlIgnoreAttribute:Attribute
    {


    }
}
