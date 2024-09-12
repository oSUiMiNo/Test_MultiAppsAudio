using UnityEngine;
using System.Runtime.InteropServices;
using System;

public class AudioCaptureManager : MonoBehaviour
{
    [DllImport("WinCaptureAudio", CallingConvention = CallingConvention.Cdecl)]
    public static extern void StartCapture();

    [DllImport("WinCaptureAudio", CallingConvention = CallingConvention.Cdecl)]
    public static extern void StopCapture();

    void Start()
    {
        try
        {
            StartCapture();
            Debug.Log("Capture started successfully.");
        }
        catch (DllNotFoundException e)
        {
            Debug.LogError($"DLL not found: {e.Message}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error starting capture: {e.Message}");
        }
    }

    void OnApplicationQuit()
    {
        try
        {
            StopCapture();
            Debug.Log("Capture stopped successfully.");
        }
        catch (DllNotFoundException e)
        {
            Debug.LogError($"DLL not found: {e.Message}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error stopping capture: {e.Message}");
        }
    }
}
