using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Text;

public class PerformanceTest : MonoBehaviour
{
    #region Inspector Properties
    [SerializeField, Min(0)]
    private float TestLength = 10;

    [SerializeField, Min(0)]
    private float WarmupLength = 5;
    #endregion

    #region Private Fields
    private int totalFrameCount;
    private float currentFPS;
    private float minFPS = float.MaxValue;
    private float maxFPS = float.MinValue;
    private BlockManager blockManager;
    #endregion

    #region Properties
    public bool IsWarmupFinished => Time.realtimeSinceStartup >= WarmupLength;
    public bool IsTestFinished => Time.realtimeSinceStartup >= WarmupLength + TestLength;
    public float AvgFPS => totalFrameCount / Time.realtimeSinceStartup;
    #endregion

    #region Unity Event Methods
    private void Awake()
    {
        blockManager = GetComponent<BlockManager>();

        if (float.TryParse(GetCommandLineArgValue("-testlength"), out var testLengthArgValue) &&
            testLengthArgValue > 0)
        {
            TestLength = testLengthArgValue;
        }
    }

    private void Update()
    {
        if (!IsWarmupFinished) return;

        totalFrameCount++;
        currentFPS = 1f / Time.unscaledDeltaTime;

        if (currentFPS < minFPS)
        {
            minFPS = currentFPS;
        }
        if (currentFPS > maxFPS)
        {
            maxFPS = currentFPS;
        }

        if (IsTestFinished)
        {
            SaveResults();
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
    #endregion

    #region Helper Methods
    private string GetCommandLineArgValue(string argTag)
    {
        var args = Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length - 1; i++)
        {
            if (string.Equals(args[i], argTag, StringComparison.CurrentCultureIgnoreCase))
            {
                return args[i + 1];
            }
        }

        return null;
    }

    private void SaveResults()
    {
        var date = DateTime.Now;
        var deviceName = SystemInfo.deviceName;
        var operatingSystem = SystemInfo.operatingSystem;
        var processorType = SystemInfo.processorType;

        var results = new TestResult
        {
            Date = date.ToShortDateString(),
            Time = date.ToShortTimeString(),

            DeviceName = deviceName,
            OperatingSystem = operatingSystem,
            ProcessorType = processorType,

            WarmupLength = WarmupLength,
            TestLength = TestLength,
            TotalFrameCount = totalFrameCount,

            MinFramesPerSecond = minFPS,
            MaxFramesPerSecond = maxFPS,
            AverageFramesPerSecond = AvgFPS,
        };

        var jsonData = JsonUtility.ToJson(results, true);
        var fileName = $"{name}_{date.ToFileTime()}.json";
        var fileDir = Path.Combine(Application.dataPath, "PerfTestResults");
        var filePath = Path.Combine(fileDir, fileName);

        Directory.CreateDirectory(fileDir);
        File.WriteAllText(filePath, jsonData, Encoding.UTF8);
    }
    #endregion

    [Serializable]
    private class TestResult
    {
        public string Date;
        public string Time;
        public string DeviceName;
        public string OperatingSystem;
        public string ProcessorType;
        public float WarmupLength;
        public float TestLength;
        public int TotalFrameCount;
        public float MinFramesPerSecond;
        public float MaxFramesPerSecond;
        public float AverageFramesPerSecond;
    }
}