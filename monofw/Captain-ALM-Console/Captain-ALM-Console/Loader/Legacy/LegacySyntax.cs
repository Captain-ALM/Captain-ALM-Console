using System;
using System.Collections.Generic;

namespace captainalm.calmcmd
{
    internal sealed class LegacySyntax : ISyntax
    {
        public captainalm.calmcon.api.ISyntax wrappedSyntax;
        private string _owner;

        public LegacySyntax(captainalm.calmcon.api.ISyntax syntIn, string owner)
        {
            wrappedSyntax = syntIn;
            _owner = owner;
        }

        public bool decode(string dataIn, ref string commandOut, ref string[] argumentsOut)
        {
            if ((dataIn.StartsWith("'") & dataIn.EndsWith("'")) | (dataIn.StartsWith("\"") & dataIn.EndsWith("\""))) return false;
            var ret = wrappedSyntax.decrypt(dataIn, new List<String>(API.getCommands()));
            if (object.ReferenceEquals(ret, null) || ret.Count == 0) return false;
            commandOut = ret[0];
            if (object.ReferenceEquals(commandOut, null)) return false;
            if (ret.Count > 1)
            {
                argumentsOut = new string[ret.Count - 1];
                for (int i = 1; i < ret.Count; i++)
                {
                    argumentsOut[i - 1] = ret[i];
                }
            }
            return true;
        }

        public object argumentTypeConversion(string argumentIn)
        {
            return ((argumentIn.StartsWith("'") & argumentIn.EndsWith("'")) | (argumentIn.StartsWith("\"") & argumentIn.EndsWith("\""))) ? argumentIn.Substring(1, argumentIn.Length - 2) : argumentIn;
        }

        public string name
        {
            get { return wrappedSyntax.name(); }
        }

        public string owner
        {
            get { return _owner; }
        }
    }
}
