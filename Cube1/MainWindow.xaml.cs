using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Media.Media3D;
using System.Windows.Media.Animation;
using System.Configuration;

namespace Cube1
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }



        MeshGeometry3D MSquare()
        {
            MeshGeometry3D square = new MeshGeometry3D();
            Point3DCollection corners = new Point3DCollection();
            corners.Add(new Point3D(0.5, 0.5, 0.5));
            corners.Add(new Point3D(0.5, -0.5, 0.5));
            corners.Add(new Point3D(0.5, -0.5, -0.5));
            corners.Add(new Point3D(0.5, 0.5, -0.5));
            square.Positions = corners;
            Int32[] indices = {
            0,1,2,
            2,3,0
            };
            Int32Collection Triangles = new Int32Collection();
            foreach (Int32 index in indices)
            {
                Triangles.Add(index);
            }
            square.TriangleIndices = Triangles;
            return square;
        }




        MeshGeometry3D MCube(double cx=0.0, double cy=0.0, double cz=0.0, double s=1.0)
        {
            double r = s / 2.0;
            MeshGeometry3D cube = new MeshGeometry3D();
            Point3DCollection corners = new Point3DCollection();
            // 0
            corners.Add(new Point3D(cx + r, cy + r, cz + r));
            // 1
            corners.Add(new Point3D(cx - r, cy + r, cz + r));
            // 2
            corners.Add(new Point3D(cx - r, cy - r, cz + r));
            // 3
            corners.Add(new Point3D(cx + r, cy - r, cz + r));
            // 4
            corners.Add(new Point3D(cx + r, cy + r, cz - r));
            // 5
            corners.Add(new Point3D(cx - r, cy + r, cz - r));
            // 6
            corners.Add(new Point3D(cx - r, cy - r, cz - r));
            // 7
            corners.Add(new Point3D(cx + r, cy - r, cz - r));
            cube.Positions = corners;
            
            Int32[] indices ={
            //front
              0,1,2,
              0,2,3,
            //back
              4,7,6,
              4,6,5,
            //Right
              4,0,3,
              4,3,7,
           //Left
              1,5,6,
              1,6,2,
           //Top
              1,0,4,
              1,4,5,
           //Bottom
              2,6,7,
              2,7,3
            };

            
            Int32Collection Triangles = new Int32Collection();
            foreach (Int32 index in indices)
            {
                Triangles.Add(index);
            }
            cube.TriangleIndices = Triangles;
            return cube;            
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            // ==== XYZ Axes ==================================================================
            GeometryModel3D ox_3D, oy_3D, oz_3D;
            ox_3D = new GeometryModel3D();
            oy_3D = new GeometryModel3D();
            oz_3D = new GeometryModel3D();

            Prism ox = new Prism(0.0, 0.0, 0.0, 1.00, 0.01, 0.01);
            Prism oy = new Prism(0.0, 0.0, 0.0, 0.01, 1.00, 0.01);
            Prism oz = new Prism(0.0, 0.0, 0.0, 0.01, 0.01, 1.00);

            ox_3D.Geometry = ox.prism_mesh;
            oy_3D.Geometry = oy.prism_mesh;
            oz_3D.Geometry = oz.prism_mesh;
            ox_3D.Material = new DiffuseMaterial(new SolidColorBrush(Colors.Red));
            oy_3D.Material = new DiffuseMaterial(new SolidColorBrush(Colors.Green));
            oz_3D.Material = new DiffuseMaterial(new SolidColorBrush(Colors.Blue));
            // ===============================================================================

            SquareCentred kwadracik_1 = new SquareCentred(1.00, 0.50, 1.00, 1, "xy");
            SquareCentred kwadracik_2 = new SquareCentred(1.00, 1.00, 0.50, 1, "xz");
            SquareCentred kwadracik_3 = new SquareCentred(0.50, 0.50, 0.50, 1, "zy");

           GeometryModel3D Square_1 = new GeometryModel3D();
            Square_1.Geometry = kwadracik_1.square_mesh;
            Square_1.Material = new DiffuseMaterial(
               new SolidColorBrush(Colors.Indigo));
            Square_1.BackMaterial = new DiffuseMaterial(
               new SolidColorBrush(Colors.Indigo));

            GeometryModel3D Square_2 = new GeometryModel3D();
            Square_2.Geometry = kwadracik_2.square_mesh;
            Square_2.Material = new DiffuseMaterial(
               new SolidColorBrush(Colors.Gold));
            Square_2.BackMaterial = new DiffuseMaterial(
               new SolidColorBrush(Colors.Gold));

            GeometryModel3D Square_3 = new GeometryModel3D();
            Square_3.Geometry = kwadracik_3.square_mesh;
            Square_3.Material = new DiffuseMaterial(
               new SolidColorBrush(Colors.Beige));
            Square_3.BackMaterial = new DiffuseMaterial(
               new SolidColorBrush(Colors.Beige));


            DirectionalLight DirLight1 = new DirectionalLight();
            DirLight1.Color = Colors.White;
            DirLight1.Direction = new Vector3D(-1, -1, -1);
            
            PerspectiveCamera Camera1 = new PerspectiveCamera();
            Camera1.FarPlaneDistance = 20;
            Camera1.NearPlaneDistance = 1;
            Camera1.FieldOfView = 45;
            Camera1.Position = new Point3D(2, 2, 2);
            Camera1.LookDirection = new Vector3D(-2, -2, -2);
            Camera1.UpDirection = new Vector3D(0, 0, 1);
            
            Model3DGroup modelGroup = new Model3DGroup();
            modelGroup.Children.Add(ox_3D);
            modelGroup.Children.Add(oy_3D);
            modelGroup.Children.Add(oz_3D);
            modelGroup.Children.Add(Square_1);
            modelGroup.Children.Add(Square_2);
            modelGroup.Children.Add(Square_3);
            modelGroup.Children.Add(DirLight1);
            ModelVisual3D modelsVisual = new ModelVisual3D();
            modelsVisual.Content = modelGroup;
            
            Viewport3D myViewport = new Viewport3D();
            myViewport.Camera = Camera1;
            myViewport.Children.Add(modelsVisual);
            this.Canvas1.Children.Add(myViewport);
            myViewport.Height = 500;
            myViewport.Width = 500;
            Canvas.SetTop(myViewport, 0);
            Canvas.SetLeft(myViewport, 0);
            this.Width = myViewport.Width;
            this.Height = myViewport.Height;

            AxisAngleRotation3D axis = new AxisAngleRotation3D(
                  new Vector3D(0, 0, 1), 0);
            RotateTransform3D Rotate = new RotateTransform3D(axis);
            Square_1.Transform = Rotate;
            Square_2.Transform = Rotate;
            Square_3.Transform = Rotate;
            /*
            ox_3D.Transform = Rotate;
            oy_3D.Transform = Rotate;
            oz_3D.Transform = Rotate;
            */
            DoubleAnimation RotAngle = new DoubleAnimation();
            RotAngle.From = 0;
            RotAngle.To = 360;
            RotAngle.Duration = new Duration(
                              TimeSpan.FromSeconds(5.0));
            RotAngle.RepeatBehavior = RepeatBehavior.Forever;

            NameScope.SetNameScope(Canvas1, new NameScope());
            Canvas1.RegisterName("cubeaxis", axis);

            Storyboard.SetTargetName(RotAngle, "cubeaxis");
            Storyboard.SetTargetProperty(RotAngle,
             new PropertyPath(AxisAngleRotation3D.AngleProperty));
            Storyboard RotCube = new Storyboard();
            RotCube.Children.Add(RotAngle);
            RotCube.Begin(Canvas1);
        }
    }

}
