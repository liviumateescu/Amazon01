using System;
using System.Collections.Generic;
using System.Text;

namespace Cheop.Models
{
    public class drum
    {
        public int oras1;
        public int oras2;

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
