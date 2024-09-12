using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using uWindowCapture;


public class AltTab : MonoBehaviour
{
    public static List<string> windowTitles = new List<string>();
    public static Dictionary<string, UwcWindow> windows = new Dictionary<string, UwcWindow>();

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
           GetAltTabWindows();
        }
    }

    public static void GetAltTabWindows()
    {
        List<string> buffer_WindowTitles = new List<string>();
        Dictionary<string, UwcWindow> buffer_Windows = new Dictionary<string, UwcWindow>();
        foreach (var kv in UwcManager.windows)
        {
            var window = kv.Value;
            if (window.isAltTabWindow)
            {
                buffer_WindowTitles.Add(window.title);
                buffer_Windows.Add(window.title, window);

                //Process process = Process.GetProcessById(window.processId);
                //ProcessThreadCollection threads = process.Threads;
                ////ProcessThread thread = threads[window.threadId];
                //UnityEngine.Debug.Log(
                //    $"タイトル: {window.title}\n" +
                //    $"メインウィンドウ名 {process.MainWindowTitle}\n" +
                //    $"プロセスID {window.processId}\n" +
                //    $"スレッドID {window.threadId}\n" +
                //    $"ウィンドウハンドル {window.handle}\n" +
                //    $"プロセス名 {process.ProcessName}\n" +
                //    $"スレッド数 {threads.Count}\n"
                //);
            }
        }
        windowTitles = buffer_WindowTitles;
        windows = buffer_Windows;
    }
}