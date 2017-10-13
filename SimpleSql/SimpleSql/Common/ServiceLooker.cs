using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace SimpleSql.Common
{
    public static class ServiceLooker
    {
        public static readonly ConcurrentDictionary<Type, Type> TransientConfig = new ConcurrentDictionary<Type, Type>();
        public static readonly ConcurrentDictionary<Type, Type> SingletonConfig = new ConcurrentDictionary<Type, Type>();
        public static readonly ConcurrentDictionary<Type, object> SingletonCache = new ConcurrentDictionary<Type, object>();
        public static void AddTransient<TInterface, TImplement>()
        {
            TransientConfig.TryAdd(typeof(TInterface), typeof(TImplement));
        }
        public static void AddSingleton<TInterface, TImplement>()
        {
            SingletonConfig.TryAdd(typeof(TInterface), typeof(TImplement));
        }
        public static TInterface GetService<TInterface>()
        {
            Type type;
            if (SingletonConfig.TryGetValue(typeof(TInterface), out type))
            {
                object obj;
                if (SingletonCache.TryGetValue(type, out obj))
                    return (TInterface)obj;
                obj = Activator.CreateInstance(type);
                SingletonCache[type] = obj;
                return (TInterface)obj;
            }
            if (TransientConfig.TryGetValue(typeof(TInterface), out type))
                return (TInterface)Activator.CreateInstance(type);
            throw new Exception("未找到对应的类型");
        }
    }
}
