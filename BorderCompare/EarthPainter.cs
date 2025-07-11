using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace ScottReece.BorderCompare.App
{
    public class EarthPosition
    {
        public LatLong Center;
        public float ZoomFactor;

        public EarthPosition() { }
        public EarthPosition(LatLong center, float zoomFactor)
        {
            Center = center;
            ZoomFactor = zoomFactor;
        }
    }

    public class Palette
    {
        public Pen BorderPen = Pens.Black;
        public Brush? OceanBrush = Brushes.DodgerBlue;
        public Brush? CountryBrush = Brushes.Tan;
    }

    public class EarthPainter
    {
        public MapInfo Draw(Graphics g, Rectangle bounds, List<GeoArea> areas, EarthPosition position, Palette palette)
        {
            g.SmoothingMode = SmoothingMode.HighQuality;

            var centerOfBounds = bounds.GetCenter();
            var minSize = Math.Min(bounds.Height, bounds.Width) - 10;
            var scale = minSize / 2f;

            scale *= position.ZoomFactor;

            g.DrawEllipse(Pens.DodgerBlue, centerOfBounds.X - scale, centerOfBounds.Y - scale, scale * 2, scale * 2);

            if (palette.OceanBrush != null)
                g.FillEllipse(palette.OceanBrush, centerOfBounds.X - scale, centerOfBounds.Y - scale, scale * 2, scale * 2);

            foreach (var area in areas)
            {
                foreach (var shape in area.Shapes)
                {
                    var points = shape.BoundaryPoints.Select(ll => Get3dPoint(ll, position)).ToArray();

                    var twoDPoints = points.Where(x => x.Y > 0).Select(p =>
                    {
                        var x = p.X;
                        var y = p.Z;

                        return new PointF(centerOfBounds.X + (float)x * scale, centerOfBounds.Y + (float)-y * scale);
                    }).ToArray();

                    if (twoDPoints.Length < 3)
                        continue;

                    if (palette.CountryBrush != null)
                        g.FillPolygon(palette.CountryBrush, twoDPoints);

                    g.DrawPolygon(palette.BorderPen, twoDPoints);
                }
            }

            var info = new MapInfo();
            info.Center = position;
            info.Bounds = bounds;

            var screenEarthWidth = scale * 2;
            var halfEarthCircumferenceInPixels = Math.PI * screenEarthWidth / 2;
            info.DegreesPerPixel = 180 / halfEarthCircumferenceInPixels;

            return info;
        }

        private Vector3 Get3dPoint(LatLong latLong, EarthPosition position)
        {
            var lat = DegreesToRads(latLong.Latitude);
            var lng = DegreesToRads(latLong.Longitude - 90 - position.Center.Longitude) * -1;

            var xyScale = Math.Cos(lat);
            var z = Math.Sin(lat);
            var x = Math.Cos(lng) * xyScale;
            var y = Math.Sin(lng) * xyScale;

            var pt = new Vector3(x, y, z);
            pt = pt.RotateAroundXAxis(-position.Center.Latitude);


            return pt;
        }

        private double DegreesToRads(double degrees)
        {
            return degrees / 360 * 2 * Math.PI;
        }
    }

    public class MapInfo
    {
        public EarthPosition Center;
        public Rectangle Bounds;

        public double DegreesPerPixel;
    }

    public class Vector3
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Vector3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3 RotateAroundXAxis(double degrees)
        {
            var rads = MyGeometry.DegreesToRadians(degrees);

            var xx = Y;
            var yy = Z;

            var radius = MyGeometry.SolveForHypotenuse(xx, yy);
            var angle = Math.Atan2(yy, xx);
            angle += rads;

            xx = Math.Cos(angle) * radius;
            yy = Math.Sin(angle) * radius;

            return new Vector3(X, xx, yy);
        }
    }
}
