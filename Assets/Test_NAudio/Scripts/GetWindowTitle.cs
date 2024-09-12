using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityEngine.XR;

public class GetWindowTitle : MonoBehaviour
{
    delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
    
    void Start()
    {
        EnumWindows(MyEnumWindowsProc, IntPtr.Zero);
    }

    public List<string> WindowList = new List<string>();
    int count = 0;
    int max;

    bool MyEnumWindowsProc(IntPtr hWnd, IntPtr lParam)
    {
        // 目に見えるウィンドウだけに絞らないと何千個もあってフリーズする
        if (IsWindowVisible(hWnd) == false) return true;

        //ウィンドウのタイトルの長さを取得する
        int textLen = GetWindowTextLength(hWnd);
        if (textLen == 0) return true;

        IntPtr shellWindow = GetShellWindow();
        if (hWnd == shellWindow)
        {
            return true;
        }

        StringBuilder title = new StringBuilder(textLen + 1);

        GetWindowText(hWnd, title, title.Capacity);

        UnityEngine.Debug.Log($"タイトル: {title}");
        if (title.ToString().Contains("test", StringComparison.OrdinalIgnoreCase))
        {
            if (WindowList.Contains(title.ToString())) return true;
            else
            {
                WindowList.Add(title.ToString());
                UnityEngine.Debug.Log($"ウィンドウハンドル: {hWnd}, タイトル: {title}");
                //return false; // 検索終了
            }
        }
        return true; // 検索続行
    }



    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    [DllImport("user32.dll")]
    static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);


    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern int GetWindowTextLength(IntPtr hWnd);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern int GetClassName(IntPtr hWnd,
        StringBuilder lpClassName, int nMaxCount);
    
    [DllImport("user32.dll")]
    private static extern bool IsWindowVisible(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern IntPtr GetShellWindow();
}

