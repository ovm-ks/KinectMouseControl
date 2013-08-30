using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect;
using Microsoft.Samples.Kinect.WpfViewers;

namespace KinectMouseControl
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            KinectSensor mySensor = KinectSensor.KinectSensors.First();

            if (mySensor.Status != KinectStatus.Connected)
            {
                throw new Exception("No kinect connected!");
            }

            mySensor.SkeletonFrameReady += MySensorOnSkeletonFrameReady;
            mySensor.Start();

            EnableGui(mySensor);

            Out.Text = "";
        }

        private int i;
        private void MySensorOnSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            SkeletonHelper skeletonHelper;
            try
            {
                skeletonHelper = new SkeletonHelper(e, 0);
            }
            catch
            {
                return;
            }

            Joint? rightHand = skeletonHelper.GetTrackedJoint(JointType.HandRight);
            Joint? hipCenter = skeletonHelper.GetTrackedJoint(JointType.HipCenter);            

            if (rightHand != null &&
                hipCenter != null)
            {
                Vector3D rightHandVector = rightHand.ToVector3D();
                Vector3D hipCenterVector = hipCenter.ToVector3D();

                NeutralCenterEvaluator neutralCenterEvaluator = new NeutralCenterEvaluator(hipCenterVector, 0.35);
                bool mouseRests = neutralCenterEvaluator.IsInNeutralArea(rightHandVector);

                if (mouseRests)
                {
                    this.Out.Text = "true";

                    int curPosX = (int)(SystemParameters.VirtualScreenWidth * 2.5 * rightHand.Value.Position.X);
                    int curPosY = (int)(SystemParameters.VirtualScreenHeight * 2.5 * rightHand.Value.Position.Y);

                    Out.Text = string.Format("X: {0}{3}Y: {1}{3}Z: {2}",
                        rightHand.Value.Position.X.ToString("##.##"),
                        rightHand.Value.Position.Y.ToString("##.##"),
                        rightHand.Value.Position.Z.ToString("##.##"),
                        Environment.NewLine);

                    Native.SetCursorPos(
                        curPosX,
                        curPosY);
                }
                else
                {
                    this.Out.Text = "false";
                }
            }
        }

        private void EnableGui(KinectSensor mySensor)
        {
            var sensorManager = new KinectSensorManager();
            sensorManager.KinectSensor = mySensor;
            sensorManager.SkeletonStreamEnabled = true;

            //this.ColorViewer.KinectSensorManager = sensorManager;
            this.SkeletonViewer.KinectSensorManager = sensorManager;
        }
    }
}
