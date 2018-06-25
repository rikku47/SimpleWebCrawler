using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWC
{
    public class Config
    {
        public string lastUrlOrFile = "Url or file";
        public string lastSelectors = "h1";

        static string pathIncludeAssembly = System.Reflection.Assembly.GetExecutingAssembly().Location;
        static string path = System.IO.Path.GetDirectoryName(pathIncludeAssembly);

        public string outputPath = path + "\\out.txt";
    }
}
