using System;

namespace captainalm.calmcmd
{
    /// <summary>
    /// Exception for the Captain ALM Console system.
    /// </summary>
    [Serializable]
    public class CaptainALMConsoleException : Exception
    {
        /// <summary>
        /// Constructs a new CaptainALMConsoleException
        /// </summary>
        public CaptainALMConsoleException() { }
        /// <summary>
        /// Constructs a new CaptainALMConsoleException with the specified message
        /// </summary>
        /// <param name="message">The message to use</param>
        public CaptainALMConsoleException(string message) : base(message) { }
        /// <summary>
        /// Constructs a new CaptainALMConsoleException with the specified message and inner exception
        /// </summary>
        /// <param name="message">The message to use</param>
        /// <param name="inner">The inner exception to use</param>
        public CaptainALMConsoleException(string message, Exception inner) : base(message, inner) { }
        protected CaptainALMConsoleException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
