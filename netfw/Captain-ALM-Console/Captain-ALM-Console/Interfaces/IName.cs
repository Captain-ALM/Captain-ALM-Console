using System;
using System.Collections.Generic;

namespace captainalm.calmcmd
{
    /// <summary>
    /// Provides an Interface with a readonly name property.
    /// </summary>
    public interface IName
    {
        /// <summary>
        /// Returns the name of the class
        /// </summary>
        string name { get; }
        /// <summary>
        /// Returns the owner of the class
        /// </summary>
        string owner { get; }
    }
}
