using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Xml.Linq;
using System.Windows.Media.Media3D;
using System.Windows.Media;

namespace Cube1
{
    class SquareCentred
    {

        public MeshGeometry3D square_mesh;
        // parellel_to can be: "xy", "xz", "yz"
        public SquareCentred(double xc, double yc, double zc, double size, string parallel_to,
                                string color = "red")
        {
            double d = size / 2.0;
            square_mesh = new MeshGeometry3D();
            Point3DCollection corners = new Point3DCollection();
            if (parallel_to == "xy")
            {
                corners.Add(new Point3D(xc - d, yc - d, zc));
                corners.Add(new Point3D(xc - d, yc + d, zc));
                corners.Add(new Point3D(xc + d, yc + d, zc));
                corners.Add(new Point3D(xc + d, yc - d, zc));
                square_mesh.Positions = corners;
                Int32[] indices = {
                 1,0,3,
                 3,2,1
                };
                Int32Collection Triangles = new Int32Collection();
                foreach (Int32 index in indices)
                {
                    Triangles.Add(index);
                }
                square_mesh.TriangleIndices = Triangles;

            }
            if (parallel_to == "xz")
            {
                corners.Add(new Point3D(xc + d, yc, zc + d));
                corners.Add(new Point3D(xc + d, yc, zc - d));
                corners.Add(new Point3D(xc - d, yc, zc - d));
                corners.Add(new Point3D(xc - d, yc, zc + d));
                square_mesh.Positions = corners;
                Int32[] indices = {
                 0,3,2,
                 2,1,0
                };
                Int32Collection Triangles = new Int32Collection();
                foreach (Int32 index in indices)
                {
                    Triangles.Add(index);
                }
                square_mesh.TriangleIndices = Triangles;

            }
            if (parallel_to == "zy")
            {
                corners.Add(new Point3D(xc, yc - d, zc + d));
                corners.Add(new Point3D(xc, yc - d, zc - d));
                corners.Add(new Point3D(xc, yc + d, zc - d));
                corners.Add(new Point3D(xc, yc + d, zc + d));
                square_mesh.Positions = corners;
                Int32[] indices = {
                 0,1,2,
                 2,3,0
                };
                Int32Collection Triangles = new Int32Collection();
                foreach (Int32 index in indices)
                {
                    Triangles.Add(index);
                }
                square_mesh.TriangleIndices = Triangles;

            }

        }
    }
}
