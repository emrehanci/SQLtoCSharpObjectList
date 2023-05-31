using System.Reflection;
using System.Text;

namespace SQLtoCSharpObjectList
{
    public static class ProcessFile
    {
        public static void Process(string fileName, object transformedClass, string nameOfClass, bool isPrintConsole)
        {
            using StreamReader reader = new(@"../../../Data/" + fileName);
            string? line;
            List<string> proccesedLines = new();

            while ((line = reader.ReadLine()) != null)
            {
                ProcessLine(line, transformedClass, nameOfClass, proccesedLines, reader.EndOfStream);
            }

            if (isPrintConsole)
            {
                PrintToConsole(proccesedLines);
            }
            else
            {
                PrintToFile(proccesedLines, nameOfClass);
            }
        }

        public static string[] GetParsedLine(string line)
        {
            var temp = line.Remove(0, 1);
            if (temp[^1] == ',')
            {
                return temp.Remove(temp.Length - 2, 2).Split(", ");
            }
            else
            {
                return temp.Remove(temp.Length - 1, 1).Split(", ");
            }
        }

        public static void ProcessLine(string line, object transformedClass, string nameOfClass, List<string> proccesedLine, bool isLastLine)
        {
            int index = 0;
            var lineArr = GetParsedLine(line);
            var transformedLine = new List<string>() { "new " + nameOfClass + " {" };

            foreach (PropertyInfo prop in transformedClass.GetType().GetProperties())
            {
                var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                if (type == typeof(string))
                {
                    transformedLine.Add(prop.Name + " = " + ConvertToString(lineArr[index]));
                }
                else if (type == typeof(short))
                {
                    transformedLine.Add(prop.Name + " = " + ConvertToShort(lineArr[index]));
                }
                else if (type == typeof(int))
                {
                    transformedLine.Add(prop.Name + " = " + ConvertToInt(lineArr[index]));
                }
                else if (type == typeof(bool))
                {
                    transformedLine.Add(prop.Name + " = " + ConvertToBool(lineArr[index]));
                }
                else if (type == typeof(DateTime))
                {
                    transformedLine.Add(prop.Name + " = " + ConvertToDateTime(ConvertToString(lineArr[index])));
                }
                index++;
            }

            if (isLastLine)
            {
                transformedLine.Add(" }");
            }
            else
            {
                transformedLine.Add(" },");
            }

            proccesedLine.Add(PrepareLine(transformedLine));
        }

        public static string ConvertToString(string value)
        {
            StringBuilder sb = new(value);
            sb[0] = '\"';
            sb[value.Length - 1] = '\"';
            return sb.ToString();
        }

        public static string ConvertToShort(string value)
        {
            return "(short)" + value;
        }

        public static string ConvertToInt(string value)
        {
            return value;
        }

        public static string ConvertToBool(string value)
        {
            return value == "1" ? "true" : "false";
        }

        public static string ConvertToDateTime(string value)
        {
            return "DateTime.ParseExact(" + value + ", \"yyyy-MM-dd\", new CultureInfo(\"en-US\"))";
        }

        public static string PrepareLine(List<string> transformedLine)
        {
            string line = "";
            for (int i = 0; i < transformedLine.Count; i++)
            {
                if (i == 0)
                {
                    line += (transformedLine[i] + " ");
                }
                else if (i > 0 && i < transformedLine.Count - 2)
                {
                    line += (transformedLine[i] + ", ");
                }
                else
                {
                    line += (transformedLine[i]);
                }
            }

            line += ("\n");
            return line;
        }

        public static void PrintToFile(List<string> proccesedLines, string nameOfClass)
        {
            using StreamWriter writer = new(@"../../../Data/" + nameOfClass + ".txt");
            foreach (var proccesedLine in proccesedLines)
            {
                writer.Write(proccesedLine);
            }
        }

        public static void PrintToConsole(List<string> proccesedLines)
        {
            foreach (var proccesedLine in proccesedLines)
            {
                Console.Write(proccesedLine);
            }
        }
    }
}
