using System.IO;
using Es.Serializer;
using Xunit;

namespace SerializerTest
{
    public class JilSerializerTest
    {
        public JilSerializerTest()
        {
            SerializerFactory.AddSerializer<JilSerializer>("jil");
        }

        [Fact]
        public void Can_Jil_Serializer_String()
        {
            var bs = SerializerFactory.Get("jil");

            var foo1 = TestHelper.GetFoo();

            var str = bs.SerializeToString(foo1);

            var foo2 = bs.DeserializeFromString<Foo>(str);

            Assert.True(TestHelper.Equal(foo1, foo2));
        }

        [Fact]
        public void Can_Jil_Serializer_Stream()
        {
            var bs = SerializerFactory.Get("jil");

            var foo1 = TestHelper.GetFoo();
            Stream output = new MemoryStream();
            bs.Serialize(foo1, output);

            output.Position = 0;
            var foo2 = (Foo)bs.Deserialize(output, typeof(Foo));

            output.Dispose();

            Assert.True(TestHelper.Equal(foo1, foo2));
        }

        [Fact]
        public void Can_Jil_Serializer_Bytes()
        {
            var bs = SerializerFactory.Get("jil");

            var foo1 = TestHelper.GetFoo();
            byte[] output;
            bs.Serialize(foo1, out output);

            var foo2 = (Foo)bs.Deserialize(output, typeof(Foo));
            Assert.True(TestHelper.Equal(foo1, foo2));
        }

        [Fact]
        public void Can_Jil_Serializer_Writer_And_Reader()
        {
            var bs = SerializerFactory.Get("jil");
            var foo1 = TestHelper.GetFoo();

            StringWriter sw = new StringWriter();

            bs.Serialize(foo1, sw);

            StringReader sr = new StringReader(sw.ToString());

            var foo2 = (Foo)bs.Deserialize(sr, typeof(Foo));

            Assert.True(TestHelper.Equal(foo1, foo2));
        }
    }
}