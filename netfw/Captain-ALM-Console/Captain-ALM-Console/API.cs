using System;
using System.Collections.Generic;
using System.Reflection;
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
            catch (Exception) { }
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
            catch (Exception) { }
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
            catch (Exception) { }
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
            catch (Exception) { }
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
            if (! object.ReferenceEquals(st, null))
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

        /// <summary>
        /// Provides a function to convert a Legacy OutputText or OutputTextBlock to a StylableString
        /// </summary>
        /// <param name="objIn">The OutputText or OutputTextBlock to convert</param>
        /// <returns>The converted to stylable string</returns>
        /// <exception cref="System.ArgumentNullException">objIn is null</exception>
        public static StylableString convertOutputTextToStylableString(object objIn)
        {
            if (object.ReferenceEquals(objIn, null)) throw new ArgumentNullException("objIn");
            return Loader.convertOutputTextToStylableString(objIn);
        }

        /// <summary>
        /// Raises the <see cref="captainalm.calmcmd.CaptainALMConsoleException">CaptainALMConsoleException</see> with the specified parameters (If any)
        /// </summary>
        /// <param name="msg">The message to use</param>
        /// <param name="exp">The inner exception to use</param>
        public static void raiseCaptainALMConsoleException(string msg = null, Exception exp = null)
        {
            if (object.ReferenceEquals(msg, null)) throw new CaptainALMConsoleException();
            if (object.ReferenceEquals(exp, null)) throw new CaptainALMConsoleException(msg);
            throw new CaptainALMConsoleException(msg, exp);
        }
        /// <summary>
        /// Requests a user input using <see cref="captainalm.calmcmd.Processor.InputRequiredHandler">Processor.InputRequiredHandler</see>
        /// </summary>
        /// <param name="convertUsingCurrentSyntax">Convert the string input taken using the type conversion of the current syntax</param>
        /// <returns>The user input (Which may be converted to an object) or null if it fails</returns>
        public static object requestUserInput(bool convertUsingCurrentSyntax = false)
        {
            lock (Processor.slockir)
            {
                if (!object.ReferenceEquals(Processor._ir, null))
                {
                    var ret = Processor._ir.Invoke();
                    if (convertUsingCurrentSyntax)
                    {
                        lock (slocksyntax)
                        {
                            if (object.ReferenceEquals(Processor.currentSyntax, null)) return ret; else return Processor.currentSyntax.argumentTypeConversion(ret);
                        }
                    }
                    else
                    {
                        return ret;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Checks the types of the items in the args array against the types in the type array for convertability
        /// Use null or void in types to represent requiring a null parameter
        /// Use object to accept all types including null
        /// Also checks if the input arrays are the same length
        /// </summary>
        /// <param name="args">The arguments to assert</param>
        /// <param name="types">The types to test with</param>
        /// <exception cref="System.ArgumentNullException">args or types is null</exception>
        /// <exception cref="captainalm.calmcmd.CaptainALMConsoleException">args assertion failure</exception>
        public static void assertArguments(object[] args, Type[] types)
        {
            if (object.ReferenceEquals(args, null)) throw new ArgumentNullException("args");
            if (object.ReferenceEquals(types, null)) throw new ArgumentNullException("types");
            if (args.Length != types.Length) throw new CaptainALMConsoleException("The argument count is incorrect.");
            for (int i = 0; i < args.Length; i++)
            {
                if ((! object.ReferenceEquals(args[i], null)) && (! object.ReferenceEquals(types[i], null)))
                {
                    if ((types[i] != typeof(object)) && (! isAssignableFrom(types[i], args[i].GetType()))) throw new CaptainALMConsoleException("The argument at position " + (i + 1).ToString() + " is of the type " + args[i].GetType().FullName + " it should be of the type " + types[i].FullName + " .");
                }
                else if (! (object.ReferenceEquals(args[i], null) && object.ReferenceEquals(types[i], null)))
                {
                    if (object.ReferenceEquals(args[i], null)) {
                        if (types[i] != typeof(void) && types[i] != typeof(object)) throw new CaptainALMConsoleException("The argument at position " + (i + 1).ToString() + " is of the type null it should be of the type " + types[i].FullName + " .");
                    }
                    else if (object.ReferenceEquals(types[i], null))
                    {
                        throw new CaptainALMConsoleException("The argument at position " + (i + 1).ToString() + " is of the type " + args[i].GetType().FullName + " it should be of the type null .");
                    }
                }
            }
        }
        private static bool isAssignableFrom(Type to, Type from)
        {
            if (to == from) return true;
            if (to.IsAssignableFrom(from)) return true;
            var fromsm = from.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            foreach (var c in fromsm) if (c.Name.Equals("op_Implicit") && c.ReturnType == to) return true;
            var tosm = to.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
            foreach (var c in tosm) if (c.Name.Equals("op_Implicit") && c.GetParameters().Length > 0 && c.GetParameters()[0].ParameterType == from) return true;
            if ((from == typeof(sbyte)) && (to == typeof(short) || to == typeof(int) || to == typeof(long) || to == typeof(float) || to == typeof(double) || to == typeof(decimal))) return true;
            if ((from == typeof(byte)) && (to == typeof(short) || to == typeof(int) || to == typeof(long) || to == typeof(float) || to == typeof(double) || to == typeof(decimal) || to == typeof(ushort) || to == typeof(uint)) || to == typeof(ulong)) return true;
            if ((from == typeof(short)) && (to == typeof(int) || to == typeof(long) || to == typeof(float) || to == typeof(double) || to == typeof(decimal))) return true;
            if ((from == typeof(ushort)) && (to == typeof(int) || to == typeof(long) || to == typeof(float) || to == typeof(double) || to == typeof(decimal) || to == typeof(uint)) || to == typeof(ulong)) return true;
            if ((from == typeof(int)) && (to == typeof(long) || to == typeof(float) || to == typeof(double) || to == typeof(decimal))) return true;
            if ((from == typeof(uint)) && (to == typeof(long) || to == typeof(float) || to == typeof(double) || to == typeof(decimal) || to == typeof(ulong))) return true;
            if ((from == typeof(long)) && (to == typeof(float) || to == typeof(double) || to == typeof(decimal))) return true;
            if ((from == typeof(ulong)) && (to == typeof(float) || to == typeof(double) || to == typeof(decimal))) return true;
            if ((from == typeof(float)) && (to == typeof(double))) return true;
            return false;
        }
        /// <summary>
        /// Checks if the references of the items in the args array for being null against the array of if they are allowed to be null
        /// Also checks if the input arrays are the same length
        /// </summary>
        /// <param name="args">The arguments to assert</param>
        /// <param name="isNull">The boolean values of if each item is allowed be null</param>
        /// <exception cref="System.ArgumentNullException">args or isNull is null</exception>
        /// <exception cref="captainalm.calmcmd.CaptainALMConsoleException">args assertion failure</exception>
        public static void assertNullArguments(object[] args, bool[] isNull)
        {
            if (object.ReferenceEquals(args, null)) throw new ArgumentNullException("args");
            if (object.ReferenceEquals(isNull, null)) throw new ArgumentNullException("isNull");
            if (args.Length != isNull.Length) throw new CaptainALMConsoleException("The argument count is incorrect.");
            for (int i = 0; i < args.Length; i++)
            {
                if (object.ReferenceEquals(args[i], null) && !isNull[i]) throw new CaptainALMConsoleException("The argument at position " + (i + 1).ToString() + " is null and it should be not null .");
            }
        }
    }
}
