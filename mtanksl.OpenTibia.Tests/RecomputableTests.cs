using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTibia.Common.Objects;

namespace mtanksl.OpenTibia.Tests
{
    [TestClass]
    public class RecomputableTests
    {
        [TestMethod]
        public void RecomputableSource()
        {
            int count = 0;

            var rs = new RecomputableSource();

            rs.Changed += (sender, e) =>
            {
                count++;
            };

            Assert.AreEqual(0, count);

            rs.Change();

            Assert.AreEqual(1, count);

            rs.Change();

            Assert.AreEqual(2, count);
        }

        [TestMethod]
        public void Recomputable()
        {
            int count = 0;

            var rs = new RecomputableSource();

            var r = new Recomputable<object>(rs, () =>
            {
                count++;

                return null;
            } );

            Assert.AreEqual(0, count);

            var value = r.Value;

            Assert.AreEqual(1, count);
                        
            rs.Change();

            Assert.AreEqual(1, count);

            rs.Change();

            Assert.AreEqual(1, count);

            value = r.Value;

            Assert.AreEqual(2, count);

            value = r.Value;

            Assert.AreEqual(2, count);
        }

        [TestMethod]
        public void AndRecomputable()
        {
            int count = 0;

            var a = new RecomputableSource();

            var b = new RecomputableSource();

            var r = new Recomputable<object>(new AndRecomputableSource(a, b), () =>
            {
                count++;

                return null;
            } );

            Assert.AreEqual(0, count);

            var value = r.Value;

            Assert.AreEqual(1, count);
                                    
            a.Change();

            Assert.AreEqual(1, count);

            value = r.Value;

            Assert.AreEqual(1, count);

            b.Change();

            Assert.AreEqual(1, count);

            value = r.Value;

            Assert.AreEqual(2, count);

            value = r.Value;

            Assert.AreEqual(2, count);
        }

        [TestMethod]
        public void OrRecomputable()
        {
            int count = 0;

            var a = new RecomputableSource();

            var b = new RecomputableSource();

            var r = new Recomputable<object>(new OrRecomputableSource(a, b), () =>
            {
                count++;

                return null;
            } );

            Assert.AreEqual(0, count);

            var value = r.Value;

            Assert.AreEqual(1, count);
                                    
            a.Change();

            Assert.AreEqual(1, count);

            value = r.Value;

            Assert.AreEqual(2, count);

            b.Change();

            Assert.AreEqual(2, count);

            value = r.Value;

            Assert.AreEqual(3, count);

            value = r.Value;

            Assert.AreEqual(3, count);
        }
    }
}