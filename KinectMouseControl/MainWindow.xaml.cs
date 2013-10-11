using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Media3D;
using Microsoft.Kinect;
using Microsoft.Samples.Kinect.WpfViewers;

namespace KinectMouseControl
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MouseController MouseController { get; set; }
        public bool ResetMouseControllerRequired { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            this.ResetMouseControllerRequired = true;
        }        

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            KinectSensor mySensor = KinectSensor.KinectSensors.First();

            if (mySensor.Status != KinectStatus.Connected)
            {
                throw new Exception("No kinect connected!");
            }

            this.MouseController = new MouseController(2000);


            mySensor.SkeletonFrameReady += MySensorOnSkeletonFrameReady;
            mySensor.Start();

            EnableGui(mySensor);

            Out.Text = "";
        }

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

            Joint? leftHand = skeletonHelper.GetTrackedJoint(JointType.HandLeft);
            Joint? rightHand = skeletonHelper.GetTrackedJoint(JointType.HandRight);
            Joint? hipCenter = skeletonHelper.GetTrackedJoint(JointType.HipCenter);            

            string debugOut = "";

            if (rightHand != null &&
                hipCenter != null &&
                leftHand != null)
            {
                Vector3D rightHandVector = rightHand.ToVector3D();
                Vector3D hipCenterVector = hipCenter.ToVector3D();
                Vector3D leftHandVector = leftHand.ToVector3D();

                MoveMouseEvaluator moveMouseEvaluator = new MoveMouseEvaluator(hipCenterVector, 0.25);
                bool canMoveMouse = moveMouseEvaluator.CanMoveMouse(rightHandVector);

                debugOut += "Move Mouse: "; 

                if (canMoveMouse)
                {
                    if (this.ResetMouseControllerRequired)
                    {
                        this.MouseController.ResetInitialPositions(
                            rightHandVector,
                            System.Windows.Forms.Cursor.Position.ToVector());

                        this.ResetMouseControllerRequired = false;
                    }

                    debugOut += "true";

                    Vector newMousePos = this.MouseController.GetAbsoluteMousePosition(rightHandVector);
                    System.Windows.Forms.Cursor.Position = newMousePos.ToPoint();
                }
                else
                {
                    this.ResetMouseControllerRequired = true;
                    debugOut += "false";
                }

                debugOut += Environment.NewLine;

                MouseClickEvaluator clickEvaluator = new MouseClickEvaluator(hipCenterVector, 0.20);
                bool isLeftButtonDown = clickEvaluator.IsLeftButtonPressed(leftHandVector);

                debugOut += "LButton: ";

                if (isLeftButtonDown)
                {
                    MouseClicker.SetMouseLeftButtonDown();
                    debugOut += "true";
                }
                else
                {
                    MouseClicker.SetMouseLeftButtonUp();
                    debugOut += "false";
                }

                this.Out.Text = debugOut;
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
