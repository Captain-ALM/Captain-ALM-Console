using System;
using System.Collections.Generic;
using System.Threading;

namespace captainalm.calmcmd
{
    /// <summary>
    /// Provides processing for the Console system.
    /// </summary>
    public static class Processor
    {
        internal static ISyntax currentSyntax;
        internal static object slockCommandStack = new object();
        internal static Stack<string> CommandStack = new Stack<string>();
        internal static InputRequired _ir;
        internal static object slockir = new object();

        /// <summary>
        /// Provides a delegate to return an object from an executed command
        /// </summary>
        /// <param name="objOut">The returned object</param>
        public delegate void CommandReturner(object objOut);
        /// <summary>
        /// This event is raised when a command completes execution successfully
        /// </summary>
        public static event CommandReturner CommandOutput;
        /// <summary>
        /// This event is raised when a command fails execution
        /// </summary>
        public static event CommandReturner CommandError;
        /// <summary>
        /// Provides a delegate to return then status of the command remaing count 
        /// </summary>
        /// <param name="commandsRemainingOut">The number of commands remaining</param>
        public delegate void StatusReturner(int commandsRemainingOut);
        /// <summary>
        /// This event is raised when the commands remaining is updated
        /// </summary>
        public static event StatusReturner StatusUpdate;
        /// <summary>
        /// Provides a delegate to return a string which was entered by the user
        /// </summary>
        /// <returns>The entered string</returns>
        public delegate string InputRequired();
        /// <summary>
        /// This property is used to hold the InputRequired delegate that's raised when an input is required
        /// </summary>
        public static InputRequired InputRequiredHandler
        {
            get
            {
                lock (slockir)
                {
                    return _ir;
                }
            }
            set
            {
                lock (slockir)
                {
                    _ir = value;
                }
            }
        }

        /// <summary>
        /// Executes the given command string using the current syntax
        /// </summary>
        /// <param name="cmdstr">The command string</param>
        /// <returns>The result of the command</returns>
        public static object executeCommand(string cmdstr)
        {
            lock (API.slocksyntax)
            {
                if (object.ReferenceEquals(currentSyntax, null)) return cmdstr;
            }
            var com = API.invalidCommandName;
            var args = new string[0];
            var dr = false;
            lock (API.slocksyntax)
            {
                dr = currentSyntax.decode(cmdstr, ref com, ref args);
            }
            if (dr)
            {
                ICommand cmd = Registry.getCommand(com);
                if (object.ReferenceEquals(cmd, null)) return API.invalidCommand.run(new object[0]);
                if (object.ReferenceEquals(args, null)) args = new string[0];
                var argsp = new object[args.Length];
                for (int i = 0; i < args.Length; i++) argsp[i] = executeCommand(args[i]);
                return cmd.run(argsp);
            }
            else
            {
                lock (API.slocksyntax)
                {
                    return currentSyntax.argumentTypeConversion(cmdstr);
                }
            }
        }
        /// <summary>
        /// Adds a command to the stack
        /// </summary>
        /// <param name="cmdstr">The command string to add</param>
        public static void pushCommand(string cmdstr)
        {
            lock (slockCommandStack)
            {
                CommandStack.Push(cmdstr);
            }
        }
        /// <summary>
        /// Executes the stacked commands while the keepExecuting paramter is true
        /// </summary>
        /// <param name="keepExecuting">A reference to keep executing</param>
        public static void executeCommands(ref bool keepExecuting)
        {
            API.InvokeConsoleStart();
            while (keepExecuting)
            {
                try
                {
                    Thread.Sleep(125);
                    while (CommandStack.Count > 0)
                    {
                        var cmdln = API.invalidCommandName;
                        var cs = StatusUpdate;
                        if (cs != null) cs.Invoke(CommandStack.Count);
                        lock (slockCommandStack)
                        {
                            cmdln = CommandStack.Pop();
                        }
                        if (API.InvokeConsolePreCommand(cmdln))
                        {
                            try
                            {
                                object toret = executeCommand(cmdln);
                                var c = CommandOutput;
                                API.InvokeConsolePostCommand(cmdln, toret);
                                if (c != null) c.Invoke(toret);
                            }
                            catch (ThreadAbortException ex) { throw ex; }
                            catch (Exception ex) { var c = CommandError; if (c != null) c.Invoke(ex); }
                        }
                        cs = StatusUpdate;
                        if (cs != null) cs.Invoke(CommandStack.Count);
                    }
                }
                catch (ThreadAbortException ex) { throw ex; }
                catch (Exception ex) { }
            }
            API.InvokeConsoleEnd();
        }
    }
}
