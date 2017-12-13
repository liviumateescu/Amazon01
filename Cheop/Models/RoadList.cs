using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace Cheop.Models
{
    public class RoadList<T> : Collection<Road<T>>
    {
        public RoadList() : base() { }

        public RoadList(int intialSize)
        {
            //Add the specified number of items
            for (int i = 0; i < intialSize; i++)
            {
                base.Items.Add(default(Road<T>));
            }
        }

        /*public Road<T> FindByValue(T value)
        {
            //search the list for the value
            foreach (Road<T> road in Items)
                if (road.Value.Equals(value))
                    return road;
            //if we reached here, we didn't find a matching node
            return null;
        }*/

        public override string ToString()
        {
            string s = "RoadList: \n";
            foreach (Road<T> road in base.Items)
            {
                s = s + road.ToString() + "\n";
            }
            return s+"<<<<<<>>>>>>\n";
        }
    }
}