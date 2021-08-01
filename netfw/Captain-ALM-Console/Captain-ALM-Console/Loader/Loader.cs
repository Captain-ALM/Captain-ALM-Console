using System;
using System.Reflection;
using System.Threading;

namespace captainalm.calmcmd
{
    /// <summary>
    /// Defines the assembly loader class for the console system.
    /// </summary>
    public static class Loader
    {
        private static LegacyLoader ll;
        private static bool? lav = null;
        private static object slockll = new object();

        /// <summary>
        /// Checks if legacy systems are supported
        /// Required to be called to create the loader if support is available
        /// </summary>
        /// <returns>Whether legacy systems are supported</returns>
        public static bool isLegacySupported()
        {
            if (lav.HasValue) return lav.Value;
            var toret = false;
            try
            {
                toret = LegacyLoader.legacySupported();
                if (toret)
                {
                    lock (slockll) { lav = true; ll = new LegacyLoader(); }
                }
            }
            catch (ThreadAbortException ex) { throw ex; }
            catch (Exception) { toret = false; }
            if (!toret)
            {
                lock (slockll) { lav = false; }
            }
            return toret;
        }

        internal static StylableString convertOutputTextToStylableString(object objIn)
        {
            if ((!lav.HasValue) || lav == false) return default(StylableString);
            lock (slockll)
            {
                try
                {
                    if (!object.ReferenceEquals(ll, null))
                    {
                        return ll.convertOutputTextToStylableString(objIn);
                    }
                }
                catch (ThreadAbortException ex) { throw ex; }
                catch (Exception)
                {
                    lav = false;
                }
            }
            return default(StylableString);
        }

        internal static void startEvent()
        {
            if ((!lav.HasValue) || lav == false) return;
            lock (slockll)
            {
                try
                {
                    if (!object.ReferenceEquals(ll, null))
                    {
                        ll.executeStartHooks();
                    }
                }
                catch (ThreadAbortException ex) { throw ex; }
                catch (Exception)
                {
                    lav = false;
                }
            }
        }
        /// <summary>
        /// Registers the startEvent used by the legacy loading system
        /// </summary>
        public static void registerStartEvent()
        {
            API.ConsoleStart += startEvent;
        }
        /// <summary>
        /// Unregisters the startEvent used by the legacy loading system
        /// </summary>
        public static void unregisterStartEvent()
        {
            API.ConsoleStart -= startEvent;
        }

        internal static void endEvent()
        {
            if ((!lav.HasValue) || lav == false) return;
            lock (slockll)
            {
                try
                {
                    if (!object.ReferenceEquals(ll, null))
                    {
                        ll.executeEndHooks();
                    }
                }
                catch (ThreadAbortException ex) { throw ex; }
                catch (Exception)
                {
                    lav = false;
                }
            }
        }
        /// <summary>
        /// Registers the endEvent used by the legacy loading system
        /// </summary>
        public static void registerEndEvent()
        {
            API.ConsoleEnd += endEvent;
        }
        /// <summary>
        /// Unregisters the endEvent used by the legacy loading system
        /// </summary>
        public static void unregisterEndEvent()
        {
            API.ConsoleEnd -= endEvent;
        }

        internal static bool precmdEvent(string cmdln)
        {
            if ((!lav.HasValue) || lav == false) return true;
            lock (slockll)
            {
                try
                {
                    if (!object.ReferenceEquals(ll, null))
                    {
                        ll.executePreCHooks(cmdln);
                    }
                }
                catch (ThreadAbortException ex) { throw ex; }
                catch (Exception)
                {
                    lav = false;
                }
            }
            return true;
        }
        /// <summary>
        /// Registers the precmdEvent used by the legacy loading system
        /// </summary>
        public static void registerprecmdEvent()
        {
            API.ConsolePreCommand += precmdEvent;
        }
        /// <summary>
        /// Unregisters the precmdEvent used by the legacy loading system
        /// </summary>
        public static void unregisterprecmdEvent()
        {
            API.ConsolePreCommand -= precmdEvent;
        }

        internal static void postcmdEvent(string cmdln, object objOut)
        {
            if ((!lav.HasValue) || lav == false) return;
            lock (slockll)
            {
                try
                {
                    if (!object.ReferenceEquals(ll, null))
                    {
                        ll.executePostCHooks(cmdln, objOut);
                    }
                }
                catch (ThreadAbortException ex) { throw ex; }
                catch (Exception)
                {
                    lav = false;
                }
            }
        }
        /// <summary>
        /// Registers the postcmdEvent used by the legacy loading system
        /// </summary>
        public static void registerpostcmdEvent()
        {
            API.ConsolePostCommand += postcmdEvent;
        }
        /// <summary>
        /// Unregisters the postcmdEvent used by the legacy loading system
        /// </summary>
        public static void unregisterpostcmdEvent()
        {
            API.ConsolePostCommand -= postcmdEvent;
        }

        /// <summary>
        /// Gets the legacyHookHolder for the legacy system
        /// Returns null if legacy is not supported or isLegacySupported has not been called yet
        /// </summary>
        public static LegacyHookHolder legacyHookHolder
        {
            get
            {
                if ((!lav.HasValue) || lav == false) return null;
                lock (slockll)
                {
                    try
                    {
                        if (!object.ReferenceEquals(ll, null))
                        {
                            return ll.hookHolder;
                        }
                    }
                    catch (ThreadAbortException ex) { throw ex; }
                    catch (Exception)
                    {
                        lav = false;
                    }
                }
                return null;
            }
        }
        /// <summary>
        /// Gets or sets the legacyHookExecutorHolder used by the legacy system
        /// Returns the default value if legacy is not supported or isLegacySupported has not been called yet
        /// </summary>
        public static LegacyHookRunnerHolder legacyHookExecutorHolder
        {
            get
            {
                if ((!lav.HasValue) || lav == false) return default(LegacyHookRunnerHolder);
                lock (slockll)
                {
                    try
                    {
                        if (!object.ReferenceEquals(ll, null))
                        {
                            return ll.hookRunnerHolder;
                        }
                    }
                    catch (ThreadAbortException ex) { throw ex; }
                    catch (Exception)
                    {
                        lav = false;
                    }
                }
                return default(LegacyHookRunnerHolder);
            }
            set
            {
                if ((!lav.HasValue) || lav == false) return;
                lock (slockll)
                {
                    try
                    {
                        if (!object.ReferenceEquals(ll, null))
                        {
                            ll.hookRunnerHolder = value;
                        }
                    }
                    catch (ThreadAbortException ex) { throw ex; }
                    catch (Exception)
                    {
                        lav = false;
                    }
                }
            }
        }
        /// <summary>
        /// Gets or sets whether the first legacy syntax of a name registered is registered to the standard library if the syntax name is not already taken
        /// Returns false if legacy is not supported or isLegacySupported has not been called yet
        /// </summary>
        public static bool registerFirstLegacySyntaxesToStandardLibrary
        {
            get
            {
                if ((!lav.HasValue) || lav == false) return false;
                lock (slockll)
                {
                    if (!object.ReferenceEquals(ll, null))
                    {
                        return ll.registerFirstLegacySyntaxesToStandardLibrary;
                    }
                }
                return false;
            }
            set
            {
                if ((!lav.HasValue) || lav == false) return;
                lock (slockll)
                {
                    if (!object.ReferenceEquals(ll, null))
                    {
                        ll.registerFirstLegacySyntaxesToStandardLibrary = value;
                    }
                }
            }
        }
        /// <summary>
        /// Gets or sets whether the first legacy command of a name registered is registered to the standard library if the command name is not already taken
        /// Returns false if legacy is not supported or isLegacySupported has not been called yet
        /// </summary>
        public static bool registerFirstLegacyCommandsToStandardLibrary
        {
            get
            {
                if ((!lav.HasValue) || lav == false) return false;
                lock (slockll)
                {
                    if (!object.ReferenceEquals(ll, null))
                    {
                        return ll.registerFirstLegacyCommandsToStandardLibrary;
                    }
                }
                return false;
            }
            set
            {
                if ((!lav.HasValue) || lav == false) return;
                lock (slockll)
                {
                    if (!object.ReferenceEquals(ll, null))
                    {
                        ll.registerFirstLegacyCommandsToStandardLibrary = value;
                    }
                }
            }
        }

        /// <summary>
        /// Syncs the manipulator values to and from the legacy system
        /// Does nothing if legacy is not supported or isLegacySupported has not been called yet
        /// </summary>
        public static void syncManipulatorValues()
        {
            if ((!lav.HasValue) || lav == false) return;
            lock (slockll)
            {
                try
                {
                    if (!object.ReferenceEquals(ll, null))
                    {
                        ll.syncManipulatorValues();
                    }
                }
                catch (ThreadAbortException ex) { throw ex; }
                catch (Exception) {
                    lav = false;
                }
            }
        }

        /// <summary>
        /// Loads an assembly for extension registration
        /// Also legacy loads an assembly if the legacy system is supported
        /// For non-legacy, only public types are loaded
        /// </summary>
        /// <param name="assemblyIn">The assembly to load</param>
        /// <returns>Whether all type loading succeeded</returns>
        public static bool loadAssembly(Assembly assemblyIn)
        {
            if (object.ReferenceEquals(assemblyIn, null)) return false;
            var toret = true;
            foreach (Type c in assemblyIn.GetTypes())
            {
                if (c.IsPublic & !c.IsAbstract & !c.IsInterface & !c.IsEnum)
                {
                    toret &= typeProcessor(c);
                }
            }
            if (lav.HasValue && lav == true)
            {
                lock (slockll)
                {
                    try
                    {
                        if (!object.ReferenceEquals(ll, null))
                        {
                            toret &= ll.loadAssembly(assemblyIn);
                        }
                    }
                    catch (ThreadAbortException ex) { throw ex; }
                    catch (Exception)
                    {
                        lav = false;
                    }
                }
            }
            return toret;
        }

        private static bool typeProcessor(Type typeIn)
        {
            MethodInfo initMethod = null;
            bool isInstanceMethod = false;
            try
            {
                foreach (MethodInfo c in typeIn.GetMethods(BindingFlags.Public | BindingFlags.Static))
                {
                    ExtensionSetupMethodAttribute[] aarr = (ExtensionSetupMethodAttribute[])c.GetCustomAttributes(typeof(ExtensionSetupMethodAttribute), false);
                    if (aarr.Length != 0) { initMethod = c; isInstanceMethod = false; break; }
                }
                if (object.ReferenceEquals(initMethod, null))
                {
                    foreach (MethodInfo c in typeIn.GetMethods(BindingFlags.Public | BindingFlags.Instance))
                    {
                        ExtensionSetupMethodAttribute[] aarr = (ExtensionSetupMethodAttribute[])c.GetCustomAttributes(typeof(ExtensionSetupMethodAttribute), false);
                        if (aarr.Length != 0) { initMethod = c; isInstanceMethod = true; break; }
                    }
                }
            }
            catch (ThreadAbortException ex) { throw ex; }
            catch (Exception) { return false; }
            if (initMethod != null)
            {
                try
                {
                    object instance = null;
                    if (isInstanceMethod) instance = typeIn.Assembly.CreateInstance(typeIn.FullName);
                    initMethod.Invoke(instance, null);
                }
                catch (ThreadAbortException ex) { throw ex; }
                catch (Exception) { return false; }
            }
            return true;
        }
    }
}
