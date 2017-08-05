using System;
using System.Collections.Generic;
using System.Configuration;
using NUnit.Framework.Internal;

namespace ASimpleHttpServer
{
    public class ResouceRoute
    {
        public static Dictionary<string,string> ResouceDictionary { get; } = new Dictionary<string, string>();

        static public bool InitializeResouceRoute()
        {
            ResouceDictionary.Clear();
            foreach (var resouce in ConfigurationManager.AppSettings.AllKeys)
            {
                ResouceDictionary.Add(resouce, ConfigurationManager.AppSettings[resouce].Trim());
            }
            return true;
        }

    }
}
