using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    public class DataFile
    {
        public string filename;
        public DataFile()
        {
        }
        public static List<UserInfo> DataFileContent(string filename)
        {
            if (File.Exists(filename))
            {
                List<UserInfo> users = JsonConvert.DeserializeObject<List<UserInfo>>(File.ReadAllText(filename));
                return users;
            }
            return null;
        }
    }
}
