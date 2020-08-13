using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DocumentScanner;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class RangedListTests
    {
        private RangedList<int?> _rangedList;
        private const int _defaultValue = -1;

        private KeyValuePair<int, int?> kv(int key, int? val) =>
            new KeyValuePair<int, int?>(key, val);

        [TestInitialize]
        public void InitializeTest()
        {
            _rangedList = new RangedList<int?>(_defaultValue, false);
            _rangedList[1] = 10;
            _rangedList[2] = null;
            _rangedList[4] = 40;
        }

        [TestMethod]
        public void TestEnumeration()
        {
            var list = _rangedList.Take(6).ToList();

            var expectedList = new List<KeyValuePair<int, int?>> {
                kv(0, _defaultValue),
                kv(1, 10),
                kv(2, (int?)null),
                kv(3, (int?)null),
                kv(4, 40),
                kv(5, 40),
            };

            CollectionAssert.AreEqual(expectedList, list);
        }

        [TestMethod]
        public void TestExplicitIndexes()
        {
            Assert.AreEqual(10, _rangedList[1]);
            Assert.AreEqual(null, _rangedList[2]);
            Assert.AreEqual(40, _rangedList[4]);
        }

        [TestMethod]
        public void TestImplicitIndexes()
        {
            Assert.AreEqual(_defaultValue, _rangedList[0]);
            Assert.AreEqual(null, _rangedList[3]);
        }
    }
}