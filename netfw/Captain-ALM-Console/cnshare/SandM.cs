using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;

namespace captainalm.calmcon.api
{
    /// <summary>
    /// This provides API switching for the program.
    /// </summary>
    /// <remarks></remarks>
    [StandardModule]
    public static class Switches
    {
        /// <summary>
        /// Enable, Disable or Program Control the Multiline Entry Checkbox.
        /// </summary>
        /// <remarks></remarks>
        public static bool? MultilineCheckboxEnablement = null;
        /// <summary>
        /// Read keys from the output boxes.
        /// </summary>
        /// <remarks></remarks>
        public static bool OutputBoxReadKey = false;
        /// <summary>
        /// Enable Or Disable Command Execution.
        /// </summary>
        /// <remarks></remarks>
        public static bool CommandExecution = true;
        /// <summary>
        /// Allow multipule Lines to be Entered into the command box.
        /// </summary>
        /// <remarks></remarks>
        public static bool AllowMultiCommandEntry = false;
    }
    /// <summary>
    /// This provides API manipulation for the program.
    /// </summary>
    /// <remarks></remarks>
    [StandardModule]
    public static class Manipulator
    {
        /// <summary>
        /// This provides the command stack for access and modification.
        /// </summary>
        /// <remarks></remarks>
        public static Stack<string> CommandStack = new Stack<string>();
        /// <summary>
        /// This provides the variable dictionary for access and modification.
        /// </summary>
        /// <remarks></remarks>
        public static Dictionary<string, string> VariableDictionary = new Dictionary<string, string>();
        /// <summary>
        /// Provides the syntax mode for access and manipulation, be careful!
        /// </summary>
        /// <remarks></remarks>
        public static string SyntaxMode = "";
    }

}
