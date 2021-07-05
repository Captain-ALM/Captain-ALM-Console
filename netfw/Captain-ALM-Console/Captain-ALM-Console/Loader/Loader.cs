using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace captainalm.calmcmd
{
    /// <summary>
    /// Defines the assembly loader class for the console system.
    /// </summary>
    public static class Loader
    {
        /// <summary>
        /// Loads an assembly for extension registration
        /// </summary>
        /// <param name="assemblyIn">The assembly to load</param>
        /// <returns>Whether all type loading succeeded</returns>
        public static bool loadAssembly(Assembly assemblyIn)
        {
            if (object.ReferenceEquals(assemblyIn, null)) return false;
            var toret = true;
            foreach (Type c in assemblyIn.GetTypes())
            {
                if (c.IsPublic & !c.IsAbstract & !c.IsInterface)
                {
                    toret &= typeProcessor(c);
                }
            }
            return toret;
        }

        private static bool typeProcessor(Type typeIn)
        {
            MethodInfo initMethod = null;
            bool isInstanceMethod = false;
            try
            {
                foreach (MethodInfo c in typeIn.GetMethods(BindingFlags.Public | BindingFlags.Static))
                {
                    ExtensionSetupMethodAttribute[] aarr = (ExtensionSetupMethodAttribute[])c.GetCustomAttributes(typeof(ExtensionSetupMethodAttribute), false);
                    if (aarr.Length != 0) { initMethod = c; isInstanceMethod = false; }
                }
                foreach (MethodInfo c in typeIn.GetMethods(BindingFlags.Public | BindingFlags.Instance))
                {
                    ExtensionSetupMethodAttribute[] aarr = (ExtensionSetupMethodAttribute[])c.GetCustomAttributes(typeof(ExtensionSetupMethodAttribute), false);
                    if (aarr.Length != 0) { initMethod = c; isInstanceMethod = true; }
                }
            }
            catch (ThreadAbortException ex) { throw ex; }
            catch (Exception ex) { return false; }
            if (initMethod != null)
            {
                try
                {
                    object instance = null;
                    if (isInstanceMethod) instance = typeIn.Assembly.CreateInstance(typeIn.FullName);
                    initMethod.Invoke(instance, null);
                }
                catch (ThreadAbortException ex) { throw ex; }
                catch (Exception ex) { return false; }
            }
            return true;
        }
    }
}
