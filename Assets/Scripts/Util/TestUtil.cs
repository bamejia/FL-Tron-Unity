using UnityEngine;

namespace Util
{
    public static class TestUtil
    {
        //TODO To modify logger in future perhaps otherwise can delete
        //private static readonly Logger logger = new(Debug.unityLogger.logHandler);

        private static float timeI = float.MinValue;

        public static void Log(string text)
        {
            Debug.Log(text);
        }
        public static void Log(string format, params System.Object[] formatArgs)
        {
            Debug.Log(string.Format(format, formatArgs));
        }

        /// <summary>
        /// Will log text only if the call is a set interval after intialization or the last call to this method. 
        /// Good for using in Update() methods while testing if you do not want to print every frame.
        /// </summary>
        /// <param name="text">Text to log</param>
        /// <param name="interval">Interval in seconds to allow a log to be created</param>
        public static void TimedLog(string text, float interval = 1f)
        {
            if (Time.time - timeI >= interval)
            {
                timeI = Time.time;
                Log(text);
            }
        }
        /// <summary>
        /// Will log text only if the call is a set interval after intialization or the last call to this method. 
        /// Good for using in Update() methods while testing if you do not want to print every frame.
        /// </summary>
        /// <param name="format">Text to log</param>
        /// <param name="interval">Interval in seconds to allow a log to be created</param>
        /// <param name="formatterArgs">Arguments to replace in the format string</param>
        public static void TimedLog(string format, float interval = 1f, params Object[] formatterArgs)
        {
            if (Time.time - timeI >= interval)
            {
                timeI = Time.time;
                Log(format, formatterArgs);
            }
        }
    }
}
