namespace captainalm.calmcmd
{
    /// <summary>
    /// This hook is used to run a command,
    /// and can be invoked by the library once an instance is retrieved via the GetRunCommandHook.
    /// </summary>
    /// <param name="command">The command to pass.</param>
    /// <param name="args">The arguments to pass.</param>
    /// <returns>The return value of the command.</returns>
    /// <remarks></remarks>
    public delegate StylableString RunCommandHook(string command, string[] args);
    /// <summary>
    /// This hook is used to write to the output textbox,
    /// and can be invoked by the library once an instance is retrieved via the GetWriteOutputHook.
    /// </summary>
    /// <param name="text">The Output Text to write to the output textbox.</param>
    /// <remarks></remarks>
    public delegate void WriteOutputHook(StylableString text);
    /// <summary>
    /// This hook is used to read from the output textbox,
    /// and can be invoked by the library once an instance is retrieved via the GetReadOutputHook.
    /// </summary>
    /// <returns>The contents of the output textbox.</returns>
    /// <remarks></remarks>
    public delegate string ReadOutputHook();
    /// <summary>
    /// This hook is used to add commands during program execution.
    /// and can be invoked by the library once an instance is retrieved via the GetAddExternalCommandHook.
    /// </summary>
    /// <param name="libname">The library name.</param>
    /// <param name="command">The command to add.</param>
    /// <returns>The id of the added command or 0 for faliure.</returns>
    /// <remarks></remarks>
    public delegate int AddExternalCommandHook(string libname, ICommand command);
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
    /// This hook is used to list added commands during program execution.
    /// and can be invoked by the library once an instance is retrieved via the GetListExternalCommandsHook.
    /// </summary>
    /// <param name="libname">The library name.</param>
    /// <returns>This list of the commands added by this library name in the form "id : name"</returns>
    /// <remarks></remarks>
    public delegate string[] ListExternalCommandsHook(string libname);
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
    /// This hook is used to remove added syntaxes during program execution.
    /// and can be invoked by the library once an instance is retrieved via the GetRemoveExternalSyntaxHook.
    /// </summary>
    /// <param name="libname">The library name.</param>
    /// <param name="id">The ID of the syntax to remove.</param>
    /// <returns>If the removal succeded.</returns>
    /// <remarks></remarks>
    public delegate bool RemoveExternalSyntaxHook(string libname, int id);
    /// <summary>
    /// This hook is used to list added syntaxes during program execution.
    /// and can be invoked by the library once an instance is retrieved via the GetListExternalSyntaxesHook.
    /// </summary>
    /// <param name="libname">The library name.</param>
    /// <returns>This list of the syntaxes added by this library name in the form "id : name"</returns>
    /// <remarks></remarks>
    public delegate string[] ListExternalSyntaxesHook(string libname);
}