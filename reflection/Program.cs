using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace reflection
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var item in ReadCsv1<My>("airquality.csv"))
            {
                
                Console.WriteLine(item.Name + ' ' + item.Ozone);
            }
        }
        class My
        {
            //Name,"Ozone","Solar.R","Wind","Temp","Month","Day"
            string name;
            double ozone;
            int? solar;
            double? wind;
            int? temp;
            int? month;
            int? day;
            public string Name
            {
                get
                {
                    return name;
                }

                set
                {
                    name = value;
                }
            }

            public double Ozone
            {
                get
                {
                    return ozone;
                }

                set
                {
                    ozone = value;
                }
            }

            public int? Solar
            {
                get
                {
                    return solar;
                }

                set
                {
                    solar = value;
                }
            }

            public double? Wind
            {
                get
                {
                    return wind;
                }

                set
                {
                    wind = value;
                }
            }

            public int? Month
            {
                get
                {
                    return month;
                }

                set
                {
                    month = value;
                }
            }

            public int? Day
            {
                get
                {
                    return day;
                }

                set
                {
                    day = value;
                }
            }

            public int? Temp
            {
                get
                {
                    return temp;
                }

                set
                {
                    temp = value;
                }
            }
        }
        static IEnumerable<T> ReadCsv1<T>(string path) where T : new()
        {
            using (var textReader = new StreamReader(path))
            {
                var head = textReader.ReadLine();
                var columns = head.Split(',');
                Type type = typeof(T);

                foreach (var column in columns)
                {
                    var property = type.GetProperty(column.Trim('\"'));
                    if (property == null)
                        throw new NotImplementedException("Отсутствует подходящее свойство.");
                }
                while (true)
                {
                    var str = textReader.ReadLine();
                    if (str == null)
                        yield break;
                    T obj = new T();
                    string[] fields = str.Split(',');
                    int pos = 0;
                    foreach (var column in columns)
                    {
                        var property = type.GetProperty(column.Trim('\"'));
                        if (fields[pos] == "NA")
                        {
                            if (property.PropertyType == typeof(int) || property.PropertyType == typeof(double))
                                throw new ArgumentNullException("Пустое поле");
                            fields[pos] = null;
                        }
                        else
                        {
                            if (property.PropertyType == typeof(int) || property.PropertyType == typeof(int?))
                                property.SetValue(obj, Convert.ToInt32(fields[pos]));
                            else
                            if (property.PropertyType == typeof(double) || property.PropertyType == typeof(double?))
                                property.SetValue(obj, Convert.ToDouble(fields[pos], new CultureInfo("en-US")));
                            else
                            if (property.PropertyType == typeof(string))
                                property.SetValue(obj, fields[pos]);
                            else
                                throw new InvalidCastException("Не соответствие типов данных.");
                        }
                       

                        pos++;
                    }
                    yield return obj;
                }
            }
        }
    }
}
