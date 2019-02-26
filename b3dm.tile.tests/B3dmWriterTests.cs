﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using Wkx;

namespace B3dm.Tile.Tests
{
    public class B3dmWriterTests
    {
        [Test]
        public void WriteB3dmTest()
        {
            const string testfile = "B3dm.Tile.Tests.testfixtures.building.wkb";
            var buildingWkb = Assembly.GetExecutingAssembly().GetManifestResourceStream(testfile);
            var g = Geometry.Deserialize<WkbSerializer>(buildingWkb);
            Assert.IsTrue(g.GeometryType == GeometryType.PolyhedralSurface);
            var polyhedralsurface = ((PolyhedralSurface)g);
            Assert.IsTrue(polyhedralsurface.Geometries.Count == 15);  // 15 polygons
            Assert.IsTrue(polyhedralsurface.Geometries[0].ExteriorRing.Points.Count == 5);  // 5 points in exteriorring of first geometry

            var bb = polyhedralsurface.GetBoundingBox();
            Assert.IsTrue(Math.Round(bb.XMin, 2) == -8.75);
            Assert.IsTrue(Math.Round(bb.YMin, 2) == -7.36);
            Assert.IsTrue(Math.Round(bb.XMax, 2) == 8.80);
            Assert.IsTrue(Math.Round(bb.YMax, 2) == 7.30);

            var polygon3d = polyhedralsurface.Geometries[0];
            var points2d = Projections.Get2DPoints(polygon3d);
            Assert.IsTrue(points2d.Count == 10);

            // act
            var res = Earcut.Tessellate(points2d, new List<int>());
        }

    }
}
