using System;

namespace SunMapper.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class MapToAttribute : Attribute
    {
        public Type Desination { get; }

        public MapToAttribute(Type destination)
        {
            Desination = destination;
        }
    }
}
