using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace captainalm.calmcmd
{
    internal sealed class HookInfoWrapper : IHookInfoWrapper
    {
        public captainalm.calmcon.api.HookInfo hookinfo;
        //Define the legacy hooks
        private captainalm.calmcon.api.Types.RunCommandHook runcommandhook;
        private captainalm.calmcon.api.Types.AddExternalCommandHook addecmdhook;
        private captainalm.calmcon.api.Types.AddExternalSyntaxHook addesnxhook;
        private captainalm.calmcon.api.Types.RemoveExternalCommandHook remecmdhook;
        private captainalm.calmcon.api.Types.RemoveExternalSyntaxHook remesnxhook;
        private captainalm.calmcon.api.Types.ListExternalCommandsHook lsecmdhook;
        private captainalm.calmcon.api.Types.ListExternalSyntaxesHook lsesnxhook;
        private captainalm.calmcon.api.Types.ReadOutputHook rohook;
        private captainalm.calmcon.api.Types.WriteOutputHook wohook;
        // TODO: Just access these from a passed legacy loader (Then refs can be global)
        public HookInfoWrapper(captainalm.calmcon.api.HookInfo hookinfo, captainalm.calmcon.api.Types.RunCommandHook runcommandhook,
            captainalm.calmcon.api.Types.AddExternalCommandHook addecmdhook, captainalm.calmcon.api.Types.AddExternalSyntaxHook addesnxhook,
            captainalm.calmcon.api.Types.RemoveExternalCommandHook remecmdhook, captainalm.calmcon.api.Types.RemoveExternalSyntaxHook remesnxhook,
            captainalm.calmcon.api.Types.ListExternalCommandsHook lsecmdhook, captainalm.calmcon.api.Types.ListExternalSyntaxesHook lsesnxhook,
            captainalm.calmcon.api.Types.ReadOutputHook rohook, captainalm.calmcon.api.Types.WriteOutputHook wohook)
        {
            this.hookinfo = hookinfo;
            this.runcommandhook = runcommandhook;
            this.addecmdhook = addecmdhook;
            this.addesnxhook = addesnxhook;
            this.remecmdhook = remecmdhook;
            this.remesnxhook = remesnxhook;
            this.lsecmdhook = lsecmdhook;
            this.lsesnxhook = lsesnxhook;
            this.rohook = rohook;
            this.wohook = wohook;
        }

        public string name
        {
            get
            {
                return hookinfo.name;
            }
            set
            {
                hookinfo.name = value;
            }
        }

        public void hook_programstart()
        {
            hookinfo.hook_programstart();
        }

        public void hook_programstop()
        {
            hookinfo.hook_programstop();
        }

        public void hook_command_preexecute(string commandstr)
        {
            hookinfo.hook_command_preexecute(commandstr);
        }

        public void hook_command_postexecute(string commandstr, StylableString returnval)
        {
            hookinfo.hook_command_postexecute(commandstr, (captainalm.calmcon.api.OutputText)LegacyLoader.ConvertStylableStringToOutputText(returnval));
        }

        public void hook_runcommand()
        {
            hookinfo.hook_runcommand(ref this.runcommandhook);
        }

        public void hook_writeoutput()
        {
            hookinfo.hook_writeoutput(ref this.wohook);
        }

        public void hook_readoutput()
        {
            hookinfo.hook_readoutput(ref this.rohook);
        }

        public void hook_rk(string key)
        {
            hookinfo.hook_rk(key);
        }

        public void hook_e_cmd_add()
        {
            hookinfo.hook_e_cmd_add(ref this.addecmdhook);
        }

        public void hook_e_cmd_list()
        {
            hookinfo.hook_e_cmd_list(ref this.lsecmdhook);
        }

        public void hook_e_cmd_remove()
        {
            hookinfo.hook_e_cmd_remove(ref this.remecmdhook);
        }

        public void hook_e_snx_add()
        {
            hookinfo.hook_e_snx_add(ref this.addesnxhook);
        }

        public void hook_e_snx_list()
        {
            hookinfo.hook_e_snx_list(ref this.lsesnxhook);
        }

        public void hook_e_snx_remove()
        {
            hookinfo.hook_e_snx_remove(ref this.remesnxhook);
        }
    }
}
