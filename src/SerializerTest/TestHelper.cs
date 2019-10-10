using System;
using Es.Serializer;

namespace SerializerTest
{
    public class TestHelper
    {
        public static Foo GetFoo()
        {

            return new Foo
            {
                Id = 1,
                Name = "John",
                Age = 30,
                Date = DateTime.UtcNow,
                Sex = true,
                Xuid = Guid.NewGuid(),
                UserStatus = UserStatus.Approved,
                Lastip = new byte[] { 192, 168, 0, 254 },
                Inner = new InnerFoo { InnerNumeric = 999.99912m }
            };
        }

        public static bool Equal(Foo expected, Foo actual)
        {
            return expected.Id == actual.Id
                && expected.Name == actual.Name
                && expected.Age == actual.Age
                && expected.Date.ToLongDateString() == actual.Date.ToLongDateString()
                && expected.Sex == actual.Sex
                && expected.Xuid == actual.Xuid
                && expected.UserStatus == actual.UserStatus
                && string.Join(",", expected.Lastip) == string.Join(",", actual.Lastip)
                && expected.Inner.InnerNumeric == actual.Inner.InnerNumeric;
        }
    }
}