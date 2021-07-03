using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;

namespace captainalm.calmcon.api
{
    [StandardModule]
    public static class Types
    {
        /// <summary>
        /// Any commands set in libraries need to use this as the delegate type.
        /// </summary>
        /// <param name="arr">The array of parameters for the command.</param>
        /// <returns>Return value of the command.</returns>
        /// <remarks></remarks>
        public delegate OutputText Cmd(string[] arr);
        /// <summary>
        /// This is the hook delegate used for the start and stop program hook events.
        /// </summary>
        /// <remarks></remarks>
        public delegate void ProgramEventHook();
        /// <summary>
        /// This is the hook used for the preexecute command hook event.
        /// </summary>
        /// <param name="commandstr">The command string being executed.</param>
        /// <remarks></remarks>
        public delegate void PreCommandExecuteHook(string commandstr);
        /// <summary>
        /// This is the hook used for the postexecute command hook event.
        /// </summary>
        /// <param name="commandstr">The command string being executed.</param>
        /// <param name="returnval">The return output text from the command.</param>
        /// <remarks></remarks>
        public delegate void PostCommandExecuteHook(string commandstr, OutputText returnval);
        /// <summary>
        /// This hook is used to access and modify the main form.
        /// </summary>
        /// <param name="form">The passed main form.</param>
        /// <remarks></remarks>
        public delegate void FormHook(ref System.Windows.Forms.Form form);
        /// <summary>
        /// This hook is used to access the output box on the main form.
        /// </summary>
        /// <param name="txtbx">The output textbox on the form.</param>
        /// <remarks></remarks>
        public delegate void OutputTextBoxHook(ref System.Windows.Forms.RichTextBox txtbx);
        /// <summary>
        /// This hook is used to access the command box on the main form.
        /// </summary>
        /// <param name="txtbx">The command textbox on the form.</param>
        /// <remarks></remarks>
        public delegate void CommandTextBoxHook(ref System.Windows.Forms.TextBox txtbx);
        /// <summary>
        /// This hook is used to run a command,
        /// and can be invoked by the library once an instance is retrieved via the GetRunCommandHook.
        /// </summary>
        /// <param name="command">The command to pass.</param>
        /// <param name="args">The arguments to pass.</param>
        /// <returns>The return value of the command.</returns>
        /// <remarks></remarks>
        public delegate OutputText RunCommandHook(string command, string[] args);
        /// <summary>
        /// Gets the hook instance for RunCommandHook so it can be invoked by the library.
        /// </summary>
        /// <param name="hook">The hook instance.</param>
        /// <remarks></remarks>
        public delegate void GetRunCommandHook(ref RunCommandHook hook);
        /// <summary>
        /// This hook is used to write to the output textbox,
        /// and can be invoked by the library once an instance is retrieved via the GetWriteOutputHook.
        /// </summary>
        /// <param name="text">The Output Text to write to the output textbox.</param>
        /// <remarks></remarks>
        public delegate void WriteOutputHook(OutputText text);
        /// <summary>
        /// Gets the hook instance for WriteOutputHook so it can be invoked by the library.
        /// </summary>
        /// <param name="hook">The hook instance.</param>
        /// <remarks></remarks>
        public delegate void GetWriteOutputHook(ref WriteOutputHook hook);
        /// <summary>
        /// This hook is used to read from the output textbox,
        /// and can be invoked by the library once an instance is retrieved via the GetReadOutputHook.
        /// </summary>
        /// <returns>The contents of the output textbox.</returns>
        /// <remarks></remarks>
        public delegate string ReadOutputHook();
        /// <summary>
        /// Gets the hook instance for ReadOutputHook so it can be invoked by the library.
        /// </summary>
        /// <param name="hook">The hook instance.</param>
        /// <remarks></remarks>
        public delegate void GetReadOutputHook(ref ReadOutputHook hook);
        /// <summary>
        /// This hook is used to listen for a key being pressed in the Operation Log Text Box event.
        /// </summary>
        /// <param name="key">The key data caught.</param>
        /// <remarks></remarks>
        public delegate void ReadKeyHook(string key);
        /// <summary>
        /// This hook is used to add commands during program execution.
        /// and can be invoked by the library once an instance is retrieved via the GetAddExternalCommandHook.
        /// </summary>
        /// <param name="libname">The library name.</param>
        /// <param name="command">The command to add.</param>
        /// <returns>The id of the added command or 0 for faliure.</returns>
        /// <remarks></remarks>
        public delegate int AddExternalCommandHook(string libname, Command command);
        /// <summary>
        /// Gets the hook instance for AddExternalCommandHook so it can be invoked by the library.
        /// </summary>
        /// <param name="hook">The hook instance.</param>
        /// <remarks></remarks>
        public delegate void GetAddExternalCommandHook(ref AddExternalCommandHook hook);
        /// <summary>
        /// This hook is used to remove added commands during program execution.
        /// and can be invoked by the library once an instance is retrieved via the GetRemoveExternalCommandHook.
        /// </summary>
        /// <param name="libname">The library name.</param>
        /// <param name="id">The ID of the command to remove.</param>
        /// <returns>If the removal succeded.</returns>
        /// <remarks></remarks>
        public delegate bool RemoveExternalCommandHook(string libname, int id);
        /// <summary>
        /// Gets the hook instance for RemoveExternalCommandHook so it can be invoked by the library.
        /// </summary>
        /// <param name="hook">The hook instance.</param>
        /// <remarks></remarks>
        public delegate void GetRemoveExternalCommandHook(ref RemoveExternalCommandHook hook);
        /// <summary>
        /// This hook is used to list added commands during program execution.
        /// and can be invoked by the library once an instance is retrieved via the GetListExternalCommandsHook.
        /// </summary>
        /// <param name="libname">The library name.</param>
        /// <returns>This list of the commands added by this library name in the form "id : name"</returns>
        /// <remarks></remarks>
        public delegate string[] ListExternalCommandsHook(string libname);
        /// <summary>
        /// Gets the hook instance for ListExternalCommandsHook so it can be invoked by the library.
        /// </summary>
        /// <param name="hook">The hook instance.</param>
        /// <remarks></remarks>
        public delegate void GetListExternalCommandsHook(ref ListExternalCommandsHook hook);
        /// <summary>
        /// This hook is used to add syntaxes during program execution.
        /// and can be invoked by the library once an instance is retrieved via the GetAddExternalSyntaxHook.
        /// </summary>
        /// <param name="libname">The library name.</param>
        /// <param name="syntax">The syntax to add.</param>
        /// <returns>The id of the added syntax or 0 for faliure.</returns>
        /// <remarks></remarks>
        public delegate int AddExternalSyntaxHook(string libname, ISyntax syntax);
        /// <summary>
        /// Gets the hook instance for AddExternalSyntaxHook so it can be invoked by the library.
        /// </summary>
        /// <param name="hook">The hook instance.</param>
        /// <remarks></remarks>
        public delegate void GetAddExternalSyntaxHook(ref AddExternalSyntaxHook hook);
        /// <summary>
        /// This hook is used to remove added syntaxes during program execution.
        /// and can be invoked by the library once an instance is retrieved via the GetRemoveExternalSyntaxHook.
        /// </summary>
        /// <param name="libname">The library name.</param>
        /// <param name="id">The ID of the syntax to remove.</param>
        /// <returns>If the removal succeded.</returns>
        /// <remarks></remarks>
        public delegate bool RemoveExternalSyntaxHook(string libname, int id);
        /// <summary>
        /// Gets the hook instance for RemoveExternalSyntaxHook so it can be invoked by the library.
        /// </summary>
        /// <param name="hook">The hook instance.</param>
        /// <remarks></remarks>
        public delegate void GetRemoveExternalSyntaxHook(ref RemoveExternalSyntaxHook hook);
        /// <summary>
        /// This hook is used to list added syntaxes during program execution.
        /// and can be invoked by the library once an instance is retrieved via the GetListExternalSyntaxesHook.
        /// </summary>
        /// <param name="libname">The library name.</param>
        /// <returns>This list of the syntaxes added by this library name in the form "id : name"</returns>
        /// <remarks></remarks>
        public delegate string[] ListExternalSyntaxesHook(string libname);
        /// <summary>
        /// Gets the hook instance for ListExternalSyntaxesHook so it can be invoked by the library.
        /// </summary>
        /// <param name="hook">The hook instance.</param>
        /// <remarks></remarks>
        public delegate void GetListExternalSyntaxesHook(ref ListExternalSyntaxesHook hook);
    }

    /// <summary>
    /// API interface for adding other syntax language interpreters.
    /// </summary>
    /// <remarks></remarks>
    public interface ISyntax 
    {
        /// <summary>
        /// Returns the Syntax Name.
        /// </summary>
        /// <returns>Returns the Syntax Name.</returns>
        /// <remarks></remarks>
        string name();
        /// <summary>
        /// Defines the required command decryptor, the returned list has the command as the first item and the arguments as the other items.
        /// </summary>
        /// <param name="strcmd">This is where the command string is passed.</param>
        /// <param name="commands">This is where the list of commands on the console is passed.</param>
        /// <returns>The returned list has the command as the first item and the arguments as the other items.</returns>
        /// <remarks></remarks>
        List<string> decrypt(string strcmd, List<string> commands);
    }

    /// <summary>
    /// Use this structure to register commands from a library.
    /// </summary>
    /// <remarks></remarks>
    public struct Command
    {
        /// <summary>
        /// The held command delegate.
        /// </summary>
        /// <remarks></remarks>
        public Types.Cmd command;
        /// <summary>
        /// The name of the command.
        /// </summary>
        /// <remarks></remarks>
        public string name;
        /// <summary>
        /// The help string for the command.
        /// </summary>
        /// <remarks></remarks>
        public string help;
        /// <summary>
        /// Constructs a new command.
        /// </summary>
        /// <param name="_name">The name of the command.</param>
        /// <param name="del">The delegate for the command using <seealso cref="captainalm.calmcon.api.types.Cmd">the Cmd delegate type</seealso>.</param>
        /// <param name="_help">The help string for the command.</param>
        /// <remarks></remarks>
        public Command(string _name, Types.Cmd del, string _help = "")
        {
            name = _name;
            command = del;
            help = _help;
        }
    }

    /// <summary>
    /// Pass this structure from a function with a <see cref="SetupMethodAttribute">SetupMethodAttribute</see>, you need one of these functions per library definition.
    /// </summary>
    /// <remarks></remarks>
    public struct LibrarySetup
    {
        /// <summary>
        /// Returns the name of the library.
        /// </summary>
        /// <remarks></remarks>
        public string name;
        /// <summary>
        /// Returns the version number of the library.
        /// </summary>
        /// <remarks></remarks>
        public int version;
        /// <summary>
        /// Returns the hook set information of the library.
        /// </summary>
        /// <remarks></remarks>
        public HookInfo hook_info;
        /// <summary>
        /// Returns the commands of the library.
        /// </summary>
        /// <remarks></remarks>
        public Command[] commands;
        /// <summary>
        /// Retruns the syntaxes of the library.
        /// </summary>
        /// <remarks></remarks>
        public ISyntax[] syntaxes;
        /// <summary>
        /// Constructs a new LibrarySetup Structure.
        /// </summary>
        /// <param name="_name">The library name.</param>
        /// <param name="ver">The library Version.</param>
        /// <param name="_hookinfo">The library Hook Information.</param>
        /// <param name="_commands">The library Commands Array.</param>
        /// <param name="_syntaxes">The library Syntax Array.</param>
        /// <remarks></remarks>
        public LibrarySetup(string _name, int ver, HookInfo _hookinfo, Command[] _commands = null, ISyntax[] _syntaxes = null)
        {
            name = _name;
            version = ver;
            hook_info = _hookinfo;
            commands = _commands;
            syntaxes = _syntaxes;
        }
    }

    /// <summary>
    /// Use this structure to register hooks against a delegate, use nothing to specify no hook.
    /// </summary>
    /// <remarks></remarks>
    public struct HookInfo 
    {
        /// <summary>
        /// The hook set name.
        /// </summary>
        /// <remarks></remarks>
        public string name;
        /// <summary>
        /// The hook program start delegate.
        /// </summary>
        /// <remarks></remarks>
        public Types.ProgramEventHook hook_programstart;
        /// <summary>
        /// The hook program stop delegate.
        /// </summary>
        /// <remarks></remarks>
        public Types.ProgramEventHook hook_programstop;
        /// <summary>
        /// The hook command pre-execute delegate.
        /// </summary>
        /// <remarks></remarks>
        public Types.PreCommandExecuteHook hook_command_preexecute;
        /// <summary>
        /// The hook command post-execute delegate.
        /// </summary>
        /// <remarks></remarks>
        public Types.PostCommandExecuteHook hook_command_postexecute;
        /// <summary>
        /// The hook form delegate.
        /// </summary>
        /// <remarks></remarks>
        public Types.FormHook hook_form;
        /// <summary>
        /// The hook out textbox delegate.
        /// </summary>
        /// <remarks></remarks>
        public Types.OutputTextBoxHook hook_out_txtbx;
        /// <summary>
        /// The hook get runcommand delegate.
        /// </summary>
        /// <remarks></remarks>
        public Types.GetRunCommandHook hook_runcommand;
        /// <summary>
        /// The hook get writeoutput delegate.
        /// </summary>
        /// <remarks></remarks>
        public Types.GetWriteOutputHook hook_writeoutput;
        /// <summary>
        /// The hook get readoutput delegate.
        /// </summary>
        /// <remarks></remarks>
        public Types.GetReadOutputHook hook_readoutput;
        /// <summary>
        /// The hook read key delegate.
        /// </summary>
        /// <remarks></remarks>
        public Types.ReadKeyHook hook_rk;
        /// <summary>
        /// The hook out textbox delegate.
        /// </summary>
        /// <remarks></remarks>
        public Types.CommandTextBoxHook hook_cmd_txtbx;
        /// <summary>
        /// The hook get add external command delegate.
        /// </summary>
        /// <remarks></remarks>
        public Types.GetAddExternalCommandHook hook_e_cmd_add;
        /// <summary>
        /// The hook get list external commands delegate.
        /// </summary>
        /// <remarks></remarks>
        public Types.GetListExternalCommandsHook hook_e_cmd_list;
        /// <summary>
        /// The hook get remove external command delegate.
        /// </summary>
        /// <remarks></remarks>
        public Types.GetRemoveExternalCommandHook hook_e_cmd_remove;
        /// <summary>
        /// The hook get add external syntax delegate.
        /// </summary>
        /// <remarks></remarks>
        public Types.GetAddExternalSyntaxHook hook_e_snx_add;
        /// <summary>
        /// The hook get list external syntaxes delegate.
        /// </summary>
        /// <remarks></remarks>
        public Types.GetListExternalSyntaxesHook hook_e_snx_list;
        /// <summary>
        /// The hook get remove external syntax delegate.
        /// </summary>
        /// <remarks></remarks>
        public Types.GetRemoveExternalSyntaxHook hook_e_snx_remove;

        /// <summary>
        /// Constructs a new set of hook info, use nothing as a parameter if you do not want to register a certain hook.
        /// </summary>
        /// <param name="hook_set_name">The name of the hook set.</param>
        /// <param name="hcompreex">The Pre-Command Execute hook delegate.</param>
        /// <param name="hcompstex">The Post-Command Execute hook delegate.</param>
        /// <param name="hprostr">The Program Start hook delegate.</param>
        /// <param name="hprostp">The Program stop hook delegate.</param>
        /// <param name="hform">The form hook delegate.</param>
        /// <param name="houtbx">The output textbox hook delegate.</param>
        /// <param name="getrcom">The Get RunCommand Hook delegate.</param>
        /// <param name="getwout">The Get WriteOutput Hook delegate.</param>
        /// <param name="getrout">The Get ReadOutput Hook delegate.</param>
        /// <param name="rk">The Read Key (In the operation log text box.) Hook Delegate.</param>
        /// <param name="hcmdbx">The command textbox hook delegate.</param>
        /// <param name="hecmdadd">The Get Add External Command During Runtime hook Delegate.</param>
        /// <param name="hecmdlist">The Get List External Commands During Runtime hook Delegate.</param>
        /// <param name="hecmdremove">The Get Remove External Command During Runtime hook Delegate.</param>
        /// <param name="hesnxadd">The Get Add External Syntax During Runtime hook Delegate.</param>
        /// <param name="hesnxlist">The Get List External Syntaxes During Runtime hook Delegate.</param>
        /// <param name="hesnxremove">The Get Remove External Syntax During Runtime hook Delegate.</param>
        /// <remarks></remarks>
        public HookInfo(string hook_set_name, Types.ProgramEventHook hprostr = null, Types.ProgramEventHook hprostp = null, Types.PreCommandExecuteHook hcompreex = null, Types.PostCommandExecuteHook hcompstex = null, Types.FormHook hform = null, Types.OutputTextBoxHook houtbx = null, Types.GetRunCommandHook getrcom = null, Types.GetWriteOutputHook getwout = null, Types.GetReadOutputHook getrout = null, Types.ReadKeyHook rk = null, Types.CommandTextBoxHook hcmdbx = null, Types.GetAddExternalCommandHook hecmdadd = null, Types.GetListExternalCommandsHook hecmdlist = null, Types.GetRemoveExternalCommandHook hecmdremove = null, Types.GetAddExternalSyntaxHook hesnxadd = null, Types.GetListExternalSyntaxesHook hesnxlist = null, Types.GetRemoveExternalSyntaxHook hesnxremove = null)
        {
            name = hook_set_name;
            hook_programstart = hprostr;
            hook_programstop = hprostp;
            hook_command_preexecute = hcompreex;
            hook_command_postexecute = hcompstex;
            hook_form = hform;
            hook_out_txtbx = houtbx;
            hook_runcommand = getrcom;
            hook_writeoutput = getwout;
            hook_readoutput = getrout;
            hook_rk = rk;
            hook_cmd_txtbx = hcmdbx;
            hook_e_cmd_add = hecmdadd;
            hook_e_cmd_list = hecmdlist;
            hook_e_cmd_remove = hecmdremove;
            hook_e_snx_add = hesnxadd;
            hook_e_snx_list = hesnxlist;
            hook_e_snx_remove = hesnxremove;
        }
    }
}