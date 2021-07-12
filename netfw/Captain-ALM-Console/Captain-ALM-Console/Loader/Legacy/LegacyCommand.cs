using System;

namespace captainalm.calmcmd
{
    internal sealed class LegacyCommand : ICommand
    {
        public captainalm.calmcon.api.Command wrappedCommand;
        private string _owner;

        public LegacyCommand(captainalm.calmcon.api.Command comIn, string owner)
        {
            wrappedCommand = comIn;
            _owner = owner;
        }

        public object run(object[] argumentsIn)
        {
            var ts = new string[argumentsIn.Length];
            if (argumentsIn.Length > 0)
            {
                for (int i = 0; i < argumentsIn.Length; i++)
                {
                    ts[i] = argumentsIn[i].ToString();
                }
            }
            return wrappedCommand.command.Invoke(ts);
        }

        public string help
        {
            get { return wrappedCommand.help; }
        }

        public string name
        {
            get { return wrappedCommand.name; }
        }

        public string owner
        {
            get { return _owner; }
        }
    }
}
