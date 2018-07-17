using SWC.Classes;
using System;
using System.IO;
using System.Text;
using System.Windows;

namespace SWC
{
    class ExportFunctions
    {
        public static void ExportSelectorGroupToCSV(SelectorGroup selectorGroup)
        {
            int count = 0;
            StringBuilder stringBuilder = null;

            if (selectorGroup.ExportSelectorGroup)
            {
                stringBuilder = new StringBuilder("Gruppenname\n" + selectorGroup.Name + "\n\n");

                foreach (var selector in selectorGroup.Selectors)
                {
                    if (selector.Export)
                    {
                        stringBuilder.Append("Selektor;Resultate insgesamt\n");
                        stringBuilder.Append(selector.CSSSelector + ";" + selector.DateEntries.Count + "\n\n");

                        foreach (var dateEntry in selector.DateEntries)
                        {
                            stringBuilder.Append("Erstellt am: " + dateEntry.CreationDate + "\n");

                            foreach (var result in dateEntry.Results)
                            {
                                foreach (var footPrintAResult in result.FootPrintsAResult)
                                {
                                    if (true)
                                    {
                                        stringBuilder.Append(footPrintAResult.Text + ";");
                                    }

                                    if (true)
                                    {
                                        stringBuilder.Append(footPrintAResult.InnerHTML + ";");
                                    }

                                    if (true)
                                    {
                                        stringBuilder.Append(footPrintAResult.OuterHTML + ";");
                                    }

                                    stringBuilder.Append("\n");
                                }
                            }
                            stringBuilder.Append("\n");
                        }
                    }
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
