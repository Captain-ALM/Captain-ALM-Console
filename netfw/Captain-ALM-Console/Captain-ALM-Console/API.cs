using System;
using System.Collections.Generic;
using System.Threading;

namespace captainalm.calmcmd
{
    /// <summary>
    /// Provides the API to be accessed by extension libraries.
    /// </summary>
    public static class API
    {
        internal static object slocksyntax = new object();
        internal static string currentSyntaxName = "";
        internal static Dictionary<string, string> StringVariableDictionary = new Dictionary<string, string>();
        internal static object slocksvd = new object();
        private static Dictionary<string, object> VariableDictionary = new Dictionary<string, object>();
        private static object slockvd = new object();
        internal static string _invalidCommandName = "invalid";

        /// <summary>
        /// Provides a delegate to be used by program events
        /// </summary>
        public delegate void ProgramEvent();
        /// <summary>
        /// This event is raised when the console has started after all libraries have been loaded
        /// </summary>
        public static event ProgramEvent ConsoleStart;
        internal static void InvokeConsoleStart()
        {
            try
            {
                var ev = ConsoleStart;
                if (ev != null) ev.Invoke();
            }
            catch (ThreadAbortException ex) { throw ex; }
            catch (Exception ex) { }
        }
        /// <summary>
        /// This event is raised when the console is ending before it terminates
        /// </summary>
        public static event ProgramEvent ConsoleEnd;
        internal static void InvokeConsoleEnd()
        {
            try
            {
                var ev = ConsoleEnd;
                if (ev != null) ev.Invoke();
            }
            catch (ThreadAbortException ex) { throw ex; }
            catch (Exception ex) { }
        }
        /// <summary>
        /// Provides a delegate to be used before a command is executed
        /// </summary>
        /// <param name="cmdln">The command string out</param>
        /// <returns>If the command should be executed</returns>
        public delegate bool PreCommandExecute(string cmdln);
        /// <summary>
        /// This event is raised before the console calls a queued command
        /// This is not called for commands executed by <see cref="captainalm.calmcmd.Processor.executeCommand">execute command</see>.
        /// </summary>
        public static event PreCommandExecute ConsolePreCommand;
        internal static bool InvokeConsolePreCommand(string cmdln)
        {
            try
            {
                var ev = ConsolePreCommand;
                if (ev != null)
                {
                    var toret = true;
                    var il = (PreCommandExecute[])ev.GetInvocationList();
                    foreach (var m in il)
                    {
                        toret &= m.Invoke(cmdln);
                    }
                    return toret;
                }
            }
            catch (ThreadAbortException ex) { throw ex; }
            catch (Exception ex) { }
            return true; //Carry on executing the command on an exception
        }
        /// <summary>
        /// Provides a delegate to be used after a command is executed
        /// </summary>
        /// <param name="cmdln">The command string out</param>
        /// <param name="objOut">The returned object from the command</param>
        public delegate void PostCommandExecute(string cmdln, object objOut);
        /// <summary>
        /// This event is raised after the console calls a queued command successfully
        /// This is not called for commands executed by <see cref="captainalm.calmcmd.Processor.executeCommand">execute command</see>.
        /// </summary>
        public static event PostCommandExecute ConsolePostCommand;
        internal static void InvokeConsolePostCommand(string cmdln, object objOut)
        {
            try
            {
                var ev = ConsolePostCommand;
                if (ev != null) ev.Invoke(cmdln, objOut);
            }
            catch (ThreadAbortException ex) { throw ex; }
            catch (Exception ex) { }
        }

        /// <summary>
        /// Gets the current syntax name (in the form owner.name)
        /// </summary>
        /// <returns>The current syntax name</returns>
        public static string getCurrentSyntax()
        {
            return currentSyntaxName;
        }
        /// <summary>
        /// Gets an array of the current registered syntax names (each in the form owner.name)
        /// </summary>
        /// <returns>An array of syntax names</returns>
        public static string[] getSyntaxes()
        {
            var ls = new List<string>();
            var sn = Registry.getSyntaxes();
            foreach (ISyntax c in sn)
            {
                ls.Add((c.owner.Equals("")) ? c.name : c.owner + "." + c.name);
            }
            return ls.ToArray();
        }
        /// <summary>
        /// Sets the current syntax using its name (in the form owner.name)
        /// </summary>
        /// <param name="nameIn">The name of the syntax</param>
        /// <returns>If the operation completed</returns>
        public static bool setSyntax(string nameIn)
        {
            if (nameIn.Equals(""))
            {
                lock (slocksyntax)
                {
                    Processor.currentSyntax = null;
                }
                currentSyntaxName = nameIn;
                return true;
            }
            ISyntax st = Registry.getSyntax(nameIn);
            if (st != null)
            {
                lock (slocksyntax)
                {
                    Processor.currentSyntax = st;
                }
                currentSyntaxName = nameIn;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets an array of the current registered command names (each in the form owner.name)
        /// </summary>
        /// <returns>An array of command names</returns>
        public static string[] getCommands()
        {
            var ls = new List<string>();
            var sn = Registry.getCommands();
            foreach (ICommand c in sn)
            {
                ls.Add((c.owner.Equals("")) ? c.name : c.owner + "." + c.name);
            }
            return ls.ToArray();
        }

        /// <summary>
        /// Sets a value of the string variable storage system
        /// </summary>
        /// <param name="name">The name of the variable</param>
        /// <param name="valueIn">The value to store</param>
        public static void setStringVariable(string name, string valueIn)
        {
            lock (slocksvd)
            {
                if (StringVariableDictionary.ContainsKey(name))
                {
                    StringVariableDictionary[name] = valueIn;
                }
                else
                {
                    StringVariableDictionary.Add(name, valueIn);
                }
            }
        }
        /// <summary>
        /// Gets a value from the string variable storage system
        /// </summary>
        /// <param name="name">The name of the variable</param>
        /// <returns>The value obtained</returns>
        public static string getStringVariable(string name)
        {
            lock (slocksvd)
            {
                if (StringVariableDictionary.ContainsKey(name))
                {
                    return StringVariableDictionary[name];
                }
            }
            return "";
        }
        /// <summary>
        /// Clears the string variable storage system
        /// </summary>
        public static void clearStringVariables()
        {
            lock (slocksvd)
            {
                StringVariableDictionary.Clear();
            }
        }

        /// <summary>
        /// Sets a value of the object variable storage system
        /// </summary>
        /// <param name="name">The name of the variable</param>
        /// <param name="valueIn">The value to store</param>
        public static void setVariable(string name, object valueIn)
        {
            lock (slockvd)
            {
                if (VariableDictionary.ContainsKey(name))
                {
                    VariableDictionary[name] = valueIn;
                }
                else
                {
                    VariableDictionary.Add(name, valueIn);
                }
            }
        }
        /// <summary>
        /// Gets a value from the object variable storage system
        /// </summary>
        /// <param name="name">The name of the variable</param>
        /// <returns>The value obtained</returns>
        public static object getVariable(string name)
        {
            lock (slockvd)
            {
                if (VariableDictionary.ContainsKey(name))
                {
                    return VariableDictionary[name];
                }
            }
            return null;
        }
        /// <summary>
        /// Clears the object variable storage system
        /// </summary>
        public static void clearVariables()
        {
            lock (slockvd)
            {
                VariableDictionary.Clear();
            }
        }

        /// <summary>
        /// Gets the name of the invalid command
        /// </summary>
        public static string invalidCommandName
        {
            get
            {
                return _invalidCommandName;
            }
        }
        /// <summary>
        /// Gets the invalid command
        /// </summary>
        public static ICommand invalidCommand
        {
            get
            {
                return Registry.getCommand(_invalidCommandName);
            }
        }
    }
}
