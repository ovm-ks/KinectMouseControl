using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using Microsoft.Kinect;

namespace KinectMouseControl
{
    static class JointExtensions
    {
        public static Vector3D ToVector3D(this Joint? joint)
        {
            return ToVector3D(joint.Value);
        }

        public static Vector3D ToVector3D(this Joint joint)
        {
            Vector3D vector = new Vector3D(
                        joint.Position.X,
                        joint.Position.Y,
                        joint.Position.Z);
            return vector;
        }
    }
}
