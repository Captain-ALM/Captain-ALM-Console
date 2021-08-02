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
        /// <summary>
        /// Constructs a new CaptainALMConsoleException from serialized data using the serialization info and streaming context passed
        /// </summary>
        /// <param name="info">The serialization info</param>
        /// <param name="context">The streaming context</param>
        /// <exception cref="System.ArgumentNullException">The info parameter is null</exception>
        /// <exception cref="System.Runtime.Serialization.SerializationException">The class name is null or System.Exception.HResult is zero (0)</exception>
        protected CaptainALMConsoleException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
