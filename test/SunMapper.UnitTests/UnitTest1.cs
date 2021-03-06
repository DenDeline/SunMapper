using SunMapper.Common.Attributes;
using SunMapper.Generated;
using Xunit;

namespace SunMapper.UnitTests
{
    
    [MapTo(typeof(UserGetDto))]
    public partial class User
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    [MapTo(typeof(UserUpdateDto))]
    public partial class User
    {
        
    }
    
    
    public class UserGetDto
    {
        public string Name { get; set; }
    }

    public class UserUpdateDto
    {
        public string Name { get; set; }
    }

    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {   
            var user = new User();

            Assert.True(user.TryMapTo(out UserGetDto destination));
        }

        [Fact]
        public void Test2()
        {
            var user = new User
            {
                Name = "User1"
            };
            
            Assert.True(user.TryMapTo(out UserGetDto destination));
            Assert.Equal("User1", destination.Name);
        }
    }
}