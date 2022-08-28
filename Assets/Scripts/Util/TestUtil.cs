using UnityEngine;

namespace Util
{
    public static class TestUtil
    {
        private static readonly Logger logger = new(Debug.unityLogger.logHandler);

        private static float timeI = float.MinValue;
        public static void log(string text)
        {
            logger.Log(text);
        }

        /// <summary>
        /// Will log text only if the call is a set interval after intialization or the last call to this method. 
        /// Good for using in Update() methods while testing if you do not want to print every frame.
        /// </summary>
        /// <param name="text">Text to log</param>
        /// <param name="interval">Interval in seconds to allow a log to be created</param>
        public static void timedLog(string text, float interval = 1f)
        {
            if (Time.time - timeI >= interval)
            {
                timeI = Time.time;
                log(text);
            }
        }
    }
}
