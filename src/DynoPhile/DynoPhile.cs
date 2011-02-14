using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;

namespace DynoPhile
{
    public class DynoPhileReader
    {
        private bool quoted;
        private string[] keys;

        public IList<dynamic> ReadFile(string filename, string delimitor)
        {

            var lines = File.ReadAllLines(filename).ToList();

            var header = lines.First();

            var result = new List<dynamic>();

            keys = (BuildResultType(header, delimitor) as IDictionary<string, object>).Keys.ToArray();

            for (var i = 1; i < lines.Count; i++)
            {
                var newItem = BuildResultType(header, delimitor);
                var p = newItem as IDictionary<string, object>;

                var fields = lines[i].Split(new[] { delimitor }, StringSplitOptions.None).ToList();

                AssignLineValues(fields, p);

                result.Add(newItem);
            }

            return result;
        }

        private void AssignLineValues(IList<string> fields, IDictionary<string, object> newItem)
        {
            for (var index = 0; index < fields.Count; index++)
            {
                var field = fields[index];
                if (quoted)
                    field = field.Replace("\"", string.Empty);

                newItem[keys[index]] = field;
            }
        }

        private dynamic BuildResultType(string header, string delimitor)
        {
            dynamic result = new ExpandoObject();
            var p = result as IDictionary<string, Object>;

            quoted = header.First() == '"';

            var fields = header.Split(new[] { delimitor }, StringSplitOptions.None).ToList();

            for (var index = 0; index < fields.Count; index++)
            {
                var field = fields[index];
                if (quoted)
                    field = field.Replace("\"", string.Empty);

                p[field.Replace(" ", string.Empty)] = string.Empty;
            }

            return result;
        }
    }
}