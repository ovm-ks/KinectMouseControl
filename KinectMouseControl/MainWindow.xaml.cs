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

            this.MouseController = new MouseController(5000);


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

            Joint? rightHand = skeletonHelper.GetTrackedJoint(JointType.HandRight);
            Joint? hipCenter = skeletonHelper.GetTrackedJoint(JointType.HipCenter);            

            if (rightHand != null &&
                hipCenter != null)
            {
                Vector3D rightHandVector = rightHand.ToVector3D();
                Vector3D hipCenterVector = hipCenter.ToVector3D();

                MoveMouseEvaluator moveMouseEvaluator = new MoveMouseEvaluator(hipCenterVector, 0.35);
                bool canMoveMouse = moveMouseEvaluator.CanMoveMouse(rightHandVector);

                if (canMoveMouse)
                {
                    if (this.ResetMouseControllerRequired)
                    {
                        this.MouseController.ResetInitialPositions(
                            rightHandVector,
                            System.Windows.Forms.Cursor.Position.ToVector());

                        this.ResetMouseControllerRequired = false;
                    }                    

                    this.Out.Text = "true";

                    Vector newMousePos = this.MouseController.GetAbsoluteMousePosition(rightHandVector);
                    System.Windows.Forms.Cursor.Position = newMousePos.ToPoint();

                    //int curPosX = (int) (SystemParameters.VirtualScreenWidth*2.5*rightHand.Value.Position.X);
                    //int curPosY = (int) (SystemParameters.VirtualScreenHeight*2.5*rightHand.Value.Position.Y);

                    //Out.Text = string.Format("X: {0}{3}Y: {1}{3}Z: {2}",
                    //    rightHand.Value.Position.X.ToString("##.##"),
                    //    rightHand.Value.Position.Y.ToString("##.##"),
                    //    rightHand.Value.Position.Z.ToString("##.##"),
                    //    Environment.NewLine);

                    //Native.SetCursorPos(
                    //    (int)newMousePos.X,
                    //    (int)newMousePos.Y);
                }
                else
                {
                    this.ResetMouseControllerRequired = true;
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
