using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace captainalm.calmcmd
{
    /// <summary>
    /// Provides an Interface for accessing a HookInfo struct.
    /// No support for winforms on this interface.
    /// </summary>
    /// <remarks></remarks>
    public interface IHookInfoWrapper
    {
        /// <summary>
        /// The hook set name.
        /// </summary>
        /// <remarks></remarks>
        String name { get; set; }
        /// <summary>
        /// The hook program start delegate.
        /// </summary>
        /// <remarks></remarks>
        void hook_programstart();
        /// <summary>
        /// The hook program stop delegate.
        /// </summary>
        /// <remarks></remarks>
        void hook_programstop();
        /// <summary>
        /// The hook command pre-execute delegate.
        /// </summary>
        /// <param name="commandstr">The command string being executed.</param>
        /// <remarks></remarks>
        void hook_command_preexecute(string commandstr);
        /// <summary>
        /// The hook command post-execute delegate.
        /// </summary>
        /// <param name="commandstr">The command string being executed.</param>
        /// <param name="returnval">The return output text from the command.</param>
        /// <remarks></remarks>
        void hook_command_postexecute(string commandstr, StylableString returnval);
        /// <summary>
        /// The hook get runcommand delegate.
        /// </summary>
        /// <remarks></remarks>
        void hook_runcommand();
        /// <summary>
        /// The hook get writeoutput delegate.
        /// </summary>
        /// <remarks></remarks>
        void hook_writeoutput();
        /// <summary>
        /// The hook get readoutput delegate.
        /// </summary>
        /// <remarks></remarks>
        void hook_readoutput();
        /// <summary>
        /// The hook read key delegate.
        /// </summary>
        /// <param name="key">The key read in</param>
        /// <remarks></remarks>
        void hook_rk(string key);
        /// <summary>
        /// The hook get add external command delegate.
        /// </summary>
        /// <remarks></remarks>
        void hook_e_cmd_add();
        /// <summary>
        /// The hook get list external commands delegate.
        /// </summary>
        /// <remarks></remarks>
        void hook_e_cmd_list();
        /// <summary>
        /// The hook get remove external command delegate.
        /// </summary>
        /// <remarks></remarks>
        void hook_e_cmd_remove();
        /// <summary>
        /// The hook get add external syntax delegate.
        /// </summary>
        /// <remarks></remarks>
        void hook_e_snx_add();
        /// <summary>
        /// The hook get list external syntaxes delegate.
        /// </summary>
        /// <remarks></remarks>
        void hook_e_snx_list();
        /// <summary>
        /// The hook get remove external syntax delegate.
        /// </summary>
        /// <remarks></remarks>
        void hook_e_snx_remove();
    }
}
