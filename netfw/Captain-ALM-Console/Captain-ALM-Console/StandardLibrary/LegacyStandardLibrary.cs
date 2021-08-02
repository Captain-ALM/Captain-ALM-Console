using System;
using System.Collections.Generic;

namespace captainalm.calmcmd
{
    /// <summary>
    /// Provides the Standard Library in legacy mode.
    /// Extend this class and override functionality to modify the standard library in legacy mode.
    /// Then use the modified standard library class in Loader.
    /// </summary>
    public class LegacyStandardLibrary : StandardLibrary
    {
        protected override object writeline(object[] args)
        {
            if (args.Length == 0) return "";
            return base.writeline(args);
        }

        protected virtual object str_conv_legacy(object[] args)
        {
            return args[0].ToString();
        }

        protected virtual object int_conv_legacy(object[] args)
        {
            try
            {
                return int.Parse(args[0].ToString());
            }
            catch (Exception)
            {
                return (int)0;
            }
        }

        protected virtual object dec_conv_legacy(object[] args)
        {
            try
            {
                return decimal.Parse(args[0].ToString());
            }
            catch (Exception)
            {
                return (decimal)0;
            }
        }

        protected virtual object char_conv_legacy(object[] args)
        {
            if ((long)args[0] < -32768 || (long)args[0] > 65535) throw new ArgumentException("Argument 'CharCode' must be within the range of -32768 to 65535.");
            return Convert.ToChar((int)args[0] & 65535);
        }

        protected override object help(object[] args)
        {
            var hlp = (string[])base.help(args);
            var toret = "Help:" + Environment.NewLine;
            if (args.Length != 0)
            {
                toret += "Help Returned For '" + args[0].ToString() + "'" + Environment.NewLine;
            }
            for (int i = 0; i < hlp.Length; i++)
            {
                toret += hlp[i];
                if (i != (hlp.Length - 1)) toret += Environment.NewLine;
            }
            return toret;
        }

        protected virtual object help_ext(object[] args)
        {
            var hlp = (string[])base.help(args);
            var toret = "Extended Help:" + Environment.NewLine;
            if (args.Length != 0)
            {
                toret += "Extended Help Returned For '" + args[0].ToString() + "'" + Environment.NewLine;
            }
            for (int i = 0; i < hlp.Length; i++)
            {
                toret += hlp[i];
                if (i != (hlp.Length - 1)) toret += Environment.NewLine;
            }
            return toret;
        }

        protected override object lang(object[] args)
        {
            API.setSyntax((string)args[0]);
            return "Syntax Language Changed to: " + API.getCurrentSyntax();
        }

        protected override object langs(object[] args)
        {
            return "Languages/Syntaxes:" + Environment.NewLine + string.Join(Environment.NewLine, (string[])base.langs(args));
        }

        protected virtual object outputtext_to_stylablestring(object[] args)
        {
            API.assertNullArguments(args, nullargsfor1notallowing);
            return API.convertOutputTextToStylableString(args[0]);
        }

        /// <summary>
        /// Gets the array of commands that makes up the standard library
        /// </summary>
        /// <returns>An array of commands</returns>
        public override Command[] getLibrary()
        {
            var toret = new List<Command>(base.getLibrary());

            toret.Add(new Command(base.writeline, "", "writeline_new", "writeline_new (output:String) > String"));

            toret.Add(new Command(str_conv_legacy, "", "str", "str %input:System.Object% > System.String"));
            toret.Add(new Command(str_conv_legacy, "", "string", "string %input:System.Object% > System.String"));

            toret.Add(new Command(int_conv_legacy, "", "int", "int %input:System.Object% > System.Int32"));
            toret.Add(new Command(int_conv_legacy, "", "integer", "integer %input:System.Object% > System.Int32"));

            toret.Add(new Command(dec_conv_legacy, "", "dec", "dec %input:System.Object% > System.Decimal"));
            toret.Add(new Command(dec_conv_legacy, "", "decimal", "decimal %input:System.Object% > System.Decimal"));

            toret.Add(new Command(char_conv_legacy, "", "char_conv", "char_conv %input:Intergral% > System.Char"));
            toret.Add(new Command(char_conv_legacy, "", "character_convert", "char_conv %input:Intergral% > System.Char"));

            toret.Find(delegate(Command x) { return NameComparer(x, "help"); }).help = "help (search constraint:String) > System.String";
            toret.Add(new Command(base.help, "", "help_new", "help_new (search constraint:String) > System.String[]"));
            toret.Add(new Command(help_ext, "", "help_ext", "help_ext (search constraint:String) > System.String"));
            toret.Add(new Command(help_ext, "", "help_extended", "help_extended (search constraint:String) > System.String"));

            toret.Add(new Command(base.lang, "", "syntax_new", "syntax_new (syntax name:String) > System.String"));
            toret.Add(new Command(lang, "", "lang", "lang (syntax name:String) > System.String"));
            toret.Add(new Command(lang, "", "language", "language (syntax name:String) > System.String"));
            toret.Find(delegate(Command x) { return NameComparer(x, "syntaxes"); }).help = "syntaxes > System.String";
            toret.Add(new Command(base.langs, "", "syntaxes_new", "syntaxes_new > System.String[]"));
            toret.Add(new Command(langs, "", "langs", "langs > System.String"));
            toret.Add(new Command(langs, "", "languages", "languages > System.String"));

            toret.Add(new Command(outputtext_to_stylablestring, "", "outputtext_to_stylablestring", "outputtext_to_stylablestring %input OutputText:System.Object% > captainalm.calmcmd.StylableString"));

            return toret.ToArray();
        }
    }
}
