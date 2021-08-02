using System;
using System.Collections.Generic;

namespace captainalm.calmcmd
{
    /// <summary>
    /// Provides the default loaded syntax.
    /// </summary>
    public class DefaultSyntax : ISyntax
    {
        /// <summary>
        /// Decodes a passed command string into a command name string and an array of argument strings
        /// </summary>
        /// <param name="dataIn">The command string in</param>
        /// <param name="commandOut">The command name string out</param>
        /// <param name="argumentsOut">The array of argument strings out</param>
        /// <returns>If the command string was valid (Should still be valid if the command was invalid but the syntax was correct)</returns>
        public bool decode(string dataIn, ref string commandOut, ref string[] argumentsOut)
        {
            if ((dataIn.StartsWith("'") && dataIn.EndsWith("'")) || (dataIn.StartsWith("\"") && dataIn.EndsWith("\"")) || (dataIn.StartsWith("(") && dataIn.EndsWith(")")) || (dataIn.StartsWith("[") && dataIn.EndsWith("]")) || (dataIn.StartsWith("{") && dataIn.EndsWith("}"))) return false;
            if (dataIn.StartsWith("'") || dataIn.StartsWith("\"") || dataIn.StartsWith("(") || dataIn.StartsWith("[") || dataIn.StartsWith("{"))
            {
                commandOut = API.invalidCommandName;
                argumentsOut = new string[0];
                return true;
            }
            var incmd = true;
            var argslst = new List<string>();
            var inescape = false;
            var braklayer = 0;
            var brakexitchar = '\0';
            var instr = false;
            var strexitchar = '\0';
            var cstr = "";
            for (int i = 0; i < dataIn.Length; i++)
            {
                if (incmd)
                {
                    if (dataIn[i] == ',' && !inescape)
                    {
                        commandOut = cstr.ToString();
                        incmd = false;
                        cstr = "";
                    }
                    else if (dataIn[i] == '\\' && !inescape)
                    {
                        inescape = true;
                    }
                    else
                    {
                        cstr += dataIn[i];
                        inescape = false;
                    }
                }
                else
                {
                    if (dataIn[i] == ',' && !inescape && braklayer == 0 && !instr)
                    {
                        argslst.Add(cstr);
                        cstr = "";
                    }
                    else if (dataIn[i] == '(' && (brakexitchar == '\0' || brakexitchar == ')') && !inescape && !instr)
                    {
                        braklayer += 1;
                        brakexitchar = ')';
                    }
                    else if (dataIn[i] == '[' && (brakexitchar == '\0' || brakexitchar == ']') && !inescape && !instr)
                    {
                        braklayer += 1;
                        brakexitchar = ']';
                    }
                    else if (dataIn[i] == '{' && (brakexitchar == '\0' || brakexitchar == '}') && !inescape && !instr)
                    {
                        braklayer += 1;
                        brakexitchar = '}';
                    }
                    else if (dataIn[i] == brakexitchar && !inescape && !instr)
                    {
                        if ((braklayer - 1) >= 0)
                        {
                            braklayer -= 1;
                        }
                        else
                        {
                            commandOut = API.invalidCommandName;
                            argumentsOut = new string[0];
                            return true;
                        }
                        if (braklayer == 0) brakexitchar = '\0';
                    }
                    else if ((dataIn[i] == '\'' || dataIn[i] == '"') && !inescape)
                    {
                        if (instr && strexitchar == dataIn[i])
                        {
                            instr = false;
                            strexitchar = '\0';
                        }
                        else if (!instr)
                        {
                            instr = true;
                            strexitchar = dataIn[i];
                        }
                    }
                    else if (dataIn[i] == '\\' && !inescape)
                    {
                        inescape = true;
                    }
                    else
                    {
                        cstr += dataIn[i];
                        inescape = false;
                    }
                }
            }
            if (inescape || instr || braklayer > 0)
            {
                commandOut = API.invalidCommandName;
                argumentsOut = new string[0];
                return true;
            }
            if (incmd)
            {
                if (isLong(cstr) || isDouble(cstr)) return false;
                if (cstr.Equals(""))
                {
                    commandOut = API.invalidCommandName;
                    argumentsOut = new string[0];
                    return true;
                }
                commandOut = cstr.ToString();
                argumentsOut = new string[0];
                return true;
            }
            else
            {
                if (!cstr.Equals("")) argslst.Add(cstr);
                argumentsOut = argslst.ToArray();
                return true;
            }
        }

        /// <summary>
        /// Converts a string argument to another type if needed
        /// </summary>
        /// <param name="argumentIn">The string argument in</param>
        /// <returns>The argument out</returns>
        public object argumentTypeConversion(string argumentIn)
        {
            if ((argumentIn.StartsWith("'") && argumentIn.EndsWith("'")) || (argumentIn.StartsWith("\"") && argumentIn.EndsWith("\"")))
            {
                return argumentIn.Substring(1, argumentIn.Length - 2);
            }
            else if (argumentIn.StartsWith("[") && argumentIn.EndsWith("]"))
            {
                return getArrayFromArgument(argumentIn.Substring(1, argumentIn.Length - 2)).ToArray();
            }
            else if (argumentIn.StartsWith("{") && argumentIn.EndsWith("}"))
            {
                return getArrayFromArgument(argumentIn.Substring(1, argumentIn.Length - 2));
            }
            else if (isLong(argumentIn))
            {
                return long.Parse(argumentIn);
            }
            else if (isDouble(argumentIn))
            {
                return double.Parse(argumentIn);
            }
            return argumentIn;
        }

        /// <summary>
        /// Returns the name of the class
        /// </summary>
        public string name
        {
            get { return "default_syntax"; }
        }

        /// <summary>
        /// Returns the owner of the class
        /// </summary>
        public string owner
        {
            get { return ""; }
        }

        private static bool isLong(string strIn)
        {
            var val = 0L;
            return long.TryParse(strIn, out val);
        }

        private static bool isDouble(string strIn)
        {
            var val = 0.0;
            return double.TryParse(strIn, out val);
        }

        private List<object> getArrayFromArgument(string dataIn)
        {
            var toret = new List<object>();
            var inescape = false;
            var braklayer = 0;
            var brakexitchar = '\0';
            var instr = false;
            var strexitchar = '\0';
            var cstr = "";
            for (int i = 0; i < dataIn.Length; i++)
            {
                if (dataIn[i] == ',' && !inescape && braklayer == 0 && !instr)
                {
                    toret.Add(argumentTypeConversion(cstr));
                    cstr = "";
                }
                else if (dataIn[i] == '[' && (brakexitchar == '\0' || brakexitchar == ']') && !inescape && !instr)
                {
                    braklayer += 1;
                    brakexitchar = ']';
                }
                else if (dataIn[i] == '{' && (brakexitchar == '\0' || brakexitchar == '}') && !inescape && !instr)
                {
                    braklayer += 1;
                    brakexitchar = '}';
                }
                else if (dataIn[i] == brakexitchar && !inescape && !instr)
                {
                    if ((braklayer - 1) >= 0) braklayer -= 1;
                    if (braklayer == 0) brakexitchar = '\0';
                }
                else if ((dataIn[i] == '\'' || dataIn[i] == '"') && !inescape)
                {
                    if (instr && strexitchar == dataIn[i])
                    {
                        instr = false;
                        strexitchar = '\0';
                    }
                    else if (!instr)
                    {
                        instr = true;
                        strexitchar = dataIn[i];
                    }
                }
                else if (dataIn[i] == '\\' && !inescape)
                {
                    inescape = true;
                }
                else
                {
                    cstr += dataIn[i];
                    inescape = false;
                }
            }
            if (!cstr.Equals("")) toret.Add(argumentTypeConversion(cstr));
            return toret;
        }
    }
}
