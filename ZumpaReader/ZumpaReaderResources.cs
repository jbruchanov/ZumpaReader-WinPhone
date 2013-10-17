using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ZumpaReader
{
    public class ZumpaReaderResources
    {
        public enum Keys
        {
            WebServiceURL,
            ZumpaPushRegisterURL,
            Login,
            Password,
            RemoteLogURL
        }

        private const string FILE_NAME = "ZumpaReaderResources.json";

        
        private Dictionary<string, string> _data;
        
        private ZumpaReaderResources()
        {
            _data = LoadData(FILE_NAME);
        }

        /// <summary>
        /// Load json file and return deserialized hashmap
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <returns></returns>
        private Dictionary<string, string> LoadData(string sourceFile)
        {
            Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            string location = String.Format("{0}.{1}", typeof(ZumpaReaderResources).Namespace, FILE_NAME);
            Stream stream = assembly.GetManifestResourceStream(location);
            if(stream == null)
            {
                throw new InvalidOperationException(FILE_NAME + " not found!");
            }
            StreamReader streamReader = new StreamReader(stream);
            
            string fileContent = streamReader.ReadToEnd();
            Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string,string>>(fileContent);
            
            return data;
        }

        private static ZumpaReaderResources _instance;
        public static ZumpaReaderResources Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new ZumpaReaderResources();
                }
                return _instance;
            }
            private set
            {
                _instance = value;
            }
        }

        public string this[Keys key]
        {
            get
            {
                return _data[key.ToString()];
            }
        }
    }
}
