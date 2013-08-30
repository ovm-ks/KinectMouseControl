using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Media3D;

namespace KinectMouseControl
{
    public class MouseController
    {
        public int Scale { get; private set; }

        public Vector3D InitialCursorPositon { get; private set; }
        public Vector InitialMousePostion { get; private set; }

        public MouseController(int scale)
        {
            Scale = scale;
        }

        public void ResetInitialPositions(Vector3D initialCursorPositon, Vector initialMousePostion)
        {
            // set the Z to 0 to exlcude it from any calculations
            initialCursorPositon.Z = 0;

            this.InitialCursorPositon = initialCursorPositon;
            this.InitialMousePostion = initialMousePostion;
        }

        public Vector GetAbsoluteMousePosition(Vector3D currentCursorPositon)
        {
            // set the Z to 0 to exlcude it from any calculations
            currentCursorPositon.Z = 0;

            Vector3D offset = currentCursorPositon - this.InitialCursorPositon;

            double newX = offset.X * this.Scale;
            double newY = offset.Y * this.Scale;

            newX += this.InitialMousePostion.X;
            newY = this.InitialMousePostion.Y - newY;

            Vector result = new Vector(newX, newY);
            return result;
        }
    }
}
