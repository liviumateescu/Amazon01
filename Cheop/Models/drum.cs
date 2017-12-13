using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Cheop.Models
{
    public class drum
    {
        public int oras1;
        public int oras2;

       /* public drum(int o1, int o2)
        {
            this.oras1 = o1;
            this.oras2 = o2;
        }

        public drum(string o1, string o2)
        {
            this.oras1 = int.Parse(o1);
            this.oras2 = int.Parse(o2);
        }
        */
        public override string ToString()
        {
            return String.Format("{0} -> {1}",oras1,oras2);
        }

        public string inverseToString()
        {
            return String.Format("{0} -> {1}", oras2, oras1);
        }
    }
}
