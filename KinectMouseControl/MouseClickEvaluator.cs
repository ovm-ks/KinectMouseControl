﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

namespace KinectMouseControl
{
    class MouseClickEvaluator
    {
        public double NeutralAreaSize { get; private set; }
        public Vector3D NeutralAreaCenter { get; private set; }

        public MouseClickEvaluator(Vector3D neutralAreaCenter, double neutralAreaSize)
        {
            NeutralAreaSize = neutralAreaSize;
            NeutralAreaCenter = neutralAreaCenter;
        }

        public bool IsLeftButtonPressed(Vector3D cursor)
        {
            double distance = this.NeutralAreaCenter.Z - cursor.Z;
            bool isNotInNeutralArea = distance > this.NeutralAreaSize;
            return isNotInNeutralArea;
        }

        public bool IsRightButtonPressed(Vector3D cursor)
        {
            double distance = this.NeutralAreaCenter.Z - cursor.Z;
            bool isNotInNeutralArea = distance > this.NeutralAreaSize;
            return isNotInNeutralArea;
        }
    }
}
