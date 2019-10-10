using System.IO;
using System.IO.Compression;
using Es.Serializer;
using Xunit;

namespace SerializerTest
{
    public class CompressionTest
    {
        [Fact]
        public void Can_Compression()
        {
            var serialize = new JsonNetSerializer();

            var foo = TestHelper.GetFoo();

            var output1 = new MemoryStream();
            var output2 = new MemoryStream();

            serialize.Serialize(foo, output1);

            var rtl1 = output1.ToArray();

            //compress
            var gzip = new GZipStream(output2, CompressionLevel.Fastest);
            StreamWriter sw2 = new StreamWriter(gzip);
            serialize.Serialize(foo, sw2);

            sw2.Dispose();
            var rtl2 = output2.ToArray();

            //comparing the size
            Assert.True(rtl2.Length < rtl1.Length);

            //decompress
            var gzip2 = new GZipStream(new MemoryStream(rtl2), CompressionMode.Decompress);
            StreamReader sr = new StreamReader(gzip2);
            var foo2 = serialize.Deserialize<Foo>(sr);

            Assert.Equal(foo.ToString(), foo2.ToString());
        }
    }
}