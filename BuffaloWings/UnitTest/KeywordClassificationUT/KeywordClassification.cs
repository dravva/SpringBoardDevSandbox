namespace KeywordClassificationUT
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using System.Collections.Generic;

    using Microsoft.Dldw.BuffaloWings.Common.KeywordClassifier;

    [TestClass]
    public class KeywordClassification
    {
        readonly KeywordClassifier keywordClassifier = new KeywordClassifier();
        [TestMethod]
        public void TestKeywordClassificationBasic()
        {

            var categories = this.keywordClassifier.GetTextClassification( new List<string> { "facebook", "cricket" }, 3 );

            Assert.AreEqual( categories.Count,2 );

            Assert.IsTrue(categories["facebook"].Categories.Count > 0);

            Assert.IsTrue(categories["facebook"].Categories[0].Contains( "Social Networking" ));

            Assert.IsTrue(categories["cricket"].Categories.Count > 0);

            Assert.IsTrue(categories["cricket"].Categories[0].Contains("Sports"));

            
            // Test Cache
            
            categories = this.keywordClassifier.GetTextClassificationFromCache(new List<string> { "facebook", "cricket" }, 3);

            Assert.AreEqual(categories.Count, 2);

            Assert.IsTrue(categories["facebook"].Categories.Count > 0);

            Assert.IsTrue(categories["facebook"].Categories[0].Contains("Social Networking"));

            Assert.IsTrue(categories["cricket"].Categories.Count > 0);

            Assert.IsTrue(categories["cricket"].Categories[0].Contains("Sports"));
        }

    }
}
