﻿using System;
using NUnit.Framework;
using WorldEdit.Regions;
using WorldEdit.Regions.Selectors;

namespace WorldEdit.Tests.Regions.Selectors
{
    [TestFixture]
    public class EllipticRegionSelectorTests
    {
        [Test]
        public void Clear()
        {
            RegionSelector selector = new EllipticRegionSelector();
            selector = selector.WithPrimary(Vector.Zero);
            selector = selector.WithSecondary(Vector.One);

            var selector2 = (EllipticRegionSelector)selector.Clear();

            Assert.That(selector2.Position1, Is.Null);
            Assert.That(selector2.Position2, Is.Null);
        }

        [TestCase(0, 0, 4, 4)]
        public void GetRegion(int x, int y, int x2, int y2)
        {
            RegionSelector selector = new EllipticRegionSelector();
            selector = selector.WithPrimary(new Vector(x, y));
            selector = selector.WithSecondary(new Vector(x2, y2));

            var region = (EllipticRegion)selector.GetRegion();

            Assert.That(region.Center, Is.EqualTo(new Vector(x, y)));
            Assert.That(region.Radius, Is.EqualTo(new Vector(Math.Abs(x2 - x), Math.Abs(y2 - y))));
        }

        [Test]
        public void GetRegion_NoPrimary_NullRegion()
        {
            RegionSelector selector = new EllipticRegionSelector();
            selector = selector.WithSecondary(Vector.Zero);

            Assert.That(selector.GetRegion(), Is.InstanceOf<EmptyRegion>());
        }

        [Test]
        public void GetRegion_NoSecondary_NullRegion()
        {
            RegionSelector selector = new EllipticRegionSelector();
            selector = selector.WithPrimary(Vector.Zero);

            Assert.That(selector.GetRegion(), Is.InstanceOf<EmptyRegion>());
        }

        [TestCase(1, 5)]
        public void Position1(int x, int y)
        {
            var selector = new EllipticRegionSelector(new Vector(x, y));

            Assert.That(selector.Position1, Is.EqualTo(new Vector(x, y)));
        }

        [TestCase(1, 5)]
        public void Position2(int x, int y)
        {
            var selector = new EllipticRegionSelector(null, new Vector(x, y));

            Assert.That(selector.Position2, Is.EqualTo(new Vector(x, y)));
        }

        [TestCase(1, 5)]
        public void PrimaryPosition(int x, int y)
        {
            var selector = new EllipticRegionSelector();

            var selector2 = (EllipticRegionSelector)selector.WithPrimary(new Vector(x, y));

            Assert.That(selector2.PrimaryPosition, Is.EqualTo(new Vector(x, y)));
        }

        [TestCase(1, 5)]
        public void WithPrimary(int x, int y)
        {
            var selector = new EllipticRegionSelector();

            var selector2 = (EllipticRegionSelector)selector.WithPrimary(new Vector(x, y));

            Assert.That(selector2.Position1, Is.EqualTo(new Vector(x, y)));
        }

        [TestCase(1, 5)]
        public void WithSecondary(int x, int y)
        {
            var selector = new EllipticRegionSelector();

            var selector2 = (EllipticRegionSelector)selector.WithSecondary(new Vector(x, y));

            Assert.That(selector2.Position2, Is.EqualTo(new Vector(x, y)));
        }
    }
}
