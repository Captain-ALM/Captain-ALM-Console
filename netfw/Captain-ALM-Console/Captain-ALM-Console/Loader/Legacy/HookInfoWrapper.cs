using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace captainalm.calmcmd
{
    internal sealed class HookInfoWrapper : IHookInfoWrapper
    {
        public captainalm.calmcon.api.HookInfo hookinfo;
        private LegacyLoader ll;
        public HookInfoWrapper(captainalm.calmcon.api.HookInfo hookinfo, LegacyLoader ll)
        {
            this.hookinfo = hookinfo;
            this.ll = ll;
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
            if (object.ReferenceEquals(hookinfo.hook_programstart, null))
                return;
            hookinfo.hook_programstart.Invoke();
        }

        public void hook_programstop()
        {
            if (object.ReferenceEquals(hookinfo.hook_programstop, null))
                return;
            hookinfo.hook_programstop.Invoke();
        }

        public void hook_command_preexecute(string commandstr)
        {
            if (object.ReferenceEquals(hookinfo.hook_command_preexecute, null))
                return;
            hookinfo.hook_command_preexecute.Invoke(commandstr);
        }

        public void hook_command_postexecute(string commandstr, StylableString returnval)
        {
            if (object.ReferenceEquals(hookinfo.hook_command_postexecute, null))
                return;
            hookinfo.hook_command_postexecute.Invoke(commandstr, (captainalm.calmcon.api.OutputText)LegacyLoader.ConvertStylableStringToOutputText(returnval));
        }

        public void hook_runcommand()
        {
            if (object.ReferenceEquals(hookinfo.hook_runcommand, null) || object.ReferenceEquals(ll.runcommandhook, null))
                return;
            hookinfo.hook_runcommand.Invoke(ref ll.runcommandhook);
        }

        public void hook_writeoutput()
        {
            if (object.ReferenceEquals(hookinfo.hook_writeoutput, null) || object.ReferenceEquals(ll.wohook, null))
                return;
            hookinfo.hook_writeoutput.Invoke(ref ll.wohook);
        }

        public void hook_readoutput()
        {
            if (object.ReferenceEquals(hookinfo.hook_readoutput, null) || object.ReferenceEquals(ll.rohook, null))
                return;
            hookinfo.hook_readoutput.Invoke(ref ll.rohook);
        }

        public void hook_rk(string key)
        {
            if (object.ReferenceEquals(hookinfo.hook_rk, null))
                return;
            hookinfo.hook_rk.Invoke(key);
        }

        public void hook_e_cmd_add()
        {
            if (object.ReferenceEquals(hookinfo.hook_e_cmd_add, null) || object.ReferenceEquals(ll.addecmdhook, null))
                return;
            hookinfo.hook_e_cmd_add.Invoke(ref ll.addecmdhook);
        }

        public void hook_e_cmd_list()
        {
            if (object.ReferenceEquals(hookinfo.hook_e_cmd_list, null) || object.ReferenceEquals(ll.lsecmdhook, null))
                return;
            hookinfo.hook_e_cmd_list.Invoke(ref ll.lsecmdhook);
        }

        public void hook_e_cmd_remove()
        {
            if (object.ReferenceEquals(hookinfo.hook_e_cmd_remove, null) || object.ReferenceEquals(ll.remecmdhook, null))
                return;
            hookinfo.hook_e_cmd_remove.Invoke(ref ll.remecmdhook);
        }

        public void hook_e_snx_add()
        {
            if (object.ReferenceEquals(hookinfo.hook_e_snx_add, null) || object.ReferenceEquals(ll.addesnxhook, null))
                return;
            hookinfo.hook_e_snx_add.Invoke(ref ll.addesnxhook);
        }

        public void hook_e_snx_list()
        {
            if (object.ReferenceEquals(hookinfo.hook_e_snx_list, null) || object.ReferenceEquals(ll.lsesnxhook, null))
                return;
            hookinfo.hook_e_snx_list.Invoke(ref ll.lsesnxhook);
        }

        public void hook_e_snx_remove()
        {
            if (object.ReferenceEquals(hookinfo.hook_e_snx_remove, null) || object.ReferenceEquals(ll.remesnxhook, null))
                return;
            hookinfo.hook_e_snx_remove.Invoke(ref ll.remesnxhook);
        }

        public void hook_form() {}

        public void hook_cmd_txtbx() {}

        public void hook_out_txtbx() {}
    }
}
