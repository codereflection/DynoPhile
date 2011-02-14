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
        private string delimitor;

        /// <summary>
        /// Imports a csv file, one that does not have headers, you'll create your own with resultTypeBuilder
        /// </summary>
        /// <param name="filename">File to import</param>
        /// <param name="delimitor">Delimitor, such as ,</param>
        /// <param name="resultTypeBuilder">Func to call that will build out a new dynamic object to add values too</param>
        /// <returns>Unicorns and rainbows, or an exception if something screws up</returns>
        public IList<dynamic> ReadFile(string filename, string delimitor, Func<dynamic> resultTypeBuilder)
        {
            this.delimitor = delimitor;
            var lines = File.ReadAllLines(filename).ToList();

            var result = new List<dynamic>();

            keys = (resultTypeBuilder.Invoke() as IDictionary<string, object>).Keys.ToArray();

            return DoWork(lines, result, resultTypeBuilder, false);
        }

        /// <summary>
        /// Imports a csv file with headers
        /// </summary>
        /// <param name="filename">File to import</param>
        /// <param name="delimitor">Delimitor, such as ,</param>
        /// <returns>Unicorns and rainbows, or an exception if something screws up</returns>
        public IList<dynamic> ReadFile(string filename, string delimitor)
        {
            this.delimitor = delimitor;
            var lines = File.ReadAllLines(filename).ToList();

            var header = lines.First();

            var result = new List<dynamic>();

            keys = (BuildResultType(header) as IDictionary<string, object>).Keys.ToArray();

            return DoWork(lines, result, () => BuildResultType(header), true);
        }

        private IList<dynamic> DoWork(IList<string> lines, List<dynamic> result, Func<dynamic> resultTypeBuilder, bool hasHeader)
        {

            for (var i = hasHeader ? 1 : 0; i < lines.Count; i++)
            {
                var newItem = resultTypeBuilder.Invoke();
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

        private dynamic BuildResultType(string header)
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