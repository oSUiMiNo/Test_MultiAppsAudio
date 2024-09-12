using NAudio.Wasapi.CoreAudioApi.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ActivateAudioInterface : MonoBehaviour
{
    //[DllImport("Mmdevapi.dll", ExactSpelling = true, PreserveSig = false)]
    //public static extern void ActivateAudioInterfaceAsync(
    //   [In, MarshalAs(Unma  nagedType.LPWStr)] string deviceInterfacePath,
    //   [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid,
    //   [In] IntPtr activationParams,
    //   [In] IActivateAudioInterfaceCompletionHandler completionHandler,
    //   out IActivateAudioInterfaceAsyncOperation createAsync);

    //// Start is called before the first frame update
    //void Start()
    //{
    //    _isRecording = true;

    //    var defaultAudioCaptureId = MediaDevice.GetDefaultAudioCaptureId(AudioDeviceRole.Default);
    //    var completionHandler = new ActivateAudioInterfaceCompletionHandler(StartCapture);
    //    IActivateAudioInterfaceAsyncOperation createAsync;

    //    WindowsMultimediaDevice.ActivateAudioInterfaceAsync(
    //        defaultAudioCaptureId, new Guid(CoreAudio.Components.WASAPI.Constants.IID_IAudioClient),
    //        IntPtr.Zero, completionHandler, out createAsync);
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
