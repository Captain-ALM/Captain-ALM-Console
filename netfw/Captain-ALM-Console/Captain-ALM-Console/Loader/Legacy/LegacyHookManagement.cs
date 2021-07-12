using System;
using System.Collections.Generic;

namespace captainalm.calmcmd
{
    /// <summary>
    /// Holds the hook database of the legacy loader for 'safer' external use.
    /// </summary>
    public sealed class LegacyHookHolder : ICloneable
    {
        /// <summary>
        /// The hook information dictionary
        /// </summary>
        public readonly Dictionary<String, captainalm.calmcon.api.HookInfo> hookInformation;
        internal string libraryNameFFA = null;

        internal LegacyHookHolder(IDictionary<string, captainalm.calmcon.api.HookInfo> dictIn) { hookInformation = new Dictionary<string, calmcon.api.HookInfo>(dictIn); }
        /// <summary>
        /// Creates a new instance of LegacyHookHolder
        /// </summary>
        public LegacyHookHolder() : this(new Dictionary<string, captainalm.calmcon.api.HookInfo>()) { }

        /// <summary>
        /// Returns the hookInfo of the libraryName that this holder was set to target
        /// </summary>
        /// <returns>The targeted hookInfo</returns>
        public captainalm.calmcon.api.HookInfo fastAccessHookInfo()
        {
            if (object.ReferenceEquals(libraryNameFFA, null)) return default(captainalm.calmcon.api.HookInfo);
            if (hookInformation.ContainsKey(libraryNameFFA)) return hookInformation[libraryNameFFA];
            return default(captainalm.calmcon.api.HookInfo);
        }
        /// <summary>
        /// Gets the targeted libraryName, if any, of the holder
        /// </summary>
        public string fastAccessName
        {
            get
            {
                return libraryNameFFA;
            }
        }
        /// <summary>
        /// Creates a clone of the object
        /// This does not clone the libraryNameFFA field
        /// </summary>
        /// <returns>A copy of the object</returns>
        public object Clone()
        {
            return new LegacyHookHolder(this.hookInformation);
        }
    }

    /// <summary>
    /// Provides a delegate that allows for a subroutine to be called with a hookHolder
    /// The fastAccessHookInfo function of the hookHolder should be called to access the required HookInformation
    /// The hook then called should be based on what the subroutine was designed to execute
    /// </summary>
    /// <param name="hookHolder">The hookHolder instance passed</param>
    public delegate void LegacyHookRunner(LegacyHookHolder hookHolder);

    /// <summary>
    /// Holds the executors that would manage the legacy hook of the specified field
    /// </summary>
    public struct LegacyHookRunnerHolder
    {
        /// <summary>
        /// Provides the holder for executing the form hook at the correct time
        /// </summary>
        public LegacyHookRunner FormHook;
        /// <summary>
        /// Provides the holder for executing the output textbox hook at the correct time
        /// </summary>
        public LegacyHookRunner OutputTextboxHook;
        /// <summary>
        /// Provides the holder for executing the command textbox hook at the correct time
        /// </summary>
        public LegacyHookRunner CommandTextboxHook;
        /// <summary>
        /// Provides the holder for executing the get write output hook at the correct time
        /// </summary>
        public LegacyHookRunner GetWriteOutputHook;
        /// <summary>
        /// Provides the holder for executing the get read output hook at the correct time
        /// </summary>
        public LegacyHookRunner GetReadOutputHook;
    }
}
