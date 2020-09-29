namespace goOfflineE.Helpers
{
    using System;
    using System.Globalization;

    // custom exception class for throwing application specific exceptions 
    // that can be caught and handled within the application
    /// <summary>
    /// Defines the <see cref="AppException" />.
    /// </summary>
    public class AppException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppException"/> class.
        /// </summary>
        public AppException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppException"/> class.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        public AppException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppException"/> class.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="args">The args<see cref="object[]"/>.</param>
        public AppException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
