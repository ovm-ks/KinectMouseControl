using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using KinectMouseControl.Exceptions;
using Microsoft.Kinect;

namespace KinectMouseControl
{
    class SkeletonHelper
    {
        private readonly SkeletonFrameReadyEventArgs _skeletonFrameEventArgs;

        public Skeleton Skeleton { get; private set; }

        public SkeletonHelper(SkeletonFrameReadyEventArgs skeletonFrameEventArgs, int player)
        {
            _skeletonFrameEventArgs = skeletonFrameEventArgs;
            this.Skeleton = this.GetSkeletonForPlayer(player);
        }

        private Skeleton GetSkeletonForPlayer(int player)
        {
            Skeleton[] skeletons;

            using (SkeletonFrame skeletonFrame = this._skeletonFrameEventArgs.OpenSkeletonFrame())
            {
                if (skeletonFrame == null)
                {
                    throw new SkeletonFrameDataNotAvailableException();
                }

                skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                skeletonFrame.CopySkeletonDataTo(skeletons);
            }

            var tracked = skeletons
                .Where(skeleton => skeleton.TrackingState == SkeletonTrackingState.Tracked);

            if (!tracked.Any())
            {
                throw new Exception("No players found!");
            }

            return tracked.ElementAtOrDefault(player);
        }

        public Joint? GetTrackedJoint(JointType type)
        {
            IEnumerable<Joint> joints = this.Skeleton.Joints.Where(
                        j => j.JointType == type &&
                             j.TrackingState == JointTrackingState.Tracked);

            if (joints.Any())
            {
                return joints.First();
            }
            else
            {
                return null;
            }
        }
    }
}
