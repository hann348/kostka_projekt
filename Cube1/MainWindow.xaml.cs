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
        private List<GeometryModel3D> squares;
        private GeometryModel3D ox_3D=null, oy_3D=null, oz_3D=null;
        private Model3DGroup all_to_show, cube_squares;
        private PerspectiveCamera Camera1 = null;
        private Point3D camPos;
        private Vector3D lookDir, upDir;
        private double azm, elev, depth;

        private double point_to_plane_dist(double A, double B, double C, double D, double x, double y, double z)
        {
            double res, top, bottom;

            top = (A * x) + (B * y) + (C * z) + D;
            bottom = Math.Sqrt((A * A) + (B * B) + (C * C));
            res = top / bottom;
            
            return Math.Abs(res);
        }



        private void CamSliderHorizontal_Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Camera1 == null)
            {
                return;
            }
            azm = e.NewValue;
            refresh_camera();

        }
        private void refresh_camera()
        {
            camPos.X = depth * Math.Sin(elev * (Math.PI / 180)) * Math.Cos(azm * (Math.PI / 180));
            camPos.Y = depth * Math.Sin(elev * (Math.PI / 180)) * Math.Sin(azm * (Math.PI / 180));
            camPos.Z = depth * Math.Cos(elev * (Math.PI / 180));
            lookDir.X = -camPos.X;
            lookDir.Y = -camPos.Y;
            lookDir.Z = -camPos.Z;
            upDir.X = 0;
            upDir.Y = 0;
            if(elev < 0)
            {
                upDir.Z = -1.0;
            }
            else 
            {
                upDir.Z = 1.0;
            }
            Camera1.Position = camPos;
            Camera1.LookDirection = lookDir;
            Camera1.UpDirection = upDir;
        }

        private void OX_minus_left_click(object sender, RoutedEventArgs e)
        {
            foreach (GeometryModel3D s in cube_squares.Children)
            {
                double currDist;
                Point3D cP;
                // from stack exchange: XXX
                MeshGeometry3D currMesh = s.Geometry as MeshGeometry3D;
                cP = currMesh.Positions[0];
                currDist = point_to_plane_dist(1, 0, 0, 0.35, cP.X, cP.Y, cP.Z);
                if(currDist <= 0.155)
                {
                    s.Material = new DiffuseMaterial(new SolidColorBrush(Colors.Black));
                    s.BackMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.Black));
                }
            }
        }

        private void OX_minus_right_click(object sender, RoutedEventArgs e)
        {
            foreach (GeometryModel3D s in cube_squares.Children)
            {
                double currDist;
                Point3D cP;
                MeshGeometry3D currMesh = s.Geometry as MeshGeometry3D;
                cP = currMesh.Positions[0];
                currDist = point_to_plane_dist(1, 0, 0, 0.35, cP.X, cP.Y, cP.Z);
                if (currDist <= 0.155)
                {
                    s.Material = new DiffuseMaterial(new SolidColorBrush(Colors.Black));
                    s.BackMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.Black));
                }
            }
        }

        private void OX_zero_left_click(object sender, RoutedEventArgs e)
        {
            foreach (GeometryModel3D s in cube_squares.Children)
            {
                double currDist;
                Point3D cP;
                MeshGeometry3D currMesh = s.Geometry as MeshGeometry3D;
                cP = currMesh.Positions[0];
                currDist = point_to_plane_dist(1, 0, 0, 0, cP.X, cP.Y, cP.Z);
                if (currDist <= 0.155)
                {
                    s.Material = new DiffuseMaterial(new SolidColorBrush(Colors.Black));
                    s.BackMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.Black));
                }
            }
        }

        private void OX_zero_right_click(object sender, RoutedEventArgs e)
        {
            foreach (GeometryModel3D s in cube_squares.Children)
            {
                double currDist;
                Point3D cP;
                MeshGeometry3D currMesh = s.Geometry as MeshGeometry3D;
                cP = currMesh.Positions[0];
                currDist = point_to_plane_dist(1, 0, 0, 0, cP.X, cP.Y, cP.Z);
                if (currDist <= 0.155)
                {
                    s.Material = new DiffuseMaterial(new SolidColorBrush(Colors.Black));
                    s.BackMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.Black));
                }
            }
        }

        private void OX_plus_left_click(object sender, RoutedEventArgs e)
        {
            foreach (GeometryModel3D s in cube_squares.Children)
            {
                double currDist;
                Point3D cP;
                MeshGeometry3D currMesh = s.Geometry as MeshGeometry3D;
                cP = currMesh.Positions[0];
                currDist = point_to_plane_dist(1, 0, 0, -0.35, cP.X, cP.Y, cP.Z);
                if (currDist <= 0.155)
                {
                    s.Material = new DiffuseMaterial(new SolidColorBrush(Colors.Black));
                    s.BackMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.Black));
                }
            }
        }

        private void OX_plus_right_click(object sender, RoutedEventArgs e)
        {
            foreach (GeometryModel3D s in cube_squares.Children)
            {
                double currDist;
                Point3D cP;
                MeshGeometry3D currMesh = s.Geometry as MeshGeometry3D;
                cP = currMesh.Positions[0];
                currDist = point_to_plane_dist(1, 0, 0, -0.35, cP.X, cP.Y, cP.Z);
                if (currDist <= 0.155)
                {
                    s.Material = new DiffuseMaterial(new SolidColorBrush(Colors.Black));
                    s.BackMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.Black));
                }
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (ox_3D != null && oy_3D != null && oz_3D != null)
            {
                all_to_show.Children.Add(ox_3D);
                all_to_show.Children.Add(oy_3D);
                all_to_show.Children.Add(oz_3D);
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (ox_3D != null && oy_3D != null && oz_3D != null)
            {
                all_to_show.Children.Remove(ox_3D);
                all_to_show.Children.Remove(oy_3D);
                all_to_show.Children.Remove(oz_3D);
            }
        }

        private void Zoom_Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Camera1 == null)
            {
                return;
            }
            depth = e.NewValue;
            refresh_camera();
        }

        private void CamSliderVertical_Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Camera1 == null)
            {
                return;
            }
            elev = e.NewValue;
            refresh_camera();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            camPos = new Point3D(0, 0, 0);
            lookDir = new Vector3D(0, 0, 0);
            upDir = new Vector3D(0, 0, 0);
            azm = 45;
            elev = 45;
            depth = 5.2;

            // ==== XYZ Axes ==================================================================
            ox_3D = new GeometryModel3D();
            oy_3D = new GeometryModel3D();
            oz_3D = new GeometryModel3D();

            Prism ox = new Prism(0.0, 0.0, 0.0, 3.00, 0.01, 0.01);
            Prism oy = new Prism(0.0, 0.0, 0.0, 0.01, 3.00, 0.01);
            Prism oz = new Prism(0.0, 0.0, 0.0, 0.01, 0.01, 3.00);

            ox_3D.Geometry = ox.prism_mesh;
            oy_3D.Geometry = oy.prism_mesh;
            oz_3D.Geometry = oz.prism_mesh;
            ox_3D.Material = new DiffuseMaterial(new SolidColorBrush(Colors.Red));
            oy_3D.Material = new DiffuseMaterial(new SolidColorBrush(Colors.Green));
            oz_3D.Material = new DiffuseMaterial(new SolidColorBrush(Colors.Blue));
            // ===============================================================================


            squares = new List<GeometryModel3D>();
            for(int ctr = 0; ctr < 9; ctr++)
            {
                squares.Add(new GeometryModel3D());
            }

            double xx_gora = -0.35;
            double yy_gora = -0.35;
            for (int ctr = 1; ctr < 10; ctr++)
            {
                squares[ctr-1].Material = new DiffuseMaterial(new SolidColorBrush(Colors.SlateBlue));
                squares[ctr-1].BackMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.SlateBlue));
                squares[ctr-1].Geometry = new SquareCentred(xx_gora, yy_gora, 0.5, 0.3, "xy").square_mesh;
                if (ctr % 3 == 0)
                {
                    yy_gora = -0.35;
                    xx_gora += 0.35;
                    continue;
                }
                yy_gora += 0.35;
            }

            for (int ctr = 0; ctr < 9; ctr++)
            {
                squares.Add(new GeometryModel3D());
            }

            double zz_przod = -0.35;
            double yy_przod = -0.35;
            for (int ctr = 10; ctr < 19; ctr++)
            {
                squares[ctr - 1].Material = new DiffuseMaterial(new SolidColorBrush(Colors.DarkOliveGreen));
                squares[ctr - 1].BackMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.DarkOliveGreen));
                squares[ctr - 1].Geometry = new SquareCentred( 0.5, yy_przod, zz_przod, 0.3, "zy").square_mesh;
                if (ctr % 3 == 0)
                {
                    yy_przod = -0.35;
                    zz_przod += 0.35;
                    continue;
                }
                yy_przod += 0.35;
            }

            for (int ctr = 0; ctr < 9; ctr++)
            {
                squares.Add(new GeometryModel3D());
            }

            double zz_prawa = -0.35;
            double xx_prawa = -0.35;
            for (int ctr = 19; ctr < 28; ctr++)
            {
                squares[ctr - 1].Material = new DiffuseMaterial(new SolidColorBrush(Colors.DarkGoldenrod));
                squares[ctr - 1].BackMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.DarkGoldenrod));
                squares[ctr - 1].Geometry = new SquareCentred( xx_prawa, 0.5, zz_prawa, 0.3, "xz").square_mesh;
                if (ctr % 3 == 0)
                {
                    zz_prawa = -0.35;
                    xx_prawa += 0.35;
                    continue;
                }
                zz_prawa += 0.35;
            }

            for (int ctr = 0; ctr < 9; ctr++)
            {
                squares.Add(new GeometryModel3D());
            }

            double xx_spod = -0.35;
            double yy_spod = -0.35;
            for (int ctr = 28; ctr < 37; ctr++)
            {
                squares[ctr - 1].Material = new DiffuseMaterial(new SolidColorBrush(Colors.Indigo));
                squares[ctr - 1].BackMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.Indigo));
                squares[ctr - 1].Geometry = new SquareCentred(xx_spod, yy_spod, -0.5, 0.3, "xy").square_mesh;
                if (ctr % 3 == 0)
                {
                    yy_spod = -0.35;
                    xx_spod += 0.35;
                    continue;
                }
                yy_spod += 0.35;
            }

            for (int ctr = 0; ctr < 9; ctr++)
            {
                squares.Add(new GeometryModel3D());
            }

            double zz_tyl = -0.35;
            double yz_tyl = -0.35;
            for (int ctr = 37; ctr < 46; ctr++)
            {
                squares[ctr - 1].Material = new DiffuseMaterial(new SolidColorBrush(Colors.DodgerBlue));
                squares[ctr - 1].BackMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.DodgerBlue));
                squares[ctr - 1].Geometry = new SquareCentred(-0.5, yz_tyl, zz_tyl, 0.3, "zy").square_mesh;
                if (ctr % 3 == 0)
                {
                    yz_tyl = -0.35;
                    zz_tyl += 0.35;
                    continue;
                }
                yz_tyl += 0.35;
            }

            for (int ctr = 0; ctr < 9; ctr++)
            {
                squares.Add(new GeometryModel3D());
            }

            double zz_lewa = -0.35;
            double xx_lewa = -0.35;
            for (int ctr = 46; ctr < 55; ctr++)
            {
                squares[ctr - 1].Material = new DiffuseMaterial(new SolidColorBrush(Colors.DarkRed));
                squares[ctr - 1].BackMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.DarkRed));
                squares[ctr - 1].Geometry = new SquareCentred(xx_lewa, -0.5, zz_lewa, 0.3, "xz").square_mesh;
                if (ctr % 3 == 0)
                {
                    zz_lewa = -0.35;
                    xx_lewa += 0.35;
                    continue;
                }
                zz_lewa += 0.35;
            }


            DirectionalLight DirLightZ = new DirectionalLight();
            DirLightZ.Color = Colors.White;
            DirLightZ.Direction = new Vector3D(0, 0, -1);
            DirectionalLight DirLightY = new DirectionalLight();
            DirLightY.Color = Colors.White;
            DirLightY.Direction = new Vector3D(0, -1, 0);
            DirectionalLight DirLightX = new DirectionalLight();
            DirLightX.Color = Colors.White;
            DirLightX.Direction = new Vector3D(-1, 0, 0);


            DirectionalLight DirLightZ2 = new DirectionalLight();
            DirLightZ2.Color = Colors.White;
            DirLightZ2.Direction = new Vector3D(0, 0, 1);
            DirectionalLight DirLightY2 = new DirectionalLight();
            DirLightY2.Color = Colors.White;
            DirLightY2.Direction = new Vector3D(0, 1, 0);
            DirectionalLight DirLightX2 = new DirectionalLight();
            DirLightX2.Color = Colors.White;
            DirLightX2.Direction = new Vector3D(1, 0, 0);



            Camera1 = new PerspectiveCamera();
            Camera1.FarPlaneDistance = 20;
            Camera1.NearPlaneDistance = 1;
            Camera1.FieldOfView = 40;
            //Camera1.Position = new Point3D(3, 3, 3);
            //Camera1.LookDirection = new Vector3D(-3, -3, -3);
            //Camera1.UpDirection = new Vector3D(0, 0, 1);
            refresh_camera();

            cube_squares = new Model3DGroup();
            foreach (GeometryModel3D m in squares)
            {
                cube_squares.Children.Add(m);
            }
            all_to_show = new Model3DGroup();
            all_to_show.Children.Add(ox_3D);
            all_to_show.Children.Add(oy_3D);
            all_to_show.Children.Add(oz_3D);
            all_to_show.Children.Add(DirLightX);
            all_to_show.Children.Add(DirLightY);
            all_to_show.Children.Add(DirLightZ);
            all_to_show.Children.Add(DirLightX2);
            all_to_show.Children.Add(DirLightY2);
            all_to_show.Children.Add(DirLightZ2);
            all_to_show.Children.Add(cube_squares);
            ModelVisual3D modelsVisual = new ModelVisual3D();
            modelsVisual.Content = all_to_show;
            
            Viewport3D myViewport = new Viewport3D();
            myViewport.Camera = Camera1;
            myViewport.Children.Add(modelsVisual);
            this.Canvas1.Children.Add(myViewport);
            myViewport.Height = 500;
            myViewport.Width = 500;
            Canvas.SetTop(myViewport, -50);
            Canvas.SetLeft(myViewport, 0);
            this.Width = myViewport.Width;
            this.Height = myViewport.Height;

            AxisAngleRotation3D axis = new AxisAngleRotation3D(
                  new Vector3D(0, 0, 1), 0);
            RotateTransform3D Rotate = new RotateTransform3D(axis);
            
            foreach(GeometryModel3D n in squares) 
            {
              //  n.Transform = Rotate;
            }
            /*
            Square_1.Transform = Rotate;
            Square_2.Transform = Rotate;
            Square_3.Transform = Rotate;
            Square_4.Transform = Rotate;
            
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
