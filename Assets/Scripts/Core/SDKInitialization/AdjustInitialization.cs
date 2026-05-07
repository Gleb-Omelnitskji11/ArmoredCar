using AdjustSdk;
using UnityEngine;

public static class AdjustInitialization
{
    private static string AppToken = "demo";
    
    public static void Init()
    {
#if UNITY_EDITOR
        return;
#endif
        AdjustConfig adjustConfig = new AdjustConfig(AppToken, AdjustEnvironment.Sandbox);

        adjustConfig.LogLevel = AdjustLogLevel.Info;
        adjustConfig.IsDeferredDeeplinkOpeningEnabled = true;
        adjustConfig.IsSendingInBackgroundEnabled = true;
        Adjust.InitSdk(adjustConfig);
        CheckStatus();
    }

    public static void CheckStatus()
    {
        Adjust.IsEnabled(LogStatus);
    }

    private static void LogStatus(bool ready)
    {
        Debug.Log($"adjust ready {ready}");
    }
}
