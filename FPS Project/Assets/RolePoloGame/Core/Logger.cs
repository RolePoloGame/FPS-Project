using RolePoloGame.Core.Extensions;
using UnityEngine;

namespace RolePoloGame.Core
{
    /// <summary>
    /// Wrapper for default Unity Debug Console. Allows for expansion with additional types of Log, and custom writing log to a file.
    /// </summary>
    public class Logger
    {
        #region Fields

        private bool IsEnabled { get; set; }
        private bool DisplayName { get; set; }
        private bool DisplayInConsole { get; set; }
        private Object Owner { get; set; }

        #endregion

        #region Constructors

        public Logger() : this(null, true, true, true)
        {
        }

        public Logger(Object owner, bool isEnabled) : this(owner, isEnabled, true, true)
        {
        }

        public Logger(Object owner, bool isEnabled = true, bool displayName = true,
            bool displayInConsole = true)
        {
            Owner = owner;
            IsEnabled = isEnabled;
            DisplayName = displayName;
            DisplayInConsole = displayInConsole;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Logs a message to the Unity Console.
        /// </summary>
        /// <param name="message">String or object to be converted to string representation for display.</param>
       
        public void Log(object message)
        {
            message = ValidateMessage(message, out bool displayInConsole);
            if (displayInConsole)
                WriteLog(message);
        }

        /// <summary>
        /// Logs a warning message to the Unity Console.
        /// </summary>
        /// <param name="message">String or object to be converted to string representation for display.</param>
       
        public void LogWarning(object message)
        {
            message = ValidateMessage(message, out bool displayInConsole);
            if (displayInConsole)
                WriteWarning(message);
        }

        /// <summary>
        /// Logs an error message to the Unity Console.
        /// </summary>
        /// <param name="message">String or object to be converted to string representation for display.</param>
       
        public void LogError(object message)
        {
            message = ValidateMessage(message, out bool displayInConsole);
            if (displayInConsole)
                WriteError(message);
        }

       
        public void LogSoftError(object message)
        {
            message = ValidateMessage(message, out bool displayInConsole);
            if (displayInConsole)
                WriteLog(message.ToString().Color(Color.red));
        }

        #endregion


        #region Private methods

       
        private void WriteLog(object message) => Debug.Log(message);

       
        private void WriteWarning(object message) => Debug.LogWarning(message);

       
        private void WriteError(object message) => Debug.LogError(message);

       
        private object ValidateMessage(object message, out bool displayInConsole)
        {
            displayInConsole = false;
            if (!IsEnabled) return message;
            if (DisplayName)
            {
                if (Owner != null)
                    message = $"[{Owner.name.Bold()}] {message}";
            }

            if (!DisplayInConsole) return message;
            displayInConsole = true;
            return message;
        }

        #endregion
    }
}