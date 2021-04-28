using System;
using System.Collections.Generic;
using SunMapper.Core.Attributes;

namespace SunMapper.Benchmarks
{
    [MapTo(typeof(UserGetDto))]
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public DateTime Birthday { get; set; }
        public bool IsEmailApproved { get; set; }
        public Image ProfileImage { get; set; }
        public IEnumerable<Image> Images { get; set; }
        public string Status { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public IEnumerable<Post> Posts { get; set; }
        public DateTime LastLoginAt { get; set; }
        public DateTime RegisterAt { get; set; }
    }

    public class UserGetDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Country { get; set; }
        public DateTime Birthday { get; set; }
        public Image ProfileImage { get; set; }
        public IEnumerable<Image> Images { get; set; }
        public string Status { get; set; }
        public IEnumerable<Post> Posts { get; set; }
        public DateTime LastLoginAt { get; set; }
    }
    
    public class Image
    {
        
    }
    public class Post
    {
        
    }
}