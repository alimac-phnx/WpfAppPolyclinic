using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Wpf_server
{
    public class DataFile
    {
        public static List<UserInfo> DataFileContent(string filename)
        {
            if (File.Exists(filename))
            {
                List<UserInfo> users = JsonConvert.DeserializeObject<List<UserInfo>>(File.ReadAllText(filename));

                return users;
            }

            return null;
        }
        public static UserInfo SingleDataFileContent(string filename)
        {
            if (File.Exists(filename))
            {
                UserInfo userInfo = JsonConvert.DeserializeObject<UserInfo>(File.ReadAllText(filename));
                return userInfo;
            }

            return null;
        }
    }
}
