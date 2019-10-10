using System.IO;
using System.Linq;
using Es.Serializer;
using Xunit;

namespace SerializerTest
{

    public class NETSerializerTest
    {
        public NETSerializerTest()
        {
            var type = typeof(INetMessage);
            var types = type.Assembly.GetTypes()
                .Where(t => !t.IsInterface && t.IsClass && !t.IsAbstract && type.IsAssignableFrom(t))
                .ToArray();

            NetSerializer.Serializer instance = new NetSerializer.Serializer(types);
            NETSerializer ns = new NETSerializer(instance);
            SerializerFactory.AddSerializer(new NETSerializer(instance), "NET");
        }

        [Fact]
        public void Can_NET_Serializer_String()
        {
            var bs = SerializerFactory.Get("NET");

            var foo1 = TestHelper.GetFoo();

            var str = bs.SerializeToString(foo1);

            var foo2 = bs.DeserializeFromString<Foo>(str);

            Assert.True(TestHelper.Equal(foo1, foo2));
        }

        [Fact]
        public void Can_NET_Serializer_Stream()
        {
            var bs = SerializerFactory.Get("NET");

            var foo1 = TestHelper.GetFoo();
            Stream output = new MemoryStream();
            bs.Serialize(foo1, output);

            output.Position = 0;
            var foo2 = bs.Deserialize<Foo>(output);

            output.Dispose();

            Assert.True(TestHelper.Equal(foo1, foo2));
        }

        [Fact]
        public void Can_NET_Serializer_Bytes()
        {
            var bs = SerializerFactory.Get("NET");

            var foo1 = TestHelper.GetFoo();
            byte[] output;
            bs.Serialize(foo1, out output);

            var foo2 = bs.Deserialize<Foo>(output);
            Assert.True(TestHelper.Equal(foo1, foo2));
        }

        [Fact]
        public void Can_NET_Serializer_Writer_And_Reader()
        {
            var bs = SerializerFactory.Get("NET");
            var foo1 = TestHelper.GetFoo();

            StringWriter sw = new StringWriter();

            bs.Serialize(foo1, sw);

            StringReader sr = new StringReader(sw.ToString());

            var foo2 = bs.Deserialize<Foo>(sr);

            Assert.True(TestHelper.Equal(foo1, foo2));
        }
    }
}