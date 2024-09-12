using UnityEngine;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using System.Collections;

public class AudioCapture : MonoBehaviour
{
    public string processName;

    void Start()
    {
        StartCoroutine(CaptureAudioCoroutine(processName));
    }

    private IEnumerator CaptureAudioCoroutine(string processName)
    {
        var enumerator = new MMDeviceEnumerator();
        var device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

        Debug.Log("Using device: " + device.FriendlyName);
        CaptureAudioFromDevice(device);

        yield break;
    }

    private void CaptureAudioFromDevice(MMDevice device)
    {
        try
        {
            // WasapiLoopbackCaptureを使用
            var capture = new WasapiLoopbackCapture(device);

            capture.DataAvailable += (s, a) =>
            {
                // 音声データの処理
                Debug.Log("Capturing audio data...");
            };

            capture.RecordingStopped += (s, a) =>
            {
                Debug.Log("Recording stopped");
                capture.Dispose();
            };

            capture.StartRecording();
        }
        catch (System.Runtime.InteropServices.COMException ex)
        {
            Debug.LogError("COMException: " + ex.Message);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Exception: " + ex.Message);
        }
    }
}
