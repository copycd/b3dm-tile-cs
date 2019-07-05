using glTFLoader;
using NUnit.Framework;
using System.IO;

namespace B3dm.Tile.Tests
{
    public class B3dmReaderTests
    {
        Stream b3dmfile;
        string expectedMagicHeader = "b3dm";
        int expectedVersionHeader = 1;

        [SetUp]
        public void Setup()
        {
            b3dmfile = File.OpenRead(@"testfixtures/1_expected.b3dm");
            Assert.IsTrue(b3dmfile != null);
        }

        [Test]
        public void ReadB3dmTest()
        {
            // arrange

            // act
            var b3dm = B3dmReader.ReadB3dm(b3dmfile);
            var stream = new MemoryStream(b3dm.GlbData);
            var gltf = Interface.LoadModel(stream);

            // assert
            Assert.IsTrue(expectedMagicHeader == b3dm.B3dmHeader.Magic);
            Assert.IsTrue(expectedVersionHeader == b3dm.B3dmHeader.Version);
            Assert.IsTrue(b3dm.BatchTableJson.Length >= 0);
            Assert.IsTrue(b3dm.GlbData.Length > 0);
            var ms = new MemoryStream(b3dm.GlbData);
        }

        [Test]
        public void ReadB3dmWithGlbTest()
        {
            // arrange
            var buildingGlb = File.ReadAllBytes(@"testfixtures/building.glb");

            // act

            var b3dm = new B3dm(buildingGlb);

            // assert
            Assert.IsTrue(b3dm.GlbData.Length == 2924);
        }


        [Test]
        public void ReadNederland3DB3dmTest()
        {
            // arrange
            var b3dmfile1 = File.OpenRead(@"testfixtures/nederland3d_6825.b3dm");

            // act
            var b3dm = B3dmReader.ReadB3dm(b3dmfile1);
            var stream = new MemoryStream(b3dm.GlbData);
            // loading goes wrong, because animations?
            // Unhandled Exception: Newtonsoft.Json.JsonSerializationException: Error setting value to 'Animations' on 'glTFLoader.Schema.Gltf'. ---> System.ArgumentException: Array not long enough
            // at glTFLoader.Schema.Gltf.set_Animations(Animation[] value)

            // var gltf = Interface.LoadModel(stream);


            // assert
            Assert.IsTrue(expectedMagicHeader == b3dm.B3dmHeader.Magic);
            Assert.IsTrue(expectedVersionHeader == b3dm.B3dmHeader.Version);
        }
    }
}