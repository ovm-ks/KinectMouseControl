using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

namespace KinectMouseControl
{
    class MouseControlEvaluator
    {
        public double NeutralAreaSize { get; private set; }
        public Vector3D NeutralAreaCenter { get; private set; }

        public MouseControlEvaluator(Vector3D neutralAreaCenter, double neutralAreaSize)
        {
            NeutralAreaSize = neutralAreaSize;
            NeutralAreaCenter = neutralAreaCenter;
        }

        public bool IsMoved(Vector3D cursor)
        {
            Vector3D distanceVector = this.NeutralAreaCenter - cursor;
            bool isInNeutralArea = distanceVector.Length > this.NeutralAreaSize;
            return isInNeutralArea;
        }

        public bool IsMoving()
        {
            
        }
    }
}
