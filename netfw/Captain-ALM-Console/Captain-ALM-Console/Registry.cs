using System;
using System.Collections.Generic;


namespace captainalm.calmcmd
{
    /// <summary>
    /// Provides a registry for the API and applications.
    /// </summary>
    public static class Registry
    {
        private static List<ICommand> _commands = new List<ICommand>();
        private static List<ISyntax> _syntaxes = new List<ISyntax>();
        private static object slockcommands = new object();
        private static object slocksyntaxes = new object();

        /// <summary>
        /// Adds a command to the Console
        /// </summary>
        /// <param name="commandIn">The command to add</param>
        /// <exception cref="System.ArgumentNullException">The parameter is null</exception>
        /// <exception cref="captainalm.calmcmd.CaptainALMConsoleException">The command already exists</exception>
        public static void registerCommand(ICommand commandIn)
        {
            if (Object.ReferenceEquals(commandIn, null)) throw new ArgumentNullException("commandIn");
            lock (slockcommands)
            {
                if (_commands.Exists(delegate(ICommand x) { return NameComparer(x, commandIn); })) throw new CaptainALMConsoleException("Duplicated Command", new ArgumentException("commandIn"));
                _commands.Add(commandIn);
            }
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
            lock (slocksyntaxes)
            {
                if (_syntaxes.Exists(delegate(ISyntax x) { return NameComparer(x, syntaxIn); })) throw new CaptainALMConsoleException("Duplicated Syntax", new ArgumentException("syntaxIn"));
                _syntaxes.Add(syntaxIn);
            }
        }
        /// <summary>
        /// Removes a command from the console
        /// </summary>
        /// <param name="commandIn">The command to remove</param>
        /// <exception cref="System.ArgumentNullException">The parameter is null</exception>
        /// <exception cref="captainalm.calmcmd.CaptainALMConsoleException">The command does not exists</exception>
        public static void removeCommand(ICommand commandIn)
        {
            if (Object.ReferenceEquals(commandIn, null)) throw new ArgumentNullException("commandIn");
            lock (slockcommands)
            {
                if (!_commands.Exists(delegate(ICommand x) { return NameComparer(x, commandIn); })) throw new CaptainALMConsoleException("No Command", new ArgumentException("commandIn"));
                _commands.RemoveAll(delegate(ICommand x) { return NameComparer(x, commandIn); });
            }
        }
        /// <summary>
        /// Removes a syntax from the console
        /// </summary>
        /// <param name="syntaxIn">The syntax to remove</param>
        /// <exception cref="System.ArgumentNullException">The parameter is null</exception>
        /// <exception cref="captainalm.calmcmd.CaptainALMConsoleException">The syntax does not exists</exception>
        public static void removeSyntax(ISyntax syntaxIn)
        {
            if (Object.ReferenceEquals(syntaxIn, null)) throw new ArgumentNullException("syntaxIn");
            lock (slocksyntaxes)
            {
                if (!_syntaxes.Exists(delegate(ISyntax x) { return NameComparer(x, syntaxIn); })) throw new CaptainALMConsoleException("No Syntax", new ArgumentException("syntaxIn"));
                _syntaxes.RemoveAll(delegate(ISyntax x) { return NameComparer(x, syntaxIn); });
            }
        }
        /// <summary>
        /// Returns all the currently registered commands
        /// </summary>
        /// <returns>An array of registered commands</returns>
        public static ICommand[] getCommands()
        {
            lock (slockcommands)
            {
                return _commands.ToArray();
            }
        }
        /// <summary>
        /// Returns all the currently registered syntaxes
        /// </summary>
        /// <returns>An array of registered syntaxes</returns>
        public static ISyntax[] getSyntaxes()
        {
            lock (slocksyntaxes)
            {
                return _syntaxes.ToArray();
            }
        }
        /// <summary>
        /// Gets a command given its name (in the form owner.name)
        /// </summary>
        /// <param name="cmdIn">The command name in</param>
        /// <returns>The command</returns>
        public static ICommand getCommand(string cmdIn)
        {
            lock (slockcommands)
            {
                return _commands.Find(delegate(ICommand x) { return (x.owner + "." + x.name).Equals(cmdIn); });
            }
        }
        /// <summary>
        /// Gets a syntax given its name (in the form owner.name)
        /// </summary>
        /// <param name="synIn">The syntax name in</param>
        /// <returns>The syntax</returns>
        public static ISyntax getSyntax(string synIn)
        {
            lock (slocksyntaxes)
            {
                return _syntaxes.Find(delegate(ISyntax x) { return (x.owner + "." + x.name).Equals(synIn); });
            }
        }

        private static bool NameComparer(IName x, IName y)
        {
           return (x.owner + "." + x.name).Equals(y.owner + "." + y.name);
        }
    }
}
