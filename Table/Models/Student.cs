using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Table.Models
{
    public class Student
    {
        private int[] points = { 0, 0, 0, 0, 0 };
        private string name = "";

        public string Name
        {
            get => name;
            set => name = value;
        }

        public int Math
        {
            get => points[0];
            set => points[0] = value;
        }

        public int Russian
        {
            get => points[1];
            set => points[1] = value;
        }

        public int Programming
        {
            get => points[2];
            set => points[2] = value;
        }

        public int Physical_culture
        {
            get => points[3];
            set => points[3] = value;
        }

        public int History
        {
            get => points[4];
            set => points[4] = value;
        }

        public double Average_point
        {
            get;
            set;
        }
    }
}

