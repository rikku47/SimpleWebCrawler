using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWC
{
    public class Config
    {
        public string lastUrlOrFile = "test";
        public string lastSelectors = "test";

        static string pathIncludeAssembly = System.Reflection.Assembly.GetExecutingAssembly().Location;
        static string path = System.IO.Path.GetDirectoryName(pathIncludeAssembly);

        public string outputPath = path + "\\out.txt";
    }
}
