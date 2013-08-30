using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;

namespace KinectMouseControl
{
    static class FormPointExtensions
    {
        public static Vector ToVector(this System.Drawing.Point point)
        {
            return new Vector(point.X, point.Y);
        }

        public static System.Drawing.Point ToPoint(this Vector vector)
        {
            return new System.Drawing.Point((int)vector.X, (int)vector.Y);
        }
    }
}
