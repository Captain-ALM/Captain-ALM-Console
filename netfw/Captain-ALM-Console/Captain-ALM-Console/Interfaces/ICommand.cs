using System;
using System.Collections.Generic;

namespace captainalm.calmcmd
{
    /// <summary>
    /// Provides an interface for command classes.
    /// </summary>
    public interface ICommand : IName
    {
        /// <summary>
        /// Runs the command with the specified argument array returning the result
        /// </summary>
        /// <param name="argumentsIn">The arguments in</param>
        /// <returns>The result</returns>
        object run(object[] argumentsIn);
        /// <summary>
        /// Returns the help string for this command
        /// </summary>
        string help { get; }
    }
}
