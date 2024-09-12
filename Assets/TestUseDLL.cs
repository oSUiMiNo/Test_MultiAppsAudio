using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class TestUseDLL : MonoBehaviour
{
    [DllImport("MyDllForUnity.dll")]
    private static extern IntPtr createExportTest();

    [DllImport("MyDllForUnity.dll")]
    private static extern void freeExportTest(IntPtr instance);

    [DllImport("MyDllForUnity.dll")]
    private static extern int getResult(IntPtr instance, int a);



    [DllImport("MyDllForUnity.dll")]
    private static extern int TestFunc_CSformCPP(IntPtr callback, System.Int32 in1, System.Int32 in2);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    delegate void TypeOfCallback(Int32 a, Int32 b);

    private TypeOfCallback _delegateObject;
    private GCHandle _gcHandle;
    private void Callback(Int32 a, Int32 b)
    {
        Debug.Log($"C++ ‚©‚çŒÄ‚Î‚ê‚½ {a} {b}");
    }


    private void Start()
    {
        CallCPP_FormCS();
        Set_CallCS_FormCPP();
    }


    void CallCPP_FormCS()
    {
        IntPtr test = createExportTest();
        Debug.Log(test);

        int result = getResult(test, 10);
        Debug.Log(result);

        freeExportTest(test);
    }


    void Set_CallCS_FormCPP()
    {
        _delegateObject = Callback;
        IntPtr pointer = Marshal.GetFunctionPointerForDelegate(_delegateObject);
        _gcHandle = GCHandle.Alloc(_delegateObject);

        TestFunc_CSformCPP(pointer, 10, 15);

        _gcHandle.Free();
    }
}