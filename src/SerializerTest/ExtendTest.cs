using Es.Serializer;
using Xunit;

namespace SerializerTest
{
    public class ExtendTest
    {
        public ExtendTest()
        {
            SerializerFactory.AddSerializer<JilSerializer>("jil");
            SerializerFactory.AddSerializer<JsonNetSerializer>("jsonNet");
            SerializerFactory.AddSerializer<ProtoBufSerializer>("ProtoBuf");
        }

        [Fact]
        public void Can_deep_clone()
        {
            var foo = TestHelper.GetFoo();
            var foo2 = foo.DeepClone();
            Assert.NotEqual(foo, foo2);
            Assert.Equal(foo.ToString(), foo2.ToString());
        }
#if NETFULL
        [Fact]
        public void Can_soap_deep_clone() {
            Test_Deep_Clone(SerializerFactory.Get("soap"));
        }
#endif

        [Fact]
        public void Can_Jil_deep_clone()
        {
            Test_Deep_Clone(SerializerFactory.Get("jil"));
        }

        [Fact]
        public void Can_dc_deep_clone()
        {
            Test_Deep_Clone(SerializerFactory.Get("dc"));
        }

        [Fact]
        public void Can_jsonNet_deep_clone()
        {
            Test_Deep_Clone(SerializerFactory.Get("jsonNet"));
        }

        [Fact]
        public void Can_ProtoBuf_deep_clone()
        {
            Test_Deep_Clone(SerializerFactory.Get("ProtoBuf"));
        }

        [Fact]
        public void Can_xml_deep_clone()
        {
            Test_Deep_Clone(SerializerFactory.Get("xml"));
        }

        private void Test_Deep_Clone(SerializerBase sf)
        {
            var foo = TestHelper.GetFoo();
            var foo2 = sf.DeepClone(foo);
            Assert.NotEqual(foo, foo2);
            Assert.Equal(foo.ToString(), foo2.ToString());
        }
    }
}