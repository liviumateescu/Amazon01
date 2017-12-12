using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using Cheop.Models;
using Cheop.Exceptions;

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

    }

}
