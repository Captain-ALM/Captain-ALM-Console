using System;
using System.Collections.Generic;

namespace captainalm.calmcmd
{
    /// <summary>
    /// Provides the legacy default loaded syntax.
    /// </summary>
    public class LegacyDefaultSyntax : ISyntax
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
            if ((dataIn.StartsWith("'") & dataIn.EndsWith("'")) | (dataIn.StartsWith("\"") & dataIn.EndsWith("\""))) return false;
            var argsLst = new List<string>();
            var carg = "";
            var inarg = false;
            var inescape = false;

            for (int i = 0; i < dataIn.Length; i++)
            {
                var cstr = dataIn.Substring(i, 1);
                if (inescape)
                {
                    carg += cstr;
                    inescape = false;
                }
                else
                {
                    if (cstr.Equals(","))
                    {
                        if (inarg)
                        {
                            argsLst.Add(this.literalconvert(carg, (commandOut.Equals("str") || commandOut.Equals("string") || commandOut.Equals("int") || commandOut.Equals("integer") || commandOut.Equals("dec") || commandOut.Equals("decimal"))));
                        }
                        else
                        {
                            commandOut = carg.ToString();
                            inarg = true;
                        }
                        carg = "";
                    }
                    else if (cstr.Equals("/"))
                    {
                        inescape = true;
                    }
                    else
                    {
                        carg += cstr;
                    }
                }
            }
            if (!carg.Equals(""))
            {
                if (inarg)
                {
                    argsLst.Add(this.literalconvert(carg, (commandOut.Equals("str") || commandOut.Equals("string") || commandOut.Equals("int") || commandOut.Equals("integer") || commandOut.Equals("dec") || commandOut.Equals("decimal"))));
                }
                else
                {
                    commandOut = carg.ToString();
                }
            }
            argumentsOut = argsLst.ToArray();
            return true;
        }

        private string literalconvert(string toconv, bool isconv)
        {
            var isint = false;
            var isdec = false;
            var isstring = false;

            if ((toconv.StartsWith("'") & toconv.EndsWith("'")) | (toconv.StartsWith("\"") & toconv.EndsWith("\"")))
            {
                if (isconv)
                {
                    return toconv;
                }
                else
                {
                    isstring = true;
                    toconv = toconv.Substring(1, toconv.Length - 2);
                }
            }

            try
            {
                int tmp = 0;
                isint = int.TryParse(toconv, out tmp);
            }
            catch (System.Threading.ThreadAbortException)
            {
                System.Threading.Thread.CurrentThread.Abort();
            }
            catch (Exception)
            {
                isint = false;
            }

            if (!isint)
            {
                try
                {
                    decimal tmp = 0;
                    isdec = decimal.TryParse(toconv, out tmp);
                }
                catch (System.Threading.ThreadAbortException)
                {
                    System.Threading.Thread.CurrentThread.Abort();
                }
                catch (Exception)
                {
                    isdec = false;
                }
                isstring = !isdec;
            }

            if (isint)
            {
                return "int,'" + toconv + "'";
            }
            else if (isdec)
            {
                return "dec,'" + toconv + "'";
            }
            else if (isstring)
            {
                return "str,'" + toconv.Replace("/", "//").Replace(",", "/,") + "'";
            }
            return toconv;
        }

        /// <summary>
        /// Converts a string argument to another type if needed
        /// </summary>
        /// <param name="argumentIn">The string argument in</param>
        /// <returns>The argument out</returns>
        public object argumentTypeConversion(string argumentIn)
        {
            return ((argumentIn.StartsWith("'") & argumentIn.EndsWith("'")) | (argumentIn.StartsWith("\"") & argumentIn.EndsWith("\""))) ? argumentIn.Substring(1, argumentIn.Length - 2) : argumentIn;
        }

        /// <summary>
        /// Returns the name of the class
        /// </summary>
        public string name
        {
            get { return "calm_console_default_type"; }
        }

        /// <summary>
        /// Returns the owner of the class
        /// </summary>
        public string owner
        {
            get { return ""; }
        }
    }
}
