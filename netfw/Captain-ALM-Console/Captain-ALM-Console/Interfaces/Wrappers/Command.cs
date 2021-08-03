using System;

namespace captainalm.calmcmd
{
    /// <summary>
    /// Wraps a command delegate with an <see cref="captainalm.calmcmd.ICommand">ICommand</see> wrapper.
    /// </summary>
    public sealed class Command : ICommand
    {
        private CommandDelegate com;
        private string _name;
        private string _owner;
        private string _help;

        /// <summary>
        /// Provides a command delegate to be used in the wrapper to represent a runnable function for the command
        /// </summary>
        /// <param name="argumentsIn">The arguments in</param>
        /// <returns>The result</returns>
        public delegate object CommandDelegate(object[] argumentsIn);

        /// <summary>
        /// Constructs a new instance of the Command wrapper with the command delegate, name and help.
        /// </summary>
        /// <param name="commandIn">The command delegate</param>
        /// <param name="owner">The command owner</param>
        /// <param name="name">The command name</param>
        /// <param name="help">The command help message</param>
        /// <exception cref="System.ArgumentNullException">a provided parameter is null</exception>
        public Command(CommandDelegate commandIn, string owner, string name, string help)
        {
            if (object.ReferenceEquals(commandIn, null)) { throw new ArgumentNullException("commandIn"); }
            com = commandIn;
            this.owner = owner;
            this.name = name;
            this.help = help;
        }

        /// <summary>
        /// Runs the command with the specified argument array returning the result
        /// </summary>
        /// <param name="argumentsIn">The arguments in</param>
        /// <returns>The result</returns>
        public object run(object[] argumentsIn)
        {
            return com.Invoke(argumentsIn);
        }

        /// <summary>
        /// Returns the help string for this command
        /// </summary>
        /// <exception cref="System.ArgumentNullException">the provided parameter is null</exception>
        public string help
        {
            get { return _help; }
            set { if (object.ReferenceEquals(value, null)) { throw new ArgumentNullException("value"); } _help = value; }
        }
        /// <summary>
        /// Returns the name of the command
        /// </summary>
        /// <exception cref="System.ArgumentNullException">the provided parameter is null</exception>
        public string name
        {
            get { return _name; }
            set { if (object.ReferenceEquals(value, null)) { throw new ArgumentNullException("value"); } _name = value; }
        }
        /// <summary>
        /// Returns the owner of the command
        /// </summary>
        /// <exception cref="System.ArgumentNullException">the provided parameter is null</exception>
        public string owner
        {
            get { return _owner; }
            set { if (object.ReferenceEquals(value, null)) { throw new ArgumentNullException("value"); } _owner = value; }
        }
    }
}
