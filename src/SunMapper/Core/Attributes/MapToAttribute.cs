using System;

namespace SunMapper.Core.Attributes
{
    /// <summary>
    /// Allow SunMapper to find classes from you are mapping 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class MapToAttribute : Attribute
    {
        public Type Desination { get; }

        /// <summary>
        /// Allow SunMapper to find classes from you are mapping 
        /// </summary>
        /// <param name="destination">Destination mapping class type</param>
        /// <remarks> SunMapper will generate TryMapTo extension method for this class </remarks>
        public MapToAttribute(Type destination)
        {
            Desination = destination;
        }
    }
}
