using System;

namespace captainalm.calmcmd
{
    /// <summary>
    /// This Attribute is used to Specify The Setup Method of The Extension.
    /// </summary>
    /// <remarks></remarks>
    [AttributeUsage(AttributeTargets.Method)]
    class ExtensionSetupMethodAttribute : Attribute
    {
         /// <summary>
        /// Initalizes a new instance of this Attribute on The Method.
        /// </summary>
        /// <remarks></remarks>
        public ExtensionSetupMethodAttribute() { }
    }
}
