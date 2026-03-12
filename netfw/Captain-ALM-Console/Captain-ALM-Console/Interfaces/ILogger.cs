using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace captainalm.calmcmd
{
    /// <summary>
    /// Provides a logging access interface for command output.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Gets the current log.
        /// </summary>
        /// <returns></returns>
        string getLog();
        /// <summary>
        /// Writes a log entry.
        /// </summary>
        /// <param name="str">The stylable log entry</param>
        void writeEntry(StylableString str);
    }
}
