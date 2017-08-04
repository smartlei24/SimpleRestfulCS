using System;
using System.Collections.Generic;
using System.Configuration;

namespace ASimpleHttPServer
{
    public class ResouceRoute
    {
        public static Dictionary<string,string> ResouceDictionary { get; }

        public bool InitializeResouceRoute()
        {
            ResouceDictionary.Clear();
            foreach (var resouce in ConfigurationManager.AppSettings.AllKeys)
            {
                ResouceDictionary.Add(resouce, ConfigurationManager.AppSettings[resouce]);
            }
            return true;
        }
    }
}
