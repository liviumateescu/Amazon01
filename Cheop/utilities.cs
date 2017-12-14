using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using Cheop.Models;
using Cheop.Exceptions;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;

namespace Cheop.Util
{
    public static class Utilities
    {
        public static bool AllUnique(List<drum> drumuri)
        {
            List<string> drumuriString = new List<string>();
            List<string> drumuriInversString = new List<string>();
            foreach (drum d in drumuri)
            {
                drumuriString.Add(d.ToString());
                drumuriInversString.Add(d.inverseToString());
            }
            var intersect = drumuriString.Intersect(drumuriInversString);
            bool toateUnice = !drumuriString.GroupBy(n => n).Any(c => c.Count() > 1) & intersect.Count().Equals(0);
            if (!toateUnice)
            {
                throw new RepetaAutostradaException("O autostrada se repeta!");
            }

            return toateUnice;
        }

        public static T CloneJson<T>(this T source)
        {
            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            // initialize inner objects individually
            // for example in default constructor some list property initialized with some values,
            // but in 'source' these items are cleaned -
            // without ObjectCreationHandling.Replace default constructor values will be added to result
            var deserializeSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source), deserializeSettings);
        }

        public static T Clone<T>(this T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

    }

}
