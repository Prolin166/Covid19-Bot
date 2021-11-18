using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CovidBot.Services
{
    public static class TextFileWorker
    {
        public static List<long> ReadLongToList(string file)
        {
            List<long> list = new List<long>();
            var reader = new StreamReader(file);
            string item = reader.ReadLine();
            list.Add(long.Parse(item));
            while (item != null)
            {
                item = reader.ReadLine();
                if (item != null)
                    list.Add(long.Parse(item));
            }
            reader.Close();

            return list;
        }

        public static void AppendTextFileWithLong(string file, long id)
        {
            using (StreamWriter writer = System.IO.File.AppendText(file))
            {
                writer.WriteLine(id);
            }
        }

        public static void WriteLineToTextFile(string file, string text)
        {
            StreamWriter writer = new StreamWriter(file);
            writer.WriteLine(text);
        }
    }
}
