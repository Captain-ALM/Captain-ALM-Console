using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace captainalm.calmcmd
{
    internal sealed class LegacyLoader
    {
        public LegacyHookHolder hookHolder = new LegacyHookHolder();
        public LegacyHookRunnerHolder hookRunnerHolder;
        public bool registerFirstLegacySyntaxesToStandardLibrary = true;
        public bool registerFirstLegacyCommandsToStandardLibrary = true;
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
        private captainalm.calmcon.api.Types.RunCommandHook runcommandhook;
        private captainalm.calmcon.api.Types.AddExternalCommandHook addecmdhook;
        private captainalm.calmcon.api.Types.AddExternalSyntaxHook addesnxhook;
        private captainalm.calmcon.api.Types.RemoveExternalCommandHook remecmdhook;
        private captainalm.calmcon.api.Types.RemoveExternalSyntaxHook remesnxhook;
        private captainalm.calmcon.api.Types.ListExternalCommandsHook lsecmdhook;
        private captainalm.calmcon.api.Types.ListExternalSyntaxesHook lsesnxhook;

        //Checks if the legacy system is actually supported:
        public static bool legacySupported()
        {
            return new captainalm.calmcon.api.LibrarySetup().GetType() == typeof(captainalm.calmcon.api.LibrarySetup);
        }

        //Constructor:
        public LegacyLoader()
        {
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
                        catch (CaptainALMConsoleException e)
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
                        catch (CaptainALMConsoleException e)
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
        }

        public StylableString convertOutputTextToStylableString(object objIn)
        {
            if (objIn.GetType() == typeof(captainalm.calmcon.api.OutputText))
            {
                var oi = (captainalm.calmcon.api.OutputText)objIn;
                var oia = oi.ToOutputTextBlocks();
                if (oia.Length > 0) return this.convertOutputTextToStylableString(oia[0]);
            }
            else if (objIn.GetType() == typeof(captainalm.calmcon.api.OutputTextBlock))
            {
                var oi = (captainalm.calmcon.api.OutputTextBlock) objIn;
                return new StylableString(oi.text) { backcolor = System.Drawing.Color.Empty, forecolor = oi.forecolor, bold = oi.bold, italic = oi.italic, strikeout = oi.strikeout, underline = oi.underline };
            }
            return default(StylableString);
        }

        public bool loadAssembly(Assembly assemblyIn)
        {
            if (object.ReferenceEquals(assemblyIn, null)) return false;
            var toret = true;
            foreach (Type c in assemblyIn.GetTypes())
            {
                if (!c.IsAbstract & !c.IsInterface)
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
                if (!object.ReferenceEquals(c.hook_runcommand, null)) c.hook_runcommand.Invoke(ref runcommandhook);
                if (!object.ReferenceEquals(c.hook_readoutput, null)) if (!object.ReferenceEquals(hookRunnerHolder.GetReadOutputHook, null)) hookRunnerHolder.GetReadOutputHook.Invoke(v);
                if (!object.ReferenceEquals(c.hook_writeoutput, null)) if (!object.ReferenceEquals(hookRunnerHolder.GetWriteOutputHook, null)) hookRunnerHolder.GetWriteOutputHook.Invoke(v);
                if (!object.ReferenceEquals(c.hook_e_cmd_add, null)) c.hook_e_cmd_add.Invoke(ref addecmdhook);
                if (!object.ReferenceEquals(c.hook_e_cmd_list, null)) c.hook_e_cmd_list.Invoke(ref lsecmdhook);
                if (!object.ReferenceEquals(c.hook_e_cmd_remove, null)) c.hook_e_cmd_remove.Invoke(ref remecmdhook);
                if (!object.ReferenceEquals(c.hook_e_snx_add, null)) c.hook_e_snx_add.Invoke(ref addesnxhook);
                if (!object.ReferenceEquals(c.hook_e_snx_list, null)) c.hook_e_snx_list.Invoke(ref lsesnxhook);
                if (!object.ReferenceEquals(c.hook_e_snx_remove, null)) c.hook_e_snx_remove.Invoke(ref remesnxhook);
                if (!object.ReferenceEquals(c.hook_form, null)) if (!object.ReferenceEquals(hookRunnerHolder.FormHook, null)) hookRunnerHolder.FormHook.Invoke(v);
                if (!object.ReferenceEquals(c.hook_cmd_txtbx, null)) if (!object.ReferenceEquals(hookRunnerHolder.CommandTextboxHook, null)) hookRunnerHolder.CommandTextboxHook.Invoke(v);
                if (!object.ReferenceEquals(c.hook_out_txtbx, null)) if (!object.ReferenceEquals(hookRunnerHolder.OutputTextboxHook, null)) hookRunnerHolder.OutputTextboxHook.Invoke(v);
                if (!object.ReferenceEquals(c.hook_programstart, null)) c.hook_programstart.Invoke();
            }
        }

        public void executeEndHooks()
        {
            foreach (string t in hookHolder.hookInformation.Keys)
            {
                var c = hookHolder.hookInformation[t];
                if (!object.ReferenceEquals(c.hook_programstop, null)) c.hook_programstop.Invoke();
            }
        }

        public bool executePreCHooks(string cmdln)
        {
            foreach (string t in hookHolder.hookInformation.Keys)
            {
                var c = hookHolder.hookInformation[t];
                if (!object.ReferenceEquals(c.hook_command_preexecute, null)) c.hook_command_preexecute.Invoke(cmdln);
            }
            return true;
        }

        public void executePostCHooks(string cmdln, object objOut)
        {
            foreach (string t in hookHolder.hookInformation.Keys)
            {
                var c = hookHolder.hookInformation[t];
                if (!object.ReferenceEquals(c.hook_command_postexecute, null)) c.hook_command_postexecute.Invoke(cmdln, object.ReferenceEquals(objOut, null) ? (captainalm.calmcon.api.OutputText)"" : (objOut.GetType() == typeof(string) ? (captainalm.calmcon.api.OutputText)((string)objOut) : (objOut.GetType() == typeof(captainalm.calmcon.api.OutputText) ? (captainalm.calmcon.api.OutputText)objOut : (captainalm.calmcon.api.OutputText)objOut.ToString())));
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
            catch (Exception ex) { return false; }
            if (initMethod != null && instanceType != null)
            {
                try
                {
                    libs = (captainalm.calmcon.api.LibrarySetup)initMethod.Invoke(instanceType, null);
                }
                catch (ThreadAbortException ex) { throw ex; }
                catch (Exception ex) { return false; }
                if (!hookHolder.hookInformation.ContainsKey(libs.hook_info.name)) hookHolder.hookInformation.Add(libs.hook_info.name, libs.hook_info);
                if (libs.syntaxes != null) {
                    foreach (captainalm.calmcon.api.ISyntax c in libs.syntaxes)
                    {
                        if (registerFirstLegacySyntaxesToStandardLibrary && Registry.getSyntax(c.name()) == null)
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
                        if (registerFirstLegacyCommandsToStandardLibrary && Registry.getCommand(c.name) == null)
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
    }
}
