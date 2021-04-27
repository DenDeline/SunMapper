using AutoFixture;
using AutoMapper;
using BenchmarkDotNet.Attributes;
using Mapster;
using SunMapper.Generated;

namespace SunMapper.Benchmarks
{
    [Config(typeof(Config))]
    public class StandardMappingBenchmarks
    {
        private static User _user = new Fixture().Create<User>();
        private static IMapper _mapper = new MapperConfiguration(_ => _.CreateMap<User, UserGetDto>()).CreateMapper();
        
        [Benchmark]
        public UserGetDto SunMapper_UserGetDto()
        {
            if (_user.TryMapTo(out UserGetDto userGetDto))
            {
                return userGetDto; 
            }
            
            return userGetDto;
        }

        [Benchmark]
        public UserGetDto AutoMapper_UserGetDto()
        {
            return _mapper.Map<UserGetDto>(_user);
        }
        
        [Benchmark]
        public UserGetDto Mapster_UserGetDto()
        {   
            return _user.Adapt<UserGetDto>();
        }
    }
}