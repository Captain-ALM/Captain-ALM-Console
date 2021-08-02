using System;

namespace captainalm.calmcmd
{
    /// <summary>
    /// Provides an interface for syntax classes.
    /// </summary>
    public interface ISyntax : IName
    {
        /// <summary>
        /// Decodes a passed command string into a command name string and an array of argument strings
        /// </summary>
        /// <param name="dataIn">The command string in</param>
        /// <param name="commandOut">The command name string out</param>
        /// <param name="argumentsOut">The array of argument strings out</param>
        /// <returns>If the command string was valid (Should still be valid if the command was invalid but the syntax was correct)</returns>
        /// <remarks>If the syntax was correct but the command invalid, the return value should still be true, but the command out should be the value of <see cref="captainalm.calmcmd.API.invalidCommandName">API.invalidCommand</see>.</remarks>
        bool decode(string dataIn, ref string commandOut, ref string[] argumentsOut);
        /// <summary>
        /// Converts a string argument to another type if needed
        /// </summary>
        /// <param name="argumentIn">The string argument in</param>
        /// <returns>The argument out</returns>
        object argumentTypeConversion(string argumentIn);
    }
}
