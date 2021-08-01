using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace captainalm.calmcmd
{
    /// <summary>
    /// Provides the Standard Library.
    /// Extend this class and override functionality to modify the standard library.
    /// Then use the modified standard library class in Loader.
    /// </summary>
    public class StandardLibrary
    {
        protected Random rand = new Random();

        protected bool[] nullargsfor1notallowing = new bool[] { false };
        protected bool[] nullargsfor1allowing = new bool[] { true };
        protected bool[] nullargsfor2notallowing = new bool[] { false, false };
        protected bool[] nullargsfor1notallowing1allowing = new bool[] { false, true };
        protected bool[] nullargsfor2allowing = new bool[] { true, true };
        protected bool[] nullargsfor3notallowing = new bool[] { false, false, false };
        protected bool[] nullargsfor2notallowing1allowing = new bool[] { false, false, true };
        protected bool[] nullargsfor4notallowing = new bool[] { false, false, false, false };

        protected Type[] argsfor1string = new Type[] { typeof(string) };
        protected Type[] argsfor2bool = new Type[] { typeof(bool), typeof(bool) };
        protected Type[] argsfor2string = new Type[] { typeof(string), typeof(string) };
        protected Type[] argsfor1string1object = new Type[] { typeof(string), typeof(object) };
        protected Type[] argsfor1long = new Type[] { typeof(long) };
        protected Type[] argsfor1ulong = new Type[] { typeof(ulong) };
        protected Type[] argsfor1object1type = new Type[] { typeof(object), typeof(Type) };
        protected Type[] argsfor1object = new Type[] { typeof(object) };
        protected Type[] argsfor1icommand1objectarr = new Type[] { typeof(ICommand), typeof(object[]) };
        protected Type[] argsfor1string1isyntax = new Type[] { typeof(string), typeof(ISyntax) };
        protected Type[] argsfor1bool1object = new Type[] { typeof(bool), typeof(object) };
        protected Type[] argsfor1bool = new Type[] { typeof(bool) };
        protected Type[] argsfor1int = new Type[] { typeof(int) };
        protected Type[] argsfor2int = new Type[] { typeof(int), typeof(int) };
        protected Type[] argsfor2object = new Type[] { typeof(object), typeof(object) };
        protected Type[] argsfor2icomparable = new Type[] { typeof(IComparable), typeof(IComparable) };
        protected Type[] argsfor1type = new Type[] { typeof(Type) };
        protected Type[] argsfor1type1int = new Type[] { typeof(Type), typeof(int) };
        protected Type[] argsfor1object2int = new Type[] { typeof(object), typeof(int), typeof(int) };
        protected Type[] argsfor1ilist = new Type[] { typeof(IList) };
        protected Type[] argsfor1ilist2int = new Type[] { typeof(IList), typeof(int), typeof(int) };
        protected Type[] argsfor1ilist1object = new Type[] { typeof(IList), typeof(object) };
        protected Type[] argsfor1ilist1int = new Type[] { typeof(IList), typeof(int) };
        protected Type[] argsfor1ilist1int1object = new Type[] { typeof(IList), typeof(int), typeof(object) };
        protected Type[] argsfor3int = new Type[] { typeof(int), typeof(int), typeof(int) };
        protected Type[] argsfor4int = new Type[] { typeof(int), typeof(int), typeof(int), typeof(int) };
        protected Type[] argsfor1stringarr = new Type[] { typeof(string[]) };
        protected Type[] argsfor1stringarr1string = new Type[] { typeof(string[]), typeof(string) };

        //Inputing:
        protected virtual object input_req(object[] args)
        {
            return API.requestUserInput();
        }

        //Outputing:
        protected virtual object echo(object[] args)
        {
            API.assertNullArguments(args, nullargsfor1notallowing);
            return args[0];
        }
        protected virtual object write(object[] args)
        {
            if (args.Length == 0) return "";
            API.assertNullArguments(args, nullargsfor1notallowing);
            API.assertArguments(args, argsfor1string);
            return args[0];
        }
        protected virtual object writeline(object[] args)
        {
            if (args.Length == 0) return Environment.NewLine;
            API.assertNullArguments(args, nullargsfor1notallowing);
            API.assertArguments(args, argsfor1string);
            if (args[0].GetType() == typeof(StylableString)) return ((StylableString)args[0]) + Environment.NewLine;
            return args[0].ToString() + Environment.NewLine;
        }

        //Variable dictionaries:
        protected virtual object get_str_var(object[] args)
        {
            API.assertNullArguments(args, nullargsfor1notallowing);
            API.assertArguments(args, argsfor1string);
            if (args.Length > 0)
            {
                return API.getStringVariable(args[0].ToString());
            }
            return "No variable name specified.";
        }
        protected virtual object set_str_var(object[] args)
        {
            API.assertNullArguments(args, nullargsfor2notallowing);
            API.assertArguments(args, argsfor2string);
            if (args.Length > 1)
            {
                API.setStringVariable(args[0].ToString(), args[1].ToString());
                return args[1];
            }
            return "No variable name and/or value specified.";
        }
        protected virtual object get_var(object[] args)
        {
            API.assertNullArguments(args, nullargsfor1notallowing);
            API.assertArguments(args, argsfor1string);
            if (args.Length > 0)
            {
                return API.getVariable(args[0].ToString());
            }
            return "No variable name specified.";
        }
        protected virtual object set_var(object[] args)
        {
            API.assertNullArguments(args, nullargsfor1notallowing1allowing);
            API.assertArguments(args, argsfor1string1object);
            if (args.Length > 1)
            {
                API.setVariable(args[0].ToString(), args[1]);
                return args[1];
            }
            return "No variable name and/or value specified.";
        }

        //Invalid:
        protected virtual object invalid(object[] args)
        {
            return "Invalid Command";
        }

        //Reference Creators:
        protected virtual object get_null(object[] args)
        {
            return null;
        }
        protected virtual object create_object(object[] args)
        {
            return new object();
        }
        protected virtual object create_intptr(object[] args)
        {
            if (args.Length == 0) return new IntPtr();
            API.assertNullArguments(args, nullargsfor1allowing);
            if (object.ReferenceEquals(args[0], null)) return new IntPtr();
            API.assertArguments(args, argsfor1long);
            if (args[0].GetType() == typeof(int))
            {
                return new IntPtr((int)args[0]);
            }
            else if (args[0].GetType() == typeof(long))
            {
                return new IntPtr((long)args[0]);
            }
            return new IntPtr();
        }
        protected virtual object create_uintptr(object[] args)
        {
            if (args.Length == 0) return new UIntPtr();
            API.assertNullArguments(args, nullargsfor1allowing);
            if (object.ReferenceEquals(args[0], null)) return new UIntPtr();
            API.assertArguments(args, argsfor1ulong);
            if (args[0].GetType() == typeof(uint))
            {
                return new UIntPtr((uint)args[0]);
            }
            else if (args[0].GetType() == typeof(ulong))
            {
                return new UIntPtr((ulong)args[0]);
            }
            return new UIntPtr();
        }
        protected virtual object random(object[] args)
        {
            if (args.Length == 1)
            {
                API.assertNullArguments(args, nullargsfor1notallowing);
                API.assertArguments(args, argsfor1int);
                return rand.Next((int)args[0]);
            }
            else if (args.Length > 1)
            {
                API.assertNullArguments(args, nullargsfor2notallowing);
                API.assertArguments(args, argsfor2int);
                return rand.Next((int)args[1], (int)args[0]);
            }
            else
            {
                return rand.Next();
            }
        }
        protected virtual object create_color(object[] args)
        {
            if (args.Length > 1)
            {
                if (args.Length == 3)
                {
                    API.assertNullArguments(args, nullargsfor3notallowing);
                    API.assertArguments(args, argsfor3int);
                    return Color.FromArgb((int)args[0], (int)args[1], (int)args[2]);
                }
                else
                {
                    API.assertNullArguments(args, nullargsfor4notallowing);
                    API.assertArguments(args, argsfor4int);
                    return Color.FromArgb((int)args[3], (int)args[0], (int)args[1], (int)args[2]);
                }
            }
            else
            {
                API.assertNullArguments(args, nullargsfor1notallowing);
                if (args[0].GetType() == typeof(int))
                {
                    return Color.FromArgb((int)args[0]);
                }
                else
                {
                    API.assertArguments(args, argsfor1string);
                    if (args[0].ToString().StartsWith("#"))
                    {
                        var strarg = args[0].ToString().ToUpper().Substring(1);
                        if (strarg.Length == 6) strarg = "FF" + strarg;
                        return Color.FromArgb(int.Parse(strarg, System.Globalization.NumberStyles.HexNumber));
                    }
                    else
                    {
                        return Color.FromName(args[0].ToString());
                    }
                }
            }
        }

        //Converters:
        protected virtual object str_conv(object[] args)
        {
            API.assertNullArguments(args, nullargsfor1notallowing);
            if (args[0].GetType().IsArray && args[0].GetType().GetElementType() == typeof(char)) return new string((char[])args[0]);
            return args[0].ToString();
        }
        protected virtual object sbyte_conv(object[] args)
        {
            API.assertNullArguments(args, nullargsfor1notallowing);
            return sbyte.Parse(args[0].ToString());
        }
        protected virtual object byte_conv(object[] args)
        {
            API.assertNullArguments(args, nullargsfor1notallowing);
            return byte.Parse(args[0].ToString());
        }
        protected virtual object short_conv(object[] args)
        {
            API.assertNullArguments(args, nullargsfor1notallowing);
            return short.Parse(args[0].ToString());
        }
        protected virtual object ushort_conv(object[] args)
        {
            API.assertNullArguments(args, nullargsfor1notallowing);
            return ushort.Parse(args[0].ToString());
        }
        protected virtual object int_conv(object[] args)
        {
            API.assertNullArguments(args, nullargsfor1notallowing);
            return int.Parse(args[0].ToString());
        }
        protected virtual object uint_conv(object[] args)
        {
            API.assertNullArguments(args, nullargsfor1notallowing);
            return uint.Parse(args[0].ToString());
        }
        protected virtual object long_conv(object[] args)
        {
            API.assertNullArguments(args, nullargsfor1notallowing);
            return long.Parse(args[0].ToString());
        }
        protected virtual object ulong_conv(object[] args)
        {
            API.assertNullArguments(args, nullargsfor1notallowing);
            return ulong.Parse(args[0].ToString());
        }
        protected virtual object float_conv(object[] args)
        {
            API.assertNullArguments(args, nullargsfor1notallowing);
            return float.Parse(args[0].ToString());
        }
        protected virtual object double_conv(object[] args)
        {
            API.assertNullArguments(args, nullargsfor1notallowing);
            return double.Parse(args[0].ToString());
        }
        protected virtual object dec_conv(object[] args)
        {
            API.assertNullArguments(args, nullargsfor1notallowing);
            return decimal.Parse(args[0].ToString());
        }
        protected virtual object char_conv(object[] args)
        {
            API.assertNullArguments(args, nullargsfor1notallowing);
            API.assertArguments(args, argsfor1long);
            if ((long)args[0] < -32768 || (long)args[0] > 65535) API.raiseCaptainALMConsoleException("Character Code out of Range", new ArgumentException("Argument 'CharCode' must be within the range of -32768 to 65535."));
            return Convert.ToChar((int)args[0] & 65535);
        }
        protected virtual object conv(object[] args)
        {
            API.assertNullArguments(args, nullargsfor2notallowing);
            API.assertArguments(args, argsfor1object1type);
            System.ComponentModel.TypeConverter conv = System.ComponentModel.TypeDescriptor.GetConverter((Type)args[1]);
            return conv.ConvertFrom(args[0]);
        }
        protected virtual object char_arr_conv(object[] args)
        {
            API.assertNullArguments(args, nullargsfor1notallowing);
            API.assertArguments(args, argsfor1string);
            return ((string)args[0]).ToCharArray();
        }

        //Type:
        protected virtual object get_type(object[] args)
        {
            API.assertNullArguments(args, nullargsfor1notallowing);
            API.assertArguments(args, argsfor1string);
            var toret = Type.GetType((string)args[0], false);
            if (object.ReferenceEquals(toret, null))
            {
                var assm = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var c in assm)
                {
                    toret = c.GetType((string)args[0], false);
                    if (!object.ReferenceEquals(toret, null)) break;
                }
            }
            if (object.ReferenceEquals(toret, null)) API.raiseCaptainALMConsoleException("Type not found.", new TypeLoadException("Type not found."));
            return toret;
        }
        protected virtual object get_type_of(object[] args)
        {
            API.assertNullArguments(args, nullargsfor1notallowing);
            API.assertArguments(args, argsfor1object);
            return args[0].GetType();
        }

        //Caster:
        protected virtual object cast(object[] args)
        {
            API.assertNullArguments(args, nullargsfor2notallowing);
            API.assertArguments(args, argsfor1object1type);
            return Convert.ChangeType(args[0], (Type)args[1]);
        }

        //Help:
        protected virtual object help(object[] args)
        {
            string srch = null;
            if (args.Length > 0)
            {
                API.assertNullArguments(args, nullargsfor1notallowing);
                API.assertArguments(args, argsfor1string);
                srch = (string)args[0];
            }
            var toret = new List<string>();
            var cmds = Registry.getCommands();
            foreach (var c in cmds) if (srch == null || c.help.Contains(srch)) toret.Add(c.help);
            return toret.ToArray();
        }

        //Syntax:
        protected virtual object lang(object[] args)
        {
            if (args.Length > 0)
            {
                API.assertNullArguments(args, nullargsfor1notallowing);
                API.assertArguments(args, argsfor1string);
                API.setSyntax((string)args[0]);
            }
            return API.getCurrentSyntax();
        }
        protected virtual object langs(object[] args)
        {
            return API.getSyntaxes();
        }
        protected virtual object get_lang(object[] args)
        {
            API.assertNullArguments(args, nullargsfor1notallowing);
            API.assertArguments(args, argsfor1string);
            return Registry.getSyntax((string)args[0]);
        }

        //Command:
        protected virtual object commands(object[] args)
        {
            return API.getCommands();
        }
        protected virtual object get_command(object[] args)
        {
            API.assertNullArguments(args, nullargsfor1notallowing);
            API.assertArguments(args, argsfor1string);
            return Registry.getCommand((string)args[0]);
        }

        //Call command:
        protected virtual object call(object[] args)
        {
            API.assertNullArguments(args, nullargsfor2notallowing);
            API.assertArguments(args, argsfor1icommand1objectarr);
            return ((ICommand)args[0]).run((object[])args[1]);
        }
        protected virtual object callstring(object[] args)
        {
            if (args.Length > 1)
            {
                API.assertNullArguments(args, nullargsfor2notallowing);
                API.assertArguments(args, argsfor1string1isyntax);
                return Processor.executeCommand((string)args[0], (ISyntax)args[1]);
            }
            else
            {
                API.assertNullArguments(args, nullargsfor1notallowing);
                API.assertArguments(args, argsfor1string);
                return Processor.executeCommand((string)args[0]);
            }
        }

        //Boolean:
        protected virtual object and(object[] args)
        {
            API.assertNullArguments(args, nullargsfor2notallowing);
            API.assertArguments(args, argsfor2bool);
            return (bool)args[0] & (bool)args[1];
        }
        protected virtual object or(object[] args)
        {
            API.assertNullArguments(args, nullargsfor2notallowing);
            API.assertArguments(args, argsfor2bool);
            return (bool)args[0] | (bool)args[1];
        }
        protected virtual object xor(object[] args)
        {
            API.assertNullArguments(args, nullargsfor2notallowing);
            API.assertArguments(args, argsfor2bool);
            return (bool)args[0] ^ (bool)args[1];
        }
        protected virtual object andalso(object[] args)
        {
            API.assertNullArguments(args, nullargsfor1notallowing1allowing);
            API.assertArguments(args, argsfor1bool1object);
            if (!(bool)args[0]) return false;
            API.assertNullArguments(args, nullargsfor2notallowing);
            API.assertArguments(args, argsfor2bool);
            return (bool)args[1];
        }
        protected virtual object orelse(object[] args)
        {
            API.assertNullArguments(args, nullargsfor1notallowing1allowing);
            API.assertArguments(args, argsfor1bool1object);
            if ((bool)args[0]) return true;
            API.assertNullArguments(args, nullargsfor2notallowing);
            API.assertArguments(args, argsfor2bool);
            return (bool)args[1];
        }
        protected virtual object not(object[] args)
        {
            API.assertNullArguments(args, nullargsfor1notallowing);
            API.assertArguments(args, argsfor1bool);
            return !(bool)args[0];
        }

        //Comparator:
        protected virtual object equals(object[] args)
        {
            API.assertNullArguments(args, nullargsfor2allowing);
            API.assertArguments(args, argsfor2object);
            return object.Equals(args[0], args[1]);
        }
        protected virtual object compare(object[] args)
        {
            API.assertNullArguments(args, nullargsfor2notallowing);
            API.assertArguments(args, argsfor2icomparable);
            return ((IComparable)args[0]).CompareTo(args[1]);
        }
        protected virtual object reference_equals(object[] args)
        {
            API.assertNullArguments(args, nullargsfor2allowing);
            API.assertArguments(args, argsfor2object);
            return object.ReferenceEquals(args[0], args[1]);
        }

        //Collection:
        protected virtual object create_arr(object[] args)
        {
            if (args.Length > 1)
            {
                API.assertNullArguments(args, nullargsfor2notallowing);
                API.assertArguments(args, argsfor1type1int);
                return Array.CreateInstance((Type)args[0], (int)args[1]);
            }
            else if (args.Length == 1)
            {
                API.assertNullArguments(args, nullargsfor1notallowing);
                API.assertArguments(args, argsfor1type);
                return Array.CreateInstance((Type)args[0], 0);
            }
            else
            {
                return new object[0];
            }
        }
        protected virtual object create_lst(object[] args)
        {
            if (args.Length > 1)
            {
                API.assertNullArguments(args, nullargsfor2notallowing);
                API.assertArguments(args, argsfor1type1int);
                var typ = typeof(List<>).MakeGenericType((Type)args[0]);
                var con = typ.GetConstructor(argsfor1int);
                return con.Invoke(new object[] { args[1] });
            }
            else if (args.Length == 1)
            {
                API.assertNullArguments(args, nullargsfor1notallowing);
                API.assertArguments(args, argsfor1type);
                var typ = typeof(List<>).MakeGenericType((Type)args[0]);
                var con = typ.GetConstructor(Type.EmptyTypes);
                return con.Invoke(null);
            }
            else
            {
                return new List<object>();
            }
        }
        protected virtual object create_arr_from(object[] args)
        {
            var toret = new object[args.Length];
            if (args.Length > 0) Array.Copy(args, toret, args.Length);
            return toret;
        }
        protected virtual object create_lst_from(object[] args)
        {
            var toret = new List<object>();
            if (args.Length > 0) toret.AddRange(args);
            return toret;
        }
        protected virtual object create_arr_of_from(object[] args)
        {
            if (args.Length > 0)
            {
                var toret = Array.CreateInstance((Type)args[0], args.Length - 1);
                if (args.Length > 1) Array.Copy(args, 1, toret, 0, args.Length - 1);
                return toret;
            }
            else
            {
                return new object[0];
            }
        }
        protected virtual object create_lst_of_from(object[] args)
        {
            if (args.Length > 0)
            {
                var typ = typeof(List<>).MakeGenericType((Type)args[0]);
                var arr = Array.CreateInstance((Type)args[0], args.Length - 1);
                if (args.Length > 1)
                {
                    Array.Copy(args, 1, arr, 0, args.Length - 1);
                    var con = typ.GetConstructor(new Type[] { typeof(IEnumerable<>).MakeGenericType((Type)args[0]) });
                    return con.Invoke(new object[] { arr });
                }
                else
                {
                    var con = typ.GetConstructor(Type.EmptyTypes);
                    return con.Invoke(null);
                }
            }
            else
            {
                return new List<object>();
            }
        }
        protected virtual object arr_to_lst(object[] args)
        {
            API.assertNullArguments(args, nullargsfor1notallowing);
            API.assertArguments(args, argsfor1object);
            if (!args[0].GetType().IsArray) API.raiseCaptainALMConsoleException("The argument at position 1 should be an array.");
            var typ = typeof(List<>).MakeGenericType(args[0].GetType().GetElementType());
            var con = typ.GetConstructor(new Type[] { typeof(IEnumerable<>).MakeGenericType(args[0].GetType().GetElementType()) });
            return con.Invoke(new object[] { args[0] });
        }
        protected virtual object lst_to_arr(object[] args)
        {
            API.assertNullArguments(args, nullargsfor1notallowing);
            API.assertArguments(args, argsfor1ilist);
            Array toret = null;
            if (args[0].GetType().IsGenericType && args[0].GetType().GetGenericArguments().Length == 1 && typeof(IList<>).MakeGenericType(args[0].GetType().GetGenericArguments()[0]).IsAssignableFrom(args[0].GetType()))
            {
                toret = Array.CreateInstance(args[0].GetType().GetGenericArguments()[0], ((IList)args[0]).Count);
            }
            else
            {
                toret = new object[((IList)args[0]).Count];
            }
            ((IList)args[0]).CopyTo(toret, 0);
            return toret;
        }
        protected virtual object arr_dup(object[] args)
        {
            if (args.Length > 2)
            {
                API.assertNullArguments(args, nullargsfor3notallowing);
                API.assertArguments(args, argsfor1object2int);
            }
            else if (args.Length > 1)
            {
                API.assertNullArguments(args, nullargsfor2notallowing);
                API.assertArguments(args, argsfor2object);
            }
            else
            {
                API.assertNullArguments(args, nullargsfor1notallowing);
                API.assertArguments(args, argsfor1object);
            }
            if (!args[0].GetType().IsArray) API.raiseCaptainALMConsoleException("The argument at position 1 should be an array.");
            Array toret = null;
            if (args.Length > 1)
            {
                if (args[1].GetType().IsArray)
                {
                    if (args[1].GetType().GetElementType() == typeof(int))
                    {
                        toret = Array.CreateInstance(args[0].GetType().GetElementType(), ((Array)args[1]).Length);
                    }
                    else
                    {
                        API.raiseCaptainALMConsoleException("The argument at position 2 should be an array of int .");
                    }
                }
                else
                {
                    if (args[1].GetType() == typeof(int))
                    {
                        toret = Array.CreateInstance(args[0].GetType().GetElementType(), (int)args[1]);
                    }
                    else
                    {
                        API.raiseCaptainALMConsoleException("The argument at position 2 should be an int .");
                    }
                }
            }
            else
            {
                toret = Array.CreateInstance(args[0].GetType().GetElementType(), ((Array)args[0]).Length);
            }
            if (args.Length > 2)
            {
                Array.Copy((Array)args[0], (int)args[2], toret, 0, (int)args[1]);
            }
            else if (args.Length > 1)
            {
                if (args[1].GetType().IsArray)
                {
                    var iarr = (int[])args[1];
                    for (int i = 0; i < toret.Length; i++)
                    {
                        Array.Copy((Array)args[0], iarr[i], toret, i, 1);
                    }
                }
                else
                {
                    Array.Copy((Array)args[0], 0, toret, 0, (int)args[1]);
                }
            }
            else
            {
                Array.Copy((Array)args[0], toret, ((Array)args[0]).Length);
            }
            return toret;
        }
        protected virtual object lst_dup(object[] args)
        {
            if (args.Length > 2)
            {
                API.assertNullArguments(args, nullargsfor3notallowing);
                API.assertArguments(args, argsfor1ilist2int);
            }
            else if (args.Length > 1)
            {
                API.assertNullArguments(args, nullargsfor2notallowing);
                API.assertArguments(args, argsfor1ilist1object);
            }
            else
            {
                API.assertNullArguments(args, nullargsfor1notallowing);
                API.assertArguments(args, argsfor1ilist);
            }
            Array toret = null;
            var toretType = (args[0].GetType().IsGenericType && args[0].GetType().GetGenericArguments().Length == 1 && typeof(IList<>).MakeGenericType(args[0].GetType().GetGenericArguments()[0]).IsAssignableFrom(args[0].GetType())) ? args[0].GetType().GetGenericArguments()[0] : typeof(object);
            Array input = Array.CreateInstance(toretType, ((IList)args[0]).Count);
            ((IList)args[0]).CopyTo(input, 0);
            if (args.Length > 1)
            {
                if (args[1].GetType().IsArray)
                {
                    if (args[1].GetType().GetElementType() == typeof(int))
                    {
                        toret = Array.CreateInstance(toretType, ((Array)args[1]).Length);
                    }
                    else
                    {
                        API.raiseCaptainALMConsoleException("The argument at position 2 should be an array of int .");
                    }
                }
                else
                {
                    if (args[1].GetType() == typeof(int))
                    {
                        toret = Array.CreateInstance(toretType, (int)args[1]);
                    }
                    else
                    {
                        API.raiseCaptainALMConsoleException("The argument at position 2 should be an int .");
                    }
                }
            }
            else
            {
                toret = Array.CreateInstance(toretType, input.Length);
            }
            if (args.Length > 2)
            {
                Array.Copy(input, (int)args[2], toret, 0, (int)args[1]);
            }
            else if (args.Length > 1)
            {
                if (args[1].GetType().IsArray)
                {
                    var iarr = (int[])args[1];
                    for (int i = 0; i < toret.Length; i++)
                    {
                        Array.Copy(input, iarr[i], toret, i, 1);
                    }
                }
                else
                {
                    Array.Copy(input, 0, toret, 0, (int)args[1]);
                }
            }
            else
            {
                Array.Copy(input, toret, input.Length);
            }
            var typ = typeof(List<>).MakeGenericType(toret.GetType().GetElementType());
            var con = typ.GetConstructor(new Type[] { typeof(IEnumerable<>).MakeGenericType(toret.GetType().GetElementType()) });
            return con.Invoke(new object[] { toret });
        }
        protected virtual object col_count(object[] args)
        {
            API.assertNullArguments(args, nullargsfor1notallowing);
            API.assertArguments(args, argsfor1ilist);
            return ((IList)args[0]).Count;
        }
        protected virtual object col_indx_of(object[] args)
        {
            API.assertNullArguments(args, nullargsfor1notallowing1allowing);
            API.assertArguments(args, argsfor1ilist1object);
            var toret = new List<int>();
            for (int i = 0; i < ((IList)args[0]).Count; i++)
            {
                if (object.Equals(args[1], ((IList)args[0])[i])) toret.Add(i);
            }
            return toret.ToArray();
        }
        protected virtual object col_get(object[] args)
        {
            API.assertNullArguments(args, nullargsfor2notallowing);
            API.assertArguments(args, argsfor1ilist1int);
            return ((IList)args[0])[(int)args[1]];
        }
        protected virtual object col_set(object[] args)
        {
            API.assertNullArguments(args, nullargsfor2notallowing1allowing);
            API.assertArguments(args, argsfor1ilist1int1object);
            ((IList)args[0])[(int)args[1]] = args[2];
            return args[2];
        }
        protected virtual object col_ins(object[] args)
        {
            API.assertNullArguments(args, nullargsfor2notallowing1allowing);
            API.assertArguments(args, argsfor1ilist1int1object);
            ((IList)args[0]).Insert((int)args[1], args[2]);
            return args[2];
        }
        protected virtual object col_add(object[] args)
        {
            API.assertNullArguments(args, nullargsfor1notallowing1allowing);
            API.assertArguments(args, argsfor1ilist1object);
            ((IList)args[0]).Add(args[1]);
            return args[1];
        }
        protected virtual object col_remove_first(object[] args)
        {
            API.assertNullArguments(args, nullargsfor1notallowing1allowing);
            API.assertArguments(args, argsfor1ilist1object);
            ((IList)args[0]).Remove(args[1]);
            return args[1];
        }
        protected virtual object col_remove(object[] args)
        {
            API.assertNullArguments(args, nullargsfor2notallowing);
            API.assertArguments(args, argsfor1ilist1int);
            ((IList)args[0]).RemoveAt((int)args[1]);
            return args[1];
        }
        protected virtual object col_clear(object[] args)
        {
            API.assertNullArguments(args, nullargsfor1notallowing);
            API.assertArguments(args, argsfor1ilist);
            ((IList)args[0]).Clear();
            return null;
        }

        //Arithmetic:
        protected virtual object add(object[] args)
        {
            API.assertNullArguments(args, nullargsfor2notallowing);
            if (args[0].GetType() == args[1].GetType())
            {
                if (args[0].GetType() == typeof(long))
                {
                    return (long)((long)args[0] + (long)args[1]);
                }
                else if (args[0].GetType() == typeof(ulong))
                {
                    return (ulong)((ulong)args[0] + (ulong)args[1]);
                }
                else if (args[0].GetType() == typeof(int))
                {
                    return (int)((int)args[0] + (int)args[1]);
                }
                else if (args[0].GetType() == typeof(uint))
                {
                    return (uint)((uint)args[0] + (uint)args[1]);
                }
                else if (args[0].GetType() == typeof(short))
                {
                    return (short)((short)args[0] + (short)args[1]);
                }
                else if (args[0].GetType() == typeof(ushort))
                {
                    return (ushort)((ushort)args[0] + (ushort)args[1]);
                }
                else if (args[0].GetType() == typeof(float))
                {
                    return (float)((float)args[0] + (float)args[1]);
                }
                else if (args[0].GetType() == typeof(double))
                {
                    return (double)((double)args[0] + (double)args[1]);
                }
                else if (args[0].GetType() == typeof(decimal))
                {
                    return (decimal)((decimal)args[0] + (decimal)args[1]);
                }
                else if (args[0].GetType() == typeof(sbyte))
                {
                    return (sbyte)((sbyte)args[0] + (sbyte)args[1]);
                }
                else if (args[0].GetType() == typeof(byte))
                {
                    return (byte)((byte)args[0] + (byte)args[1]);
                }
            }
            API.raiseCaptainALMConsoleException("The arguments should be Numeric and the same type.");
            return null;
        }
        protected virtual object sub(object[] args)
        {
            API.assertNullArguments(args, nullargsfor2notallowing);
            if (args[0].GetType() == args[1].GetType())
            {
                if (args[0].GetType() == typeof(long))
                {
                    return (long)((long)args[0] - (long)args[1]);
                }
                else if (args[0].GetType() == typeof(ulong))
                {
                    return (ulong)((ulong)args[0] - (ulong)args[1]);
                }
                else if (args[0].GetType() == typeof(int))
                {
                    return (int)((int)args[0] - (int)args[1]);
                }
                else if (args[0].GetType() == typeof(uint))
                {
                    return (uint)((uint)args[0] - (uint)args[1]);
                }
                else if (args[0].GetType() == typeof(short))
                {
                    return (short)((short)args[0] - (short)args[1]);
                }
                else if (args[0].GetType() == typeof(ushort))
                {
                    return (ushort)((ushort)args[0] - (ushort)args[1]);
                }
                else if (args[0].GetType() == typeof(float))
                {
                    return (float)((float)args[0] - (float)args[1]);
                }
                else if (args[0].GetType() == typeof(double))
                {
                    return (double)((double)args[0] - (double)args[1]);
                }
                else if (args[0].GetType() == typeof(decimal))
                {
                    return (decimal)((decimal)args[0] - (decimal)args[1]);
                }
                else if (args[0].GetType() == typeof(sbyte))
                {
                    return (sbyte)((sbyte)args[0] - (sbyte)args[1]);
                }
                else if (args[0].GetType() == typeof(byte))
                {
                    return (byte)((byte)args[0] - (byte)args[1]);
                }
            }
            API.raiseCaptainALMConsoleException("The arguments should be Numeric and the same type.");
            return null;
        }
        protected virtual object mlt(object[] args)
        {
            API.assertNullArguments(args, nullargsfor2notallowing);
            if (args[0].GetType() == args[1].GetType())
            {
                if (args[0].GetType() == typeof(long))
                {
                    return (long)((long)args[0] * (long)args[1]);
                }
                else if (args[0].GetType() == typeof(ulong))
                {
                    return (ulong)((ulong)args[0] * (ulong)args[1]);
                }
                else if (args[0].GetType() == typeof(int))
                {
                    return (int)((int)args[0] * (int)args[1]);
                }
                else if (args[0].GetType() == typeof(uint))
                {
                    return (uint)((uint)args[0] * (uint)args[1]);
                }
                else if (args[0].GetType() == typeof(short))
                {
                    return (short)((short)args[0] * (short)args[1]);
                }
                else if (args[0].GetType() == typeof(ushort))
                {
                    return (ushort)((ushort)args[0] * (ushort)args[1]);
                }
                else if (args[0].GetType() == typeof(float))
                {
                    return (float)((float)args[0] * (float)args[1]);
                }
                else if (args[0].GetType() == typeof(double))
                {
                    return (double)((double)args[0] * (double)args[1]);
                }
                else if (args[0].GetType() == typeof(decimal))
                {
                    return (decimal)((decimal)args[0] * (decimal)args[1]);
                }
                else if (args[0].GetType() == typeof(sbyte))
                {
                    return (sbyte)((sbyte)args[0] * (sbyte)args[1]);
                }
                else if (args[0].GetType() == typeof(byte))
                {
                    return (byte)((byte)args[0] * (byte)args[1]);
                }
            }
            API.raiseCaptainALMConsoleException("The arguments should be Numeric and the same type.");
            return null;
        }
        protected virtual object div(object[] args)
        {
            API.assertNullArguments(args, nullargsfor2notallowing);
            if (args[0].GetType() == args[1].GetType())
            {
                if (args[0].GetType() == typeof(long))
                {
                    return (long)((long)args[0] / (long)args[1]);
                }
                else if (args[0].GetType() == typeof(ulong))
                {
                    return (ulong)((ulong)args[0] / (ulong)args[1]);
                }
                else if (args[0].GetType() == typeof(int))
                {
                    return (int)((int)args[0] / (int)args[1]);
                }
                else if (args[0].GetType() == typeof(uint))
                {
                    return (uint)((uint)args[0] / (uint)args[1]);
                }
                else if (args[0].GetType() == typeof(short))
                {
                    return (short)((short)args[0] / (short)args[1]);
                }
                else if (args[0].GetType() == typeof(ushort))
                {
                    return (ushort)((ushort)args[0] / (ushort)args[1]);
                }
                else if (args[0].GetType() == typeof(float))
                {
                    return (float)((float)args[0] / (float)args[1]);
                }
                else if (args[0].GetType() == typeof(double))
                {
                    return (double)((double)args[0] / (double)args[1]);
                }
                else if (args[0].GetType() == typeof(decimal))
                {
                    return (decimal)((decimal)args[0] / (decimal)args[1]);
                }
                else if (args[0].GetType() == typeof(sbyte))
                {
                    return (sbyte)((sbyte)args[0] / (sbyte)args[1]);
                }
                else if (args[0].GetType() == typeof(byte))
                {
                    return (byte)((byte)args[0] / (byte)args[1]);
                }
            }
            API.raiseCaptainALMConsoleException("The arguments should be Numeric and the same type.");
            return null;
        }

        //Formatters:
        protected virtual object style(object[] args)
        {
            if (args.Length > 0)
            {
                if (object.ReferenceEquals(args[0], null)) API.raiseCaptainALMConsoleException("The argument at position 1 is null and it should be not null .");
                var toret = new StylableString(args[0].ToString());
                if (!object.ReferenceEquals(args[1], null) && args[1].GetType() == typeof(Color)) toret.forecolor = (Color)args[1]; else API.raiseCaptainALMConsoleException("The argument at position 2 should be System.Drawing.Color .");
                if (!object.ReferenceEquals(args[2], null) && args[2].GetType() == typeof(Color)) toret.backcolor = (Color)args[2]; else API.raiseCaptainALMConsoleException("The argument at position 3 should be System.Drawing.Color .");
                if (!object.ReferenceEquals(args[3], null) && args[3].GetType() == typeof(bool)) toret.bold = (bool)args[3]; else API.raiseCaptainALMConsoleException("The argument at position 4 should be System.Boolean .");
                if (!object.ReferenceEquals(args[4], null) && args[4].GetType() == typeof(bool)) toret.italic = (bool)args[4]; else API.raiseCaptainALMConsoleException("The argument at position 5 should be System.Boolean .");
                if (!object.ReferenceEquals(args[5], null) && args[5].GetType() == typeof(bool)) toret.underline = (bool)args[5]; else API.raiseCaptainALMConsoleException("The argument at position 6 should be System.Boolean .");
                if (!object.ReferenceEquals(args[6], null) && args[6].GetType() == typeof(bool)) toret.strikeout = (bool)args[6]; else API.raiseCaptainALMConsoleException("The argument at position 7 should be System.Boolean .");
                if (!object.ReferenceEquals(args[7], null)) toret.fontname = args[7].ToString(); else API.raiseCaptainALMConsoleException("The argument at position 8 should be System.String .");
                return toret;
            }
            else
            {
                API.assertNullArguments(args, nullargsfor1notallowing);
                return null;
            }
        }
        protected virtual object str_concat(object[] args)
        {
            if (args.Length == 0) API.raiseCaptainALMConsoleException("There are no arguments.");
            var toret = new StylableString();
            if (object.ReferenceEquals(args[0], null)) API.raiseCaptainALMConsoleException("The argument at position 1 is null and it should be not null .");
            if (args[0].GetType() == typeof(StylableString)) toret = (StylableString)args[0]; else toret = new StylableString(args[0].ToString());
            if (args.Length > 1)
            {
                for (int i = 1; i < args.Length; i++)
                {
                    if (object.ReferenceEquals(args[i], null)) API.raiseCaptainALMConsoleException("The argument at position " + (i + 1).ToString() + " is null and it should be not null .");
                    toret.text += args[i].ToString();
                }
            }
            if (args[0].GetType() == typeof(StylableString))
            {
                return toret;
            }
            else
            {
                return toret.text;
            }
        }
        protected virtual object str_arr_combine(object[] args)
        {
            if (args.Length > 1)
            {
                API.assertNullArguments(args, nullargsfor2notallowing);
                API.assertArguments(args, argsfor1stringarr1string);
                return string.Join(args[1].ToString(), (string[])args[0]);
            }
            else
            {
                API.assertNullArguments(args, nullargsfor1notallowing);
                API.assertArguments(args, argsfor1stringarr);
                return string.Concat((string[])args[0]);
            }
        }
        protected virtual object str_split(object[] args)
        {
            API.assertNullArguments(args, nullargsfor2notallowing);
            API.assertArguments(args, argsfor2string);
            return args[0].ToString().Split(args[1].ToString().ToCharArray());
        }

        /// <summary>
        /// Gets the array of commands that makes up the standard library
        /// </summary>
        /// <returns>An array of commands</returns>
        public virtual Command[] getLibrary()
        {
            var toret = new List<Command>();

            toret.Add(new Command(input_req, "", "input", "input > System.String"));

            toret.Add(new Command(echo, "", "echo", "echo %output:System.Object% > System.Object"));
            toret.Add(new Command(write, "", "write", "write (output:String) > String"));
            toret.Add(new Command(writeline, "", "writeline", "writeline (output:String) > String"));

            toret.Add(new Command(invalid, "", API.invalidCommandName, API.invalidCommandName + " > System.String"));

            toret.Add(new Command(get_str_var, "", "get_str_var", "get_str_var %name:String% > System.String"));
            toret.Add(new Command(set_str_var, "", "set_str_var", "set_str_var %name:String% %value:String% > String"));
            toret.Add(new Command(get_var, "", "get_var", "get_var %name:String% > *"));
            toret.Add(new Command(set_var, "", "set_var", "set_var %name:String% %value:*% > *"));

            toret.Add(new Command(get_null, "", "null", "null > null"));
            toret.Add(new Command(create_object, "", "object", "object > System.Object"));
            toret.Add(new Command(create_intptr, "", "intptr", "intptr (value:Intergral) > System.IntPtr"));
            toret.Add(new Command(create_uintptr, "", "uintptr", "uintptr (value:UIntergral) > System.UIntPtr"));
            toret.Add(new Command(random, "", "random", "random (maximum:System.Int32) (minimum:System.Int32) > System.Int32"));
            toret.Add(new Command(create_color, "", "create_color", "create_color %name or hexdecimal #RRGGBB #AARRGGBB:String|integer value:System.Int32|integer array of rgb or rgba:System.Int32[]|Red value:System.Int32% (Green value:System.Int32) (Blue value:System.Int32) (Alpha value:System.Int32) > System.Drawing.Color"));

            toret.Add(new Command(str_conv, "", "to_str", "to_str %input:System.Object% > System.String"));
            toret.Add(new Command(sbyte_conv, "", "to_sbyte", "to_sbyte %input:System.Object% > System.SByte"));
            toret.Add(new Command(byte_conv, "", "to_byte", "to_byte %input:System.Object% > System.Byte"));
            toret.Add(new Command(short_conv, "", "to_short", "to_short %input:System.Object% > System.Int16"));
            toret.Add(new Command(ushort_conv, "", "to_ushort", "to_ushort %input:System.Object% > System.UInt16"));
            toret.Add(new Command(int_conv, "", "to_int", "to_int %input:System.Object% > System.Int32"));
            toret.Add(new Command(uint_conv, "", "to_uint", "to_uint %input:System.Object% > System.UInt32"));
            toret.Add(new Command(long_conv, "", "to_long", "to_long %input:System.Object% > System.Int64"));
            toret.Add(new Command(ulong_conv, "", "to_ulong", "to_ulong %input:System.Object% > System.UInt64"));
            toret.Add(new Command(float_conv, "", "to_float", "to_float %input:System.Object% > System.Single"));
            toret.Add(new Command(double_conv, "", "to_double", "to_double %input:System.Object% > System.Double"));
            toret.Add(new Command(dec_conv, "", "to_dec", "to_dec %input:System.Object% > System.Decimal"));
            toret.Add(new Command(char_conv, "", "to_char", "to_char %input:System.Object% > System.Char"));
            toret.Add(new Command(conv, "", "convert", "convert %input:System.Object% %target type:System.Type% > target type"));
            toret.Add(new Command(char_arr_conv, "", "to_char_array", "to_char_array %input:String% > System.Char[]"));

            toret.Add(new Command(get_type, "", "get_type", "get_type %type name:String% > System.Type"));
            toret.Add(new Command(get_type_of, "", "type_of", "type_of %input:System.Object% > System.Type"));

            toret.Add(new Command(cast, "", "cast", "cast %input:System.Object% %target type:System.Type% > target type"));

            toret.Add(new Command(help, "", "help", "help (search constraint:String) > System.String[]"));

            toret.Add(new Command(lang, "", "syntax", "syntax (syntax name:String) > System.String"));
            toret.Add(new Command(langs, "", "syntaxes", "syntaxes > System.String[]"));
            toret.Add(new Command(get_lang, "", "get_syntax", "get_syntax %syntax name:String% > captainalm.calmcmd.ISyntax"));

            toret.Add(new Command(commands, "", "commands", "commands > System.String[]"));
            toret.Add(new Command(get_command, "", "get_command", "get_command %command name:String% > captainalm.calmcmd.ICommand"));

            toret.Add(new Command(call, "", "call", "call %command in:captainalm.calmcmd.ICommand% %arguments in:System.Object[]% > *"));
            toret.Add(new Command(callstring, "", "call_string", "call_string %command in:String% (syntax in:captainalm.calmcmd.ISyntax) > *"));

            toret.Add(new Command(and, "", "and", "and %input 1:System.Boolean% %input 2:System.Boolean% > System.Boolean"));
            toret.Add(new Command(or, "", "or", "or %input 1:System.Boolean% %input 2:System.Boolean% > System.Boolean"));
            toret.Add(new Command(xor, "", "xor", "xor %input 1:System.Boolean% %input 2:System.Boolean% > System.Boolean"));
            toret.Add(new Command(andalso, "", "andalso", "andalso %input 1:System.Boolean% %input 2:System.Boolean% > System.Boolean"));
            toret.Add(new Command(orelse, "", "orelse", "orelse %input 1:System.Boolean% %input 2:System.Boolean% > System.Boolean"));
            toret.Add(new Command(not, "", "not", "not %input:System.Boolean% > System.Boolean"));

            toret.Add(new Command(equals, "", "equals", "equals %input 1:*% %input 2:*% > System.Boolean"));
            toret.Add(new Command(compare, "", "compare", "compare %input 1:*% %input 2:*% > System.Int32"));
            toret.Add(new Command(reference_equals, "", "reference_equals", "reference_equals %input 1:*% %input 2:*% > System.Boolean"));

            toret.Add(new Command(create_arr, "", "create_array", "create_array (element type:System.Type) (length:System.Int32) > element type[]"));
            toret.Add(new Command(create_lst, "", "create_list", "create_list (element type:System.Type) (capacity:System.Int32) > System.Collections.Generic.List<element type>"));
            toret.Add(new Command(create_arr_from, "", "create_array_from", "create_array_from (input x:*)... > System.Object[]"));
            toret.Add(new Command(create_lst_from, "", "create_list_from", "create_list_from (input x:*)... > System.Collections.Generic.List<System.Object>"));
            toret.Add(new Command(create_arr_of_from, "", "create_array_of_from", "create_array_of_from (element type:System.Type) (input x:*)... > element type[]"));
            toret.Add(new Command(create_lst_of_from, "", "create_list_of_from", "create_list_of_from (element type:System.Type) (input x:*)... > System.Collections.Generic.List<element type>"));
            toret.Add(new Command(arr_to_lst, "", "array_to_list", "array_to_list %array in:*[]% > System.Collections.Generic.List<*>"));
            toret.Add(new Command(lst_to_arr, "", "list_to_array", "list_to_array %list in:System.Collections.Generic.List<*>% > *[]"));
            toret.Add(new Command(arr_dup, "", "copy_array", "copy_array %array in:*[]% (indexes to copy:System.Int32[]|length of copy:System.Int32) (copy from index:System.Int32) > *[]"));
            toret.Add(new Command(lst_dup, "", "copy_list", "copy_list %list in:System.Collections.Generic.List<*>% (indexes to copy:System.Int32[]|length of copy:System.Int32) (copy from index:System.Int32) > System.Collections.Generic.List<*>"));
            toret.Add(new Command(col_count, "", "item_count", "item_count %collection in:System.Collections.IList% > System.Int32"));
            toret.Add(new Command(col_indx_of, "", "item_index", "item_index %collection in:System.Collections.IList% %item to find:*% > System.Int32[]"));
            toret.Add(new Command(col_get, "", "item_get", "item_get %collection in:System.Collections.IList% %item index:System.Int32% > *"));
            toret.Add(new Command(col_set, "", "item_set", "item_set %collection in:System.Collections.IList% %item index:System.Int32% %item value:*% > *"));
            toret.Add(new Command(col_ins, "", "item_insert", "item_insert %collection in:System.Collections.IList% %item index:System.Int32% %item value:*% > *"));
            toret.Add(new Command(col_add, "", "item_add", "item_add %collection in:System.Collections.IList% %item value:*% > *"));
            toret.Add(new Command(col_remove_first, "", "item_remove", "item_remove %collection in:System.Collections.IList% %item value:*% > *"));
            toret.Add(new Command(col_remove, "", "item_remove_at", "item_remove_at %collection in:System.Collections.IList% %item index:System.Int32% > System.Int32"));
            toret.Add(new Command(col_clear, "", "item_clear", "item_clear %collection in:System.Collections.IList% > null"));

            toret.Add(new Command(add, "", "add", "add %input 1:Number% %input 2:Number% > Number"));
            toret.Add(new Command(sub, "", "minus", "minus %input 1:Number% %input 2:Number% > Number"));
            toret.Add(new Command(mlt, "", "multiply", "multiply %input 1:Number% %input 2:Number% > Number"));
            toret.Add(new Command(div, "", "divide", "divide %input 1:Number% %input 2:Number% > Number"));

            toret.Add(new Command(style, "", "style_string", "style_string %string in:String% (forecolor in:System.Drawing.Color) (backcolor in:System.Drawing.Color) (bold in:System.Boolean) (italic in:System.Boolean) (underline in:System.Boolean) (strikeout in:System.Boolean) (font name:String) > captainalm.calmcmd.StylableString"));
            toret.Add(new Command(str_concat, "", "concat_string", "concat_string %string 1:String% (string x:String)... > String"));
            toret.Add(new Command(str_arr_combine, "", "combine_string_array", "combine_string_array %array in:System.String[]% (deliminator in:String) > System.String"));
            toret.Add(new Command(str_split, "", "split_string", "split_string %input string:String% %deliminator:String% > System.String[]"));
            return toret.ToArray();
        }
    }
}
