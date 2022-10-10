using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;

namespace FeatherLight.Pro.Console
{
    public class ConsoleOutputItem : MonoBehaviour
    {
        [Header("Graphics")]

        [SerializeField]
        private string dateTextTemplate = "{hours}<size=80%>:{minutes}:{seconds}</size>";

        [Space]

        [SerializeField]
        private Color logColor = Color.white;

        [SerializeField]
        private Color successColor = Color.green;

        [SerializeField]
        private Color warningColor = Color.yellow;

        [SerializeField]
        private Color errorColor = Color.red;

        [Header("References")]

        [SerializeField]
        private TMP_Text dateText;

        [SerializeField]
        private TMP_Text contentText;

        [SerializeField]
        private TMP_Text logCountText;

        [Header("Debugging")]

        [SerializeField]
        private int currentCount;

        [SerializeField]
        private LogType logType;

        [SerializeField]
        private string stackTrace;

        [SerializeField]
        private object currentLog;

        [SerializeField]
        private DateTime currentDate;

        public void Init(string content, DateTime date, int count, LogType type, string stackTrace, bool autoUpdate = true)
        {
            currentLog = content;
            currentDate = date;
            currentCount = count;
            logType = type;
            this.stackTrace = stackTrace;

            if (autoUpdate)
            {
                UpdateItem();
            }
        }

        private Color GetColorFromLog(LogType type)
        {
            switch(type)
            {
                case LogType.Log:
                    return logColor;
                case LogType.Success:
                    return successColor;
                case LogType.Warning:
                    return warningColor;
                case LogType.Error:
                    return errorColor;
                default:
                    return logColor;
            }
        }
        private string GetDateString(DateTime time)
        {
            string hour = time.Hour.ToString();

            if (time.Hour < 10)
            {
                hour = hour.Insert(0, "0");
            }

            string minute = time.Minute.ToString();

            if (time.Minute < 10)
            {
                minute = minute.Insert(0, "0");
            }

            string second = time.Second.ToString();

            if (time.Second < 10)
            {
                second = second.Insert(0, "0");
            }

            return dateTextTemplate.Replace("{hours}", hour).Replace("{minutes}", minute).Replace("{seconds}", second);
        }
        private string GetLogCount(int count)
        {
            string output = count.ToString();

            if (count > 99)
            {
                output = "99+";
            }

            return output;
        }

        public void UpdateItem()
        {
            contentText.text = currentLog.ToString();
            contentText.color = GetColorFromLog(logType);
            dateText.text = GetDateString(currentDate);
            logCountText.text = GetLogCount(currentCount);
        }

        public string Get()
        {
            return currentLog.ToString();
        }

        public void Increase(bool autoUpdate = true)
        {
            currentCount++;

            if (autoUpdate)
            {
                UpdateItem();
            }
        }
    }
}