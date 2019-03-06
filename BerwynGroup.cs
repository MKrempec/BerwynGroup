using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic.FileIO;
namespace BerwynGroup
{
    class Program
    {
        static void writeToCSV(List<String> GUIDS, List<String> GUIDSduplicates, int [] combo, int [] val3s, int average )
        {
            String[] GUID = GUIDS.ToArray<String>();

            Console.Write("Please specify output file path:");
            String filepath = Console.ReadLine();

            using (StreamWriter csv = new StreamWriter(new FileStream(filepath,
                FileMode.Create, FileAccess.Write)))
            {
                csv.WriteLine("\"GUID\",\"Val1+Val2\",\"isDuplicateGuid(Y or N)\",\"isVal3GreaterThanAverageLength(Y or N)\"");
                for (int i = 0; i < combo.Length; i++)
                {
                    csv.Write("\"" + GUID[i] + "\",");
                    csv.Write("\"" + combo[i] + "\",");
                    if (GUIDSduplicates.Contains(GUID[i]))
                    {
                        csv.Write("\"Y\",");
                    }
                    else
                    {
                        csv.Write("\"N\",");
                    }
                    if(val3s[i] > average)
                    {
                        csv.WriteLine("\"Y\"");
                    }
                    else
                    {
                        csv.WriteLine("\"N\"");
                    }
                }
            }
        }
        static void parse_file(String filepath)
        {
            using (TextFieldParser parser = new TextFieldParser(filepath))
            {
                var count = File.ReadLines(filepath).Count() - 1 ;
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                parser.HasFieldsEnclosedInQuotes = true;
                string[] fields = parser.ReadFields();

                var max = 0;
                var maxGUID = "";
                var countVal3 = 0;
                List<String> GUIDS = new List<String>();
                List<String> GUIDSduplicates = new List<String>();
                int[] combo = new int[count];
                int[] val3s = new int[count];

                for (int i = 0; i < count; i++)
                {
                    {
                        fields = parser.ReadFields();

                        var val1 = Int32.Parse(fields[1]);
                        var val2 = Int32.Parse(fields[2]);
                        var GUID = fields[0];

                        if (val1 + val2 > max)
                        {
                            max = val1 + val2;
                            maxGUID = GUID;
                        }
                        combo[i] = (val1 + val2);

                        if (GUIDS.Contains(GUID))
                        {
                            GUIDSduplicates.Add(GUID);
                            GUIDS.Add(GUID);
                        }
                        else
                        {
                            GUIDS.Add(GUID);
                        }
                        var val3 = fields[3].Length;
                        val3s[i] = (val3);
                        countVal3 += val3;
                    }
                }
                var GUIDSarray = GUIDSduplicates.ToArray<string>();

                Console.WriteLine("Count of Records: " + count);
                Console.WriteLine("Max Val1 + Val2: " + max);
                Console.WriteLine("Max Val1 + Val2 GUID: " + maxGUID);
                Console.WriteLine("Average Val3 Length: " + (countVal3 / count));
                Console.WriteLine("Duplicate GUIDS:");
                for(int i =0; i < GUIDSarray.Length; i++)
                {
                    Console.WriteLine(GUIDSarray[i]);
                }

                writeToCSV(GUIDS, GUIDSduplicates, combo, val3s, (countVal3 / count));
            }
           
        }
        static void Main(string[] args)
        {
            Console.Write("Please enter the file path for test.csv: ");
            var filepath = Console.ReadLine();
            parse_file(filepath);

        }
    }
}
