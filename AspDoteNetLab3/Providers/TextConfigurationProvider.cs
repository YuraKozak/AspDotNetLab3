using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace AspDotNetLab3.Providers
{
    public class TextConfigurationProvider : ConfigurationProvider
    {
        public string FilePath { get; set; }
        public TextConfigurationProvider (string path)
        {
            FilePath = path;
        }

        public override void Load()
        {
            var data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
 
            using (FileStream fs = new FileStream(FilePath,FileMode.Open))
            {
                using (StreamReader textReader = new StreamReader(fs))
                {
                    string line;
                    while ((line=textReader.ReadLine())!= null)
                    {
                        string key = line.Trim();
                        string value = textReader.ReadLine();
                        data.Add(key, value);
                    }
                }
            }
            Data = data;
        }
    }
}
