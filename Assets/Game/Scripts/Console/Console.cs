using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FeatherLight.Pro.Console
{
    public class Console : MonoBehaviour
    {
        public static Console instance;

        [SerializeField]
        private ConsoleOutputItem outputItemPrefab;

        [SerializeField]
        private RectTransform outputItemContainer;

        [SerializeField]
        private CanvasGroup consoleCanvasGroup;

        [SerializeField]
        private List<ConsoleOutputItem> outputItems = new List<ConsoleOutputItem>();

        private bool isOpened;

        private void Awake()
        {
            instance = this;
        }
        private void Start()
        {
            Success("Console > Console successfuly intialized.");
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                isOpened = !isOpened;

                UpdateConsoleState();
            }
        }

        private void UpdateConsoleState()
        {
            consoleCanvasGroup.alpha = (isOpened) ? 1 : 0;
            consoleCanvasGroup.interactable = isOpened;
            consoleCanvasGroup.blocksRaycasts = isOpened;
        }

        public void Send(object log)
        {
            CreateOutputItem(log.ToString(), DateTime.Now, LogType.Log);
        }
        public void Send(object log, LogType type)
        {
            switch(type)
            {
                case LogType.Success:
                    LogSuccess(log);
                    break;
                case LogType.Error:
                    LogError(log);
                    break;
                case LogType.Warning:
                    LogWarning(log);
                    break;
                case LogType.Log:
                    Log(log);
                    break;
                default:
                    Log(log);
                    break;
            }
        }

        public void Error(object log)
        {
            CreateOutputItem(log.ToString(), DateTime.Now, LogType.Error);
            Debug.LogError(log);
        }
        public void Warning(object log)
        {
            CreateOutputItem(log.ToString(), DateTime.Now, LogType.Warning);
            Debug.LogWarning(log);
        }
        public void Success(object log)
        {
            CreateOutputItem(log.ToString(), DateTime.Now, LogType.Success);
            Debug.Log(log);
        }

        private void CreateOutputItem(string content, DateTime date, LogType type, string stackTrace = "", int count = 1)
        {
            ConsoleOutputItem outputItem = Instantiate(outputItemPrefab, outputItemContainer);
            outputItem.Init(content, date, count, type, stackTrace);

            LayoutRebuilder.ForceRebuildLayoutImmediate(outputItemContainer);
            outputItems.Add(outputItem);
        }

        public static void Log(object log)
        {
            instance.Send(log);
        }
        public static void Log(object log, LogType type)
        {
            instance.Send(log, type);
        }
        public static void LogError(object log)
        {
            instance.Send(log);
        }
        public static void LogWarning(object log)
        {
            instance.Warning(log);
        }
        public static void LogSuccess(object log)
        {
            instance.Success(log);
        }

        public void FromListener(string logString, string stackTrace, UnityEngine.LogType type)
        {
            for (int i = 0; i < outputItems.Count; i++)
            {
                if (outputItems[i].Get() == logString)
                {
                    outputItems[i].Increase();
                    return;
                }
            }

            LogType logType = LogTypeHelper.To(type);
            CreateOutputItem(logString, DateTime.Now, logType, stackTrace);
        }
    }
}