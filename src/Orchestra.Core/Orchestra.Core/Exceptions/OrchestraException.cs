// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrchestraException.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System;

    /// <summary>
    /// Custom exception in Orchestra.
    /// </summary>
    public class OrchestraException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrchestraException" /> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public OrchestraException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrchestraException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public OrchestraException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}