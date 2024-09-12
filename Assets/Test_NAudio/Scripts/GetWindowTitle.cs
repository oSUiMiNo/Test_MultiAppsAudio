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
        // �ڂɌ�����E�B���h�E�����ɍi��Ȃ��Ɖ���������ăt���[�Y����
        if (IsWindowVisible(hWnd) == false) return true;

        //�E�B���h�E�̃^�C�g���̒������擾����
        int textLen = GetWindowTextLength(hWnd);
        if (textLen == 0) return true;

        IntPtr shellWindow = GetShellWindow();
        if (hWnd == shellWindow)
        {
            return true;
        }

        StringBuilder title = new StringBuilder(textLen + 1);

        GetWindowText(hWnd, title, title.Capacity);

        UnityEngine.Debug.Log($"�^�C�g��: {title}");
        if (title.ToString().Contains("test", StringComparison.OrdinalIgnoreCase))
        {
            if (WindowList.Contains(title.ToString())) return true;
            else
            {
                WindowList.Add(title.ToString());
                UnityEngine.Debug.Log($"�E�B���h�E�n���h��: {hWnd}, �^�C�g��: {title}");
                //return false; // �����I��
            }
        }
        return true; // �������s
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

