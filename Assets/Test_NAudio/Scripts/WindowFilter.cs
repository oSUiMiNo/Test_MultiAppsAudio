using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public partial class WindowFilter : MonoBehaviour
{
    // ���X�̓ǂݍ���
    [DllImport("USER32.DLL")]
    private static extern IntPtr GetForegroundWindow();


    [DllImport("USER32.DLL")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetForegroundWindow(IntPtr hWnd);


    private static string searchWindowText = null;
    private static string searchClassName = null;
    private static ArrayList foundProcessIds = null;
    private static ArrayList foundProcesses = null;

    private delegate bool EnumWindowsDelegate(IntPtr hWnd, IntPtr lparam);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private extern static bool EnumWindows(EnumWindowsDelegate lpEnumFunc,
        IntPtr lparam);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern int GetWindowText(IntPtr hWnd,
        StringBuilder lpString, int nMaxCount);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern int GetWindowTextLength(IntPtr hWnd);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern int GetClassName(IntPtr hWnd,
        StringBuilder lpClassName, int nMaxCount);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int GetWindowThreadProcessId(
        IntPtr hWnd, out int lpdwProcessId);


    private void Start()
    {
        GetProcessesOnlyWindow();
    }


    public static Process[] GetProcessesByWindow(
    string windowText, string className)
    {
        //�����̏���������
        foundProcesses = new ArrayList();
        foundProcessIds = new ArrayList();
        searchWindowText = windowText;
        searchClassName = className;

        //�E�B���h�E��񋓂��āA�Ώۂ̃v���Z�X��T��
        EnumWindows(new EnumWindowsDelegate(EnumWindowCallBack), IntPtr.Zero);

        //���ʂ�Ԃ�
        return (Process[])foundProcesses.ToArray(typeof(Process));
    }

    public static List<Process> GetProcessesOnlyWindow()
    {

        List<Process> foundProcesses = new List<Process>();

        //�E�B���h�E�̃^�C�g���Ɂu�v���܂ރv���Z�X�����ׂĎ擾����
        Process[] ps = GetProcessesByWindow("", null);

        //window�̖��O������v���Z�X���������X�g�ɓ����
        foreach (Process p in ps)
        {
            if (p.MainWindowTitle.Length > 1)
            {
                foundProcesses.Add(p);
            }
        }

        return foundProcesses;

    }

    private static bool EnumWindowCallBack(IntPtr hWnd, IntPtr lparam)
    {
        if (searchWindowText != null)
        {
            //�E�B���h�E�̃^�C�g���̒������擾����
            int textLen = GetWindowTextLength(hWnd);
            if (textLen == 0)
            {
                //���̃E�B���h�E������
                return true;
            }
            //�E�B���h�E�̃^�C�g�����擾����
            StringBuilder tsb = new StringBuilder(textLen + 1);
            GetWindowText(hWnd, tsb, tsb.Capacity);
            //�^�C�g���Ɏw�肳�ꂽ��������܂ނ�
            if (tsb.ToString().IndexOf(searchWindowText) < 0)
            {
                //�܂�ł��Ȃ����́A���̃E�B���h�E������
                return true;
            }
        }

        if (searchClassName != null)
        {
            //�E�B���h�E�̃N���X�����擾����
            StringBuilder csb = new StringBuilder(256);
            GetClassName(hWnd, csb, csb.Capacity);
            //�N���X���Ɏw�肳�ꂽ��������܂ނ�
            if (csb.ToString().IndexOf(searchClassName) < 0)
            {
                //�܂�ł��Ȃ����́A���̃E�B���h�E������
                return true;
            }
        }

        //�v���Z�X��ID���擾����
        int processId;
        GetWindowThreadProcessId(hWnd, out processId);
        //���܂Ō��������v���Z�X�ł͖������Ƃ��m�F����
        if (!foundProcessIds.Contains(processId))
        {
            foundProcessIds.Add(processId);
            //�v���Z�XID������Process�I�u�W�F�N�g���쐬����
            foundProcesses.Add(Process.GetProcessById(processId));
        }

        //���̃E�B���h�E������
        return true;
    }
}