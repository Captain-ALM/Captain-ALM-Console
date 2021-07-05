using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace captainalm.calmcmd
{
    /// <summary>
    /// Provides the API to be accessed by extension libraries.
    /// </summary>
    public static class API
    {
        private static object slockssyntax = new object();
        internal static string currentSyntaxName;
        internal static Dictionary<string, string> StringVariableDictionary = new Dictionary<string, string>();
        private static Dictionary<string, object> VariableDictionary = new Dictionary<string, object>();
        /// <summary>
        /// Provides a delegate to be used by program events
        /// </summary>
        public delegate void ProgramEvent();
        /// <summary>
        /// This event is raised when the console has started after all libraries have been loaded
        /// </summary>
        public static event ProgramEvent ConsoleStart;
        /// <summary>
        /// This event is raised when the console is ending before it terminates
        /// </summary>
        public static event ProgramEvent ConsoleEnd;
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
                ls.Add(c.owner + "." + c.name);
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
            ISyntax st = Registry.getSyntax(nameIn);
            if (st != null)
            {
                lock (slockssyntax)
                {
                    Processor.currentSyntax = st;
                }
                currentSyntaxName = st.owner + "." + st.name;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Sets a value of the string variable storage system
        /// </summary>
        /// <param name="name">The name of the variable</param>
        /// <param name="valueIn">The value to store</param>
        public static void setStringVariable(string name, string valueIn)
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
        /// <summary>
        /// Gets a value from the string variable storage system
        /// </summary>
        /// <param name="name">The name of the variable</param>
        /// <returns>The value obtained</returns>
        public static string getStringVariable(string name)
        {
            if (StringVariableDictionary.ContainsKey(name))
            {
                return StringVariableDictionary[name];
            }
            return "";
        }
        /// <summary>
        /// Clears the string variable storage system
        /// </summary>
        public static void clearStringVariables()
        {
            StringVariableDictionary.Clear();
        }
        /// <summary>
        /// Sets a value of the object variable storage system
        /// </summary>
        /// <param name="name">The name of the variable</param>
        /// <param name="valueIn">The value to store</param>
        public static void setVariable(string name, object valueIn)
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
        /// <summary>
        /// Gets a value from the object variable storage system
        /// </summary>
        /// <param name="name">The name of the variable</param>
        /// <returns>The value obtained</returns>
        public static object getVariable(string name)
        {
            if (VariableDictionary.ContainsKey(name))
            {
                return VariableDictionary[name];
            }
            return null;
        }
        /// <summary>
        /// Clears the object variable storage system
        /// </summary>
        public static void clearVariables()
        {
            VariableDictionary.Clear();
        }
    }
}
