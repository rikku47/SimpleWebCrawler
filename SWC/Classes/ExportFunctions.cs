using System;
using System.IO;
using System.Text;
using System.Windows;

namespace SWC
{
    class ExportFunctions
    {
        public static void ExportSelectorGroupToCSV(Link.SelectorGroup selectorGroup)
        {
            int count = 0;

            StringBuilder stringBuilder = new StringBuilder("Gruppenname\n" + selectorGroup.Name + "\n\n");

            foreach (var selector in selectorGroup.Selectors)
            {
                stringBuilder.Append("Selektor;Resultate insgesamt\n");
                stringBuilder.Append(selector.CSSSelector + ";" + selector.Results.Count + "\n\n");

                foreach (var result in selector.Results)
                {
                    stringBuilder.Append("Erstellt am: " + result.CreationDate + "\n");

                    foreach (var item in result.Items)
                    {
                        if (true)
                        {
                            stringBuilder.Append(item.Details.Text + ";");
                        }

                        if (true)
                        {
                            stringBuilder.Append(item.Details.InnerHTML + ";");
                        }

                        if (true)
                        {
                            stringBuilder.Append(item.Details.OuterHTML + ";");
                        }

                        stringBuilder.Append("\n");
                    }

                    stringBuilder.Append("\n");
                }
            }
            count++;

            string exportPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\export";

            try
            {
                using (var sw = new StreamWriter(exportPath + "\\out" + selectorGroup.Name + ".csv"))
                {
                    sw.WriteLine(stringBuilder);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
