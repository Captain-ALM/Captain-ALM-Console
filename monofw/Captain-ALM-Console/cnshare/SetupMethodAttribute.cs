using System;

namespace captainalm.calmcon.api
{
    /// <summary>
    /// This Attribute is used to Specify The Setup Method of The Library.
    /// </summary>
    /// <remarks></remarks>
    [AttributeUsage(AttributeTargets.Method)]
    public class SetupMethodAttribute : Attribute
    {
        /// <summary>
        /// Initalizes a new instance of this Attribute on The Method.
        /// </summary>
        /// <remarks></remarks>
        public SetupMethodAttribute() { }
    }

}
