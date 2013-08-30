using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

namespace KinectMouseControl
{
    class MoveMouseEvaluator
    {
        public double NeutralAreaSize { get; private set; }
        public Vector3D NeutralAreaCenter { get; private set; }

        public MoveMouseEvaluator(Vector3D neutralAreaCenter, double neutralAreaSize)
        {
            NeutralAreaSize = neutralAreaSize;
            NeutralAreaCenter = neutralAreaCenter;
        }

        public bool CanMoveMouse(Vector3D cursor)
        {
            Vector3D distanceVector = this.NeutralAreaCenter - cursor;
            bool isNotInNeutralArea = distanceVector.Length > this.NeutralAreaSize;
            return isNotInNeutralArea;
        }
    }
}
