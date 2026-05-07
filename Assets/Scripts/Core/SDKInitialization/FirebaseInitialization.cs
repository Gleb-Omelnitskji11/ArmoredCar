using System;
using System.Threading.Tasks;
using Firebase;
using UnityEngine;

public static class FirebaseInitialization
{
    private static FirebaseApp FirebaseInst;
    public static async Task<bool> InitFirebase()
    {
        try
        {
            DependencyStatus dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();

            if (dependencyStatus == DependencyStatus.Available)
            {
                FirebaseInst = FirebaseApp.DefaultInstance;

                Debug.Log("Firebase init success!");
                return true;
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
                return false;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Exception during trying init firebase: {ex}");
        }

        return false;
    }

    public static void CheckStatus()
    {
        Debug.Log($"Firebase ready {FirebaseInst != null}");
    }
}
