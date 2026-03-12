using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace captainalm.calmcmd
{
    internal sealed class LegacyLoader : ILegacyLoader
    {
        private LegacyHookHolder hookHolder;
        private LegacyHookRunnerHolder _hookRunnerHolder;
        private bool registerFirstLegacySyntaxesToStandardLibrary = true;
        private bool registerFirstLegacyCommandsToStandardLibrary = true;
        //Previous values of the manipulator during the last check:
        private Stack<string> OldCommandStack;
        private Dictionary<string, string> OldVariableDictionary;
        private string OldSyntaxMode;
        //Previous values of the manipulator data stored by the new system during the last check:
        private Stack<string> OldNewCommandStack;
        private Dictionary<string, string> OldNewVariableDictionary;
        private string OldNewSyntaxMode;
        private object slocksmv = new object();
        //Define external command and syntax registry:
        private Dictionary<int, LegacyCommand> ecmdreg = new Dictionary<int, LegacyCommand>();
        private Dictionary<int, LegacySyntax> esnxreg = new Dictionary<int,LegacySyntax>();
        private int ecmdregk = 0;
        private int esnxregk = 0;
        private object slockecmdreg = new object();
        private object slockesnxreg = new object();
        private object slockecmdregk = new object();
        private object slockesnxregk = new object();
        //Define the legacy hooks
        public captainalm.calmcon.api.Types.RunCommandHook runcommandhook;
        public captainalm.calmcon.api.Types.AddExternalCommandHook addecmdhook;
        public captainalm.calmcon.api.Types.AddExternalSyntaxHook addesnxhook;
        public captainalm.calmcon.api.Types.RemoveExternalCommandHook remecmdhook;
        public captainalm.calmcon.api.Types.RemoveExternalSyntaxHook remesnxhook;
        public captainalm.calmcon.api.Types.ListExternalCommandsHook lsecmdhook;
        public captainalm.calmcon.api.Types.ListExternalSyntaxesHook lsesnxhook;
        public captainalm.calmcon.api.Types.ReadOutputHook rohook;
        public captainalm.calmcon.api.Types.WriteOutputHook wohook;

        public bool RegisterFirstLegacySyntaxesToStandardLibrary
        {
            get
            {
                return registerFirstLegacySyntaxesToStandardLibrary;
            }
            set
            {
                registerFirstLegacySyntaxesToStandardLibrary = value;
            }
        }

        public bool RegisterFirstLegacyCommandsToStandardLibrary
        {
            get
            {
                return registerFirstLegacyCommandsToStandardLibrary;
            }
            set
            {
                registerFirstLegacyCommandsToStandardLibrary = value;
            }
        }

        //Constructor:
        public LegacyLoader()
        {
            hookHolder = new LegacyHookHolder(this);
            runcommandhook = delegate(string c, string[] a)
            {
                ICommand cmd = Registry.getCommand(c);
                if (object.ReferenceEquals(cmd, null)) cmd = API.invalidCommand;
                var ts = new object[a.Length];
                if (a.Length > 0)
                {
                    for (int i = 0; i < a.Length; i++)
                    {
                        ts[i] = a[i];
                    }
                }
                object toret = cmd.run(ts);
                if (object.ReferenceEquals(toret, null)) return "";
                if (toret.GetType() == typeof(string)) return (string)toret;
                if (toret.GetType() == typeof(captainalm.calmcon.api.OutputText)) return (captainalm.calmcon.api.OutputText)toret;
                return toret.ToString();
            };
            addecmdhook = delegate(string l, captainalm.calmcon.api.Command c)
            {
                var k = setupNextKey(ref ecmdregk, ref slockecmdregk);
                if (k != 0)
                {
                    lock (slockecmdreg)
                    {
                        ecmdreg.Add(k, new LegacyCommand(c, l));
                        try
                        {
                            Registry.registerCommand(ecmdreg[k]);
                        }
                        catch (CaptainALMConsoleException)
                        {
                            ecmdreg.Remove(k);
                            throw new Exception("Command Already Exists!");
                        }
                    }
                }
                return k;
            };
            addesnxhook = delegate(string l, captainalm.calmcon.api.ISyntax s)
            {
                var k = setupNextKey(ref esnxregk, ref slockesnxregk);
                if (k != 0)
                {
                    lock (slockesnxreg)
                    {
                        esnxreg.Add(k, new LegacySyntax(s, l));
                        try
                        {
                            Registry.registerSyntax(esnxreg[k]);
                        }
                        catch (CaptainALMConsoleException)
                        {
                            esnxreg.Remove(k);
                            throw new Exception("Syntax Already Exists!");
                        }
                    }
                }
                return k;
            };
            remecmdhook = delegate(string l, int id)
            {
                lock (slockecmdreg)
                {
                    if (ecmdreg.ContainsKey(id) && ecmdreg[id].owner.Equals(l))
                    {
                        Registry.removeCommand(ecmdreg[id]);
                        ecmdreg.Remove(id);
                        return true;
                    }
                }
                return false;
            };
            remesnxhook = delegate(string l, int id)
            {
                lock (slockesnxreg)
                {
                    if (esnxreg.ContainsKey(id) && esnxreg[id].owner.Equals(l))
                    {
                        Registry.removeSyntax(esnxreg[id]);
                        esnxreg.Remove(id);
                        return true;
                    }
                }
                return false;
            };
            lsecmdhook = delegate(string l)
            {
                var toret = new List<string>();
                lock (slockecmdreg)
                {
                    foreach (int c in ecmdreg.Keys)
                    {
                        if (ecmdreg[c].owner.Equals(l)) toret.Add(c.ToString() + " : " + ecmdreg[c].name);
                    }
                }
                return toret.ToArray();
            };
            lsesnxhook = delegate(string l)
            {
                var toret = new List<string>();
                lock (slockesnxreg)
                {
                    foreach (int c in esnxreg.Keys)
                    {
                        if (esnxreg[c].owner.Equals(l)) toret.Add(c.ToString() + " : " + esnxreg[c].name);
                    }
                }
                return toret.ToArray();
            };
            rohook = delegate() 
            {
                ILogger llog = Loader.logger;
                if (object.ReferenceEquals(llog, null))
                    return "";
                return llog.getLog();
            };
            wohook = delegate(captainalm.calmcon.api.OutputText str)
            {
                ILogger llog = Loader.logger;
                if (object.ReferenceEquals(llog, null))
                    return;
                llog.writeEntry(convertOutputTextToStylableString(str));
            };
        }

        public bool loadAssembly(Assembly assemblyIn)
        {
            if (object.ReferenceEquals(assemblyIn, null)) return false;
            var toret = true;
            foreach (Type c in assemblyIn.GetTypes())
            {
                if (!c.IsAbstract & !c.IsInterface & !c.IsEnum)
                {
                    toret &= typeProcessor(c);
                }
            }
            return toret;
        }

        public void executeStartHooks()
        {
            foreach (string t in hookHolder.hookInformation.Keys)
            {
                var c = hookHolder.hookInformation[t];
                var v = (LegacyHookHolder)hookHolder.Clone();
                v.libraryNameFFA = t;
                c.hook_runcommand();
                c.hook_readoutput();
                c.hook_writeoutput();
                c.hook_e_cmd_add();
                c.hook_e_cmd_list();
                c.hook_e_cmd_remove();
                c.hook_e_snx_add();
                c.hook_e_snx_list();
                c.hook_e_snx_remove();
                c.hook_form();
                c.hook_cmd_txtbx();
                c.hook_out_txtbx();
                c.hook_programstart();
            }
        }

        public void executeEndHooks()
        {
            foreach (string t in hookHolder.hookInformation.Keys)
            {
                hookHolder.hookInformation[t].hook_programstop();
            }
        }

        public bool executePreCHooks(string cmdln)
        {
            foreach (string t in hookHolder.hookInformation.Keys)
            {
                hookHolder.hookInformation[t].hook_command_preexecute(cmdln);
            }
            return true;
        }

        public void executePostCHooks(string cmdln, object objOut)
        {
            foreach (string t in hookHolder.hookInformation.Keys)
            {
                hookHolder.hookInformation[t].hook_command_postexecute(cmdln, object.ReferenceEquals(objOut, null) ? (StylableString)"" : (objOut.GetType() == typeof(string) ? (StylableString)((string)objOut) : (objOut.GetType() == typeof(captainalm.calmcon.api.OutputText) ? LegacyLoader.ConvertOutputTextToStylableString((captainalm.calmcon.api.OutputText)objOut) : (objOut.GetType() == typeof(StylableString) ? (StylableString)objOut : (StylableString)objOut.ToString()))));
            }
        }

        public void syncManipulatorValues()
        {
            lock (slocksmv) lock (Processor.slockCommandStack) lock (API.slocksvd)
            {
                makeSureNewManipulatorNotNull();
                makeSureManipulatorNotNull();
                makeSureSavedNotNull();
                makeSureNewSavedNotNull();
                //For each, check if the two values are different, then:
                //Check the old vs new values for each and the one that differs and not null gets selected
                //Instead of testing for null in the unsafe (volatile) legacy manipulator environment, auto set the value in a local variable before testing for null
                //If both differ or legacy was null, prefer the new system value (Which should never be null)
                if (!object.ReferenceEquals(captainalm.calmcon.api.Manipulator.CommandStack, Processor.CommandStack))
                {
                    Stack<string> lh = captainalm.calmcon.api.Manipulator.CommandStack;
                    if ((!object.ReferenceEquals(lh, OldCommandStack)) && object.ReferenceEquals(Processor.CommandStack, OldNewCommandStack) && (!object.ReferenceEquals(lh, null)))
                    {
                        Processor.CommandStack = lh;
                    }
                    else
                    {
                        captainalm.calmcon.api.Manipulator.CommandStack = Processor.CommandStack;
                    }
                }
                if (!object.ReferenceEquals(captainalm.calmcon.api.Manipulator.SyntaxMode, API.getCurrentSyntax()))
                {
                    string lh = captainalm.calmcon.api.Manipulator.SyntaxMode;
                    if ((!object.ReferenceEquals(lh, OldSyntaxMode)) && object.ReferenceEquals(API.getCurrentSyntax(), OldNewSyntaxMode) && (!object.ReferenceEquals(lh, null)))
                    {
                        API.setSyntax(lh);
                    }
                    else
                    {
                        captainalm.calmcon.api.Manipulator.SyntaxMode = API.getCurrentSyntax();
                    }
                }
                if (!object.ReferenceEquals(captainalm.calmcon.api.Manipulator.VariableDictionary, API.StringVariableDictionary))
                {
                    Dictionary<string, string> lh = captainalm.calmcon.api.Manipulator.VariableDictionary;
                    if ((!object.ReferenceEquals(lh, OldVariableDictionary)) && object.ReferenceEquals(API.StringVariableDictionary, OldNewVariableDictionary) && (!object.ReferenceEquals(lh, null)))
                    {
                        API.StringVariableDictionary = lh;
                    }
                    else
                    {
                        captainalm.calmcon.api.Manipulator.VariableDictionary = API.StringVariableDictionary;
                    }
                }
                //Set up old values
                OldCommandStack = captainalm.calmcon.api.Manipulator.CommandStack;
                OldSyntaxMode = captainalm.calmcon.api.Manipulator.SyntaxMode;
                OldVariableDictionary = captainalm.calmcon.api.Manipulator.VariableDictionary;
                OldNewCommandStack = Processor.CommandStack;
                OldNewSyntaxMode = API.getCurrentSyntax();
                OldNewVariableDictionary = API.StringVariableDictionary;
            }
        }
        //INFO: Make sure subs have been synclocked for local in the syncManipulatorValues and should only be called from <-
        //These use the values of the new system
        private void makeSureManipulatorNotNull()
        {
            if (object.ReferenceEquals(captainalm.calmcon.api.Manipulator.CommandStack, null)) captainalm.calmcon.api.Manipulator.CommandStack = Processor.CommandStack;
            if (object.ReferenceEquals(captainalm.calmcon.api.Manipulator.SyntaxMode, null)) captainalm.calmcon.api.Manipulator.SyntaxMode = API.getCurrentSyntax();
            if (object.ReferenceEquals(captainalm.calmcon.api.Manipulator.VariableDictionary, null)) captainalm.calmcon.api.Manipulator.VariableDictionary = API.StringVariableDictionary;
        }
        //These should always be not null
        private void makeSureNewManipulatorNotNull()
        {
            if (object.ReferenceEquals(Processor.CommandStack, null)) Processor.CommandStack = new Stack<string>();
            if (object.ReferenceEquals(API.getCurrentSyntax(), null)) API.setSyntax("");
            if (object.ReferenceEquals(API.StringVariableDictionary, null)) API.StringVariableDictionary = new Dictionary<string,string>();
        }
        //Hopefully makes not null by copying the values from the new system
        private void makeSureSavedNotNull()
        {
            if (object.ReferenceEquals(OldCommandStack, null)) OldCommandStack = Processor.CommandStack;
            if (object.ReferenceEquals(OldSyntaxMode, null)) OldSyntaxMode = API.getCurrentSyntax();
            if (object.ReferenceEquals(OldVariableDictionary, null)) OldVariableDictionary = API.StringVariableDictionary;
        }
        //Hopefully makes not null by copying the values from the respective source
        private void makeSureNewSavedNotNull()
        {
            if (object.ReferenceEquals(OldNewCommandStack, null)) OldNewCommandStack = Processor.CommandStack;
            if (object.ReferenceEquals(OldNewSyntaxMode, null)) OldNewSyntaxMode = API.getCurrentSyntax();
            if (object.ReferenceEquals(OldNewVariableDictionary, null)) OldNewVariableDictionary = API.StringVariableDictionary;
        }

        private bool typeProcessor(Type typeIn)
        {
            MethodInfo initMethod = null;
            object instanceType = null;
            captainalm.calmcon.api.LibrarySetup libs;
            try
            {
                foreach (MethodInfo c in typeIn.GetMethods(BindingFlags.Public | BindingFlags.Instance))
                {
                    captainalm.calmcon.api.SetupMethodAttribute[] aarr = (captainalm.calmcon.api.SetupMethodAttribute[])c.GetCustomAttributes(typeof(captainalm.calmcon.api.SetupMethodAttribute), false);
                    if (aarr.Length != 0 & c.ReturnType == typeof(captainalm.calmcon.api.LibrarySetup)) { initMethod = c; }
                }
                instanceType = typeIn.Assembly.CreateInstance(typeIn.FullName);
            }
            catch (ThreadAbortException ex) { throw ex; }
            catch (Exception) { return false; }
            if (initMethod != null && ! object.ReferenceEquals(instanceType, null))
            {
                try
                {
                    libs = (captainalm.calmcon.api.LibrarySetup)initMethod.Invoke(instanceType, null);
                }
                catch (ThreadAbortException ex) { throw ex; }
                catch (Exception) { return false; }
                if (!hookHolder.hookInformation.ContainsKey(libs.hook_info.name)) hookHolder.hookInformation.Add(libs.hook_info.name, new HookInfoWrapper(libs.hook_info, this));
                if (libs.syntaxes != null) {
                    foreach (captainalm.calmcon.api.ISyntax c in libs.syntaxes)
                    {
                        if (registerFirstLegacySyntaxesToStandardLibrary && object.ReferenceEquals(Registry.getSyntax(c.name()), null))
                        {
                            var lstsstd = new LegacySyntax(c, "");
                            Registry.registerSyntax(lstsstd);
                        }
                        var lsts = new LegacySyntax(c, libs.name);
                        Registry.registerSyntax(lsts);
                    } 
                }
                if (libs.commands != null)
                {
                    foreach (captainalm.calmcon.api.Command c in libs.commands)
                    {
                        if (registerFirstLegacyCommandsToStandardLibrary && object.ReferenceEquals(Registry.getCommand(c.name), null))
                        {
                            var lctsstd = new LegacyCommand(c, "");
                            Registry.registerCommand(lctsstd);
                        }
                        var lcts = new LegacyCommand(c, libs.name);
                        Registry.registerCommand(lcts);
                    }
                }

            }
            return true;
        }

        private int setupNextKey(ref int ckey, ref object locker)
        {
            int toret = 0;
            lock (locker)
            {
                if (ckey == int.MaxValue) { ckey = int.MinValue; }
                else
                {
                    if (ckey == -1) throw new Exception("Key Reached Limit!"); else ckey += 1;
                }
                toret = ckey;
            }
            return toret;
        }

        public LegacyHookHolder getHookHolder()
        {
            return hookHolder;
        }

        public LegacyHookRunnerHolder hookRunnerHolder
        {
            get
            {
                return _hookRunnerHolder;
            }
            set
            {
                _hookRunnerHolder = value;
            }
        }

        public LegacyHookHolder newHookHolder()
        {
            return new LegacyHookHolder(this);
        }

        public StylableString convertOutputTextToStylableString(object objIn)
        {
            return LegacyLoader.ConvertOutputTextToStylableString(objIn);
        }

        public static StylableString ConvertOutputTextToStylableString(object objIn)
        {
            if (objIn.GetType() == typeof(captainalm.calmcon.api.OutputText))
            {
                var oi = (captainalm.calmcon.api.OutputText)objIn;
                var oia = oi.ToOutputTextBlocks();
                if (oia.Length > 1)
                {
                    for (int i = 1; i < oia.Length; i++)
                    {
                        oia[0].text += oia[i].text;
                    }
                }
                if (oia.Length > 0) return LegacyLoader.ConvertOutputTextToStylableString(oia[0]);
            }
            else if (objIn.GetType() == typeof(captainalm.calmcon.api.OutputTextBlock))
            {
                var oi = (captainalm.calmcon.api.OutputTextBlock)objIn;
                return new StylableString(oi.text) { backcolor = System.Drawing.Color.Empty, forecolor = oi.forecolor, bold = oi.bold, italic = oi.italic, strikeout = oi.strikeout, underline = oi.underline };
            }
            return default(StylableString);
        }

        public static object ConvertStylableStringToOutputText(StylableString strIn)
        {
            return new captainalm.calmcon.api.OutputText(strIn.text, strIn.forecolor, strIn.bold, strIn.italic, strIn.underline, strIn.strikeout);
        }
    }
}
