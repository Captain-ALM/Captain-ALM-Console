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
        /// Executes the given command string using the current syntax
        /// </summary>
        /// <param name="cmdstr">The command string</param>
        /// <returns>The result of the command</returns>
        public static object executeCommand(string cmdstr)
        {
            if (object.ReferenceEquals(currentSyntax, null)) return cmdstr;
            var com = "";
            var args = new string[0];
            if (currentSyntax.decode(cmdstr, ref com, ref args))
            {
                if (object.ReferenceEquals(args, null)) args = new string[0];
                var argsp = new object[args.Length];
                for (int i = 0; i < args.Length; i++) argsp[i] = executeCommand(args[i]);
                ICommand cmd = Registry.getCommand(com);
                if (object.ReferenceEquals(cmd, null)) return currentSyntax.argumentTypeConversion(cmdstr);
                return cmd.run(argsp);
            }
            else
            {
                return currentSyntax.argumentTypeConversion(cmdstr);
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
            while (keepExecuting)
            {
                try
                {
                    Thread.Sleep(125);
                    while (CommandStack.Count > 0)
                    {
                        var cmdln = "";
                        var cs = StatusUpdate;
                        if (cs != null) cs.Invoke(CommandStack.Count);
                        lock (slockCommandStack)
                        {
                            cmdln = CommandStack.Pop();
                        }
                        try
                        {
                            object toret = executeCommand(cmdln);
                            var c = CommandOutput;
                            if (c != null) c.Invoke(toret);
                        }
                        catch (ThreadAbortException ex) { throw ex; }
                        catch (Exception ex) { var c = CommandError; if (c != null) c.Invoke(ex); }
                        cs = StatusUpdate;
                        if (cs != null) cs.Invoke(CommandStack.Count);
                    }
                }
                catch (ThreadAbortException ex) { throw ex; }
                catch (Exception ex) { }
            }
        }
    }
}
