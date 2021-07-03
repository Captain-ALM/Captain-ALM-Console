using System;
using System.Collections.Generic;


namespace captainalm.calmcmd
{
    /// <summary>
    /// Provides a registry for the API and applications.
    /// </summary>
    public sealed class Registry
    {
        internal static List<ICommand> _commands = new List<ICommand>();
        internal static List<ISyntax> _syntaxes = new List<ISyntax>();

        /// <summary>
        /// Adds a command to the Console
        /// </summary>
        /// <param name="commandIn">The command to add</param>
        /// <exception cref="System.ArgumentNullException">The parameter is null</exception>
        /// <exception cref="captainalm.calmcmd.CaptainALMConsoleException">The command already exists</exception>
        public static void registerCommand(ICommand commandIn)
        {
            if (Object.ReferenceEquals(commandIn, null)) throw new ArgumentNullException("commandIn");
            if (_commands.Exists(delegate(ICommand x) { return NameComparer(x, commandIn); })) throw new CaptainALMConsoleException("Duplicated Command", new ArgumentException("commandIn"));
            _commands.Add(commandIn);
        }

        /// <summary>
        /// Adds a syntax to the Console
        /// </summary>
        /// <param name="syntaxIn">The syntax to add</param>
        /// <exception cref="System.ArgumentNullException">The parameter is null</exception>
        /// <exception cref="captainalm.calmcmd.CaptainALMConsoleException">The syntax already exists</exception>
        public static void registerSyntax(ISyntax syntaxIn)
        {
            if (Object.ReferenceEquals(syntaxIn, null)) throw new ArgumentNullException("syntaxIn");
            if (_syntaxes.Exists(delegate(ISyntax x) { return NameComparer(x, syntaxIn); })) throw new CaptainALMConsoleException("Duplicated Syntax", new ArgumentException("syntaxIn"));
            _syntaxes.Add(syntaxIn);
        }

        private static bool NameComparer(IName x, IName y)
        {
           return (x.owner + "." + x.name).Equals(y.owner + "." + y.name);
        }
    }
}
