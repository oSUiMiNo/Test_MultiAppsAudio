using System;
using System.Runtime.InteropServices;

public class WinCaptureAudio
{
    //[DllImport("win-capture-audio.dll", CallingConvention = CallingConvention.Cdecl)]
    [DllImport("WinCaptureAudio.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern void StartCapture();

    //[DllImport("win-capture-audio.dll", CallingConvention = CallingConvention.Cdecl)]
    [DllImport("WinCaptureAudio.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern void StopCapture();

    // その他必要な関数を宣言
}
