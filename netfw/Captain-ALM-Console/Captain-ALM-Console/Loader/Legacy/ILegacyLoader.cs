using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace captainalm.calmcmd
{
    internal interface ILegacyLoader
    {
        LegacyHookRunnerHolder hookRunnerHolder { get; set; }
        LegacyHookHolder getHookHolder();
        StylableString convertOutputTextToStylableString(object objIn);
        void executeStartHooks();
        void executeEndHooks();
        bool executePreCHooks(string cmdln);
        void executePostCHooks(string cmdln, object objOut);
        void syncManipulatorValues();
        bool loadAssembly(Assembly assemblyIn);
        bool RegisterFirstLegacySyntaxesToStandardLibrary { get; set; }
        bool RegisterFirstLegacyCommandsToStandardLibrary { get; set; }
    }
}
