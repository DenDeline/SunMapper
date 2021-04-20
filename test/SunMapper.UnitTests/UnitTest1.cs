using SunMapper.Attributes;
using SunMapperExtensions;
using Xunit;

namespace SunMapper.UnitTests
{
    public class UnitTest1
    {
        [MapTo(typeof(UserGetDto))]
        public class User
        {

        }

        [MapTo(typeof(User))]
        public class UserGetDto
        {

        }
        
        [Fact]
        public void Test1()
        {
            var user = new User();
            Assert.True(user.TryMapTo(out UserGetDto dest));
        }
    }
}