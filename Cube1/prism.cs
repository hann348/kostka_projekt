using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows.Media;

namespace Cube1
{
    class Prism
    {
        public MeshGeometry3D prism_mesh;

        public Prism(double sx, double sy, double sz, double dx, double dy, double dz)
        {

         this.prism_mesh = new MeshGeometry3D();
         Point3DCollection corners = new Point3DCollection();
         // 0
         corners.Add(new Point3D(sx + dx, sy + dy, sz + dz));
         // 1
         corners.Add(new Point3D(sx, sy + dy, sz + dz));
         // 2
         corners.Add(new Point3D(sx , sy, sz + dz));
         // 3
         corners.Add(new Point3D(sx + dx, sy, sz + dz));
         // 4
         corners.Add(new Point3D(sx + dx, sy + dy, sz));
         // 5
         corners.Add(new Point3D(sx, sy + dy, sz));
         // 6
         corners.Add(new Point3D(sx, sy, sz));
         // 7
         corners.Add(new Point3D(sx + dx, sy, sz));
         prism_mesh.Positions = corners;

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
           prism_mesh.TriangleIndices = Triangles;
        }

        public MeshGeometry3D return_mesh()
        {
            return prism_mesh;
        }

    }
}
