using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace captainalm.calmcmd
{
    internal sealed class LegacyLoader
    {
        public LegacyHookHolder hookHolder = new LegacyHookHolder();
        public bool registerFirstLegacySyntaxesToStandardLibrary = true;
        public bool registerFirstLegacyCommandsToStandardLibrary = true;

        public bool loadAssembly(Assembly assemblyIn)
        {
            if (object.ReferenceEquals(assemblyIn, null)) return false;
            var toret = true;
            foreach (Type c in assemblyIn.GetTypes())
            {
                if (!c.IsAbstract & !c.IsInterface)
                {
                    toret &= typeProcessor(c);
                }
            }
            return toret;
        }

        private bool typeProcessor(Type typeIn)
        {
            MethodInfo initMethod = null;
            object instanceType = null;
            captainalm.calmcon.api.LibrarySetup libs;
            try
            {
                foreach (MethodInfo c in typeIn.GetMethods(BindingFlags.Public | BindingFlags.Instance))
                {
                    captainalm.calmcon.api.SetupMethodAttribute[] aarr = (captainalm.calmcon.api.SetupMethodAttribute[])c.GetCustomAttributes(typeof(captainalm.calmcon.api.SetupMethodAttribute), false);
                    if (aarr.Length != 0 & initMethod.ReturnType == typeof(captainalm.calmcon.api.LibrarySetup)) { initMethod = c; }
                }
                instanceType = typeIn.Assembly.CreateInstance(typeIn.FullName);
            }
            catch (ThreadAbortException ex) { throw ex; }
            catch (Exception ex) { return false; }
            if (initMethod != null && instanceType != null)
            {
                try
                {
                    libs = (captainalm.calmcon.api.LibrarySetup)initMethod.Invoke(instanceType, null);
                }
                catch (ThreadAbortException ex) { throw ex; }
                catch (Exception ex) { return false; }
                if (!hookHolder.hookInformation.ContainsKey(libs.hook_info.name)) hookHolder.hookInformation.Add(libs.hook_info.name, libs.hook_info);
                if (libs.syntaxes != null) {
                    foreach (captainalm.calmcon.api.ISyntax c in libs.syntaxes)
                    {
                        if (registerFirstLegacySyntaxesToStandardLibrary && Registry.getSyntax(c.name()) == null)
                        {
                            var lstsstd = new LegacySyntax(c, "");
                            Registry.registerSyntax(lstsstd);
                        }
                        var lsts = new LegacySyntax(c, libs.name);
                        Registry.registerSyntax(lsts);
                    } 
                }
                if (libs.commands != null)
                {
                    foreach (captainalm.calmcon.api.Command c in libs.commands)
                    {
                        if (registerFirstLegacyCommandsToStandardLibrary && Registry.getCommand(c.name) == null)
                        {
                            var lctsstd = new LegacyCommand(c, "");
                            Registry.registerCommand(lctsstd);
                        }
                        var lcts = new LegacyCommand(c, libs.name);
                        Registry.registerCommand(lcts);
                    }
                }

            }
            return true;
        }
    }
    /// <summary>
    /// Holds the hook database of the legacy loader for 'safer' external use.
    /// </summary>
    public sealed class LegacyHookHolder
    {
        /// <summary>
        /// The hook information dictionary
        /// </summary>
        public readonly Dictionary<String, captainalm.calmcon.api.HookInfo> hookInformation = new Dictionary<string, captainalm.calmcon.api.HookInfo>();
    }

    internal sealed class LegacySyntax : ISyntax
    {
        public captainalm.calmcon.api.ISyntax wrappedSyntax;
        private string _owner;

        public LegacySyntax(captainalm.calmcon.api.ISyntax syntIn, string owner)
        {
            wrappedSyntax = syntIn;
            _owner = owner;
        }

        public bool decode(string dataIn, ref string commandOut, ref string[] argumentsOut)
        {
            if ((dataIn.StartsWith("'") & dataIn.EndsWith("'")) | (dataIn.StartsWith("\"") & dataIn.EndsWith("\""))) return false;
            var ret = wrappedSyntax.decrypt(dataIn, new List<String>(API.getCommands()));
            if (object.ReferenceEquals(ret, null) || ret.Count == 0) return false;
            commandOut = ret[0];
            if (object.ReferenceEquals(commandOut, null)) return false;
            if (ret.Count > 1)
            {
                argumentsOut = new string[ret.Count - 1];
                for (int i = 1; i < ret.Count; i++)
                {
                    argumentsOut[i - 1] = ret[i];
                }
            }
            return true;
        }

        public object argumentTypeConversion(string argumentIn)
        {
            return ((argumentIn.StartsWith("'") & argumentIn.EndsWith("'")) | (argumentIn.StartsWith("\"") & argumentIn.EndsWith("\""))) ? argumentIn.Substring(1, argumentIn.Length - 2) : argumentIn;
        }

        public string name
        {
            get { return wrappedSyntax.name(); }
        }

        public string owner
        {
            get { return _owner; }
        }
    }

    internal sealed class LegacyCommand : ICommand
    {
        public captainalm.calmcon.api.Command wrappedCommand;
        private string _owner;

        public LegacyCommand(captainalm.calmcon.api.Command comIn, string owner)
        {
            wrappedCommand = comIn;
            _owner = owner;
        }

        public object run(object[] argumentsIn)
        {
            var ts = new string[argumentsIn.Length];
            if (argumentsIn.Length > 0)
            {
                for (int i = 0; i < argumentsIn.Length; i++)
                {
                    ts[i] = argumentsIn[i].ToString();
                }
            }
            return wrappedCommand.command.Invoke(ts);
        }

        public string help
        {
            get { return wrappedCommand.help; }
        }

        public string name
        {
            get { return wrappedCommand.name; }
        }

        public string owner
        {
            get { return _owner; }
        }
    }
}
