using UnityEngine;
using System.IO;

namespace Pretia.RelocChecker.Utils
{
    public class LogManager : MonoBehaviour
    {
        
        public const string USE_DEBUG = "SFMRelocDebugMode";
        
        private string _logFilePath;
        private string _dateString;
        private string _timeString;

        private void Start()
        {
            // Set the path for the log file in the persistent data path
            InitDirectories();

            // Call this method to redirect Unity logs to our custom log file
            Application.logMessageReceived += LogCallback;
        }

        private void LogCallback(string condition, string stackTrace, LogType type)
        {
            if (PlayerPrefs.GetInt(USE_DEBUG, 0) == 0)
            {
                return;
            }
            
            // Log the message to the file
            string logMessage = $"{type}: {condition}\n{stackTrace}\n\n";
            File.AppendAllText(_logFilePath, logMessage);
        }

        private void OnDestroy()
        {
            // Call this method to stop logging and remove the callback
            Application.logMessageReceived -= LogCallback;
        }

        public void InitDirectories()
        {
            DirectoryInfo di = Directory.CreateDirectory(Application.persistentDataPath);
            _logFilePath = Path.Combine(Application.persistentDataPath, TimeString() + "-UnityLog.txt");
        }

        private string TimeString()
        {
            System.DateTime theTime = System.DateTime.Now;
            _dateString = string.Format("{0:0000}{1:00}{2:00}", theTime.Year, theTime.Month, theTime.Day);
            _timeString = string.Format("{0:00}{1:00}{2:00}", theTime.Hour, theTime.Minute, theTime.Second);

            return _dateString + "_" + _timeString;
        }
    }
}