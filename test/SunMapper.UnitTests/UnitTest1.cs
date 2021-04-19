using SunMapper.Attributes;
using SunMapperExtensions;
using Xunit;

namespace SunMapper.UnitTests
{
    [MapTo(typeof(UserGetDto))]
    public class User
    {

    }

    [MapTo(typeof(User))]
    public class UserGetDto
    {

    }

    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var user = new User();
            user.TryMapTo(out UserGetDto dest);
        }
    }
}