using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;
using UniRx.Triggers;
using uWindowCapture;
using Cysharp.Threading.Tasks;
using System;

public class WindowUI : MonoBehaviour
{
    MyButton button => GetComponent<MyButton>();
    GameObject parent => transform.parent.gameObject;
    GameObject uWindow;
    GameObject canvas;
    TMP_Dropdown dropdown;
    UwcWindowTexture uwcWindowTexture;

    async void Start()
    {
        uWindow = transform.Find("uWCWindowObject").gameObject; ;
        canvas = transform.Find("Canvas").gameObject;
        dropdown = canvas.transform.Find("Dropdown").gameObject.GetComponent<TMP_Dropdown>();
        uwcWindowTexture = uWindow.GetComponent<UwcWindowTexture>();

        uwcWindowTexture.partialWindowTitle = "*@ML";

        SetDropDownOptions();
        while (AltTab.windowTitles.Count == 0)
        {
            Debug.Log($"{AltTab.windowTitles.Count}");
            await UniTask.WaitForSeconds(0.5f);
            SetDropDownOptions();
        }
        canvas.SetActive(false);

       

        button.On_Enter.Subscribe(_ => 
        {
            canvas.SetActive(true);
            RectTransform canvasTrans = canvas.GetComponent<RectTransform>();
            Vector2 colSize = gameObject.GetComponent<BoxCollider2D>().size;
            colSize.x = canvasTrans.GetWidth();
            colSize.y = canvasTrans.GetHeight();
            gameObject.GetComponent<BoxCollider2D>().size = colSize;
        });
        button.On_Exit.Subscribe(_ =>
        {
            canvas.SetActive(false);
            Vector3 windowScale = uWindow.transform.localScale;
            Vector2 colSize = gameObject.GetComponent<BoxCollider2D>().size;
            colSize.x = windowScale.x - 0.3f;
            colSize.y = windowScale.y - 0.3f;
            if (windowScale.x < 0.3f) colSize.x = windowScale.x;
            if (windowScale.y < 0.3f) colSize.y = windowScale.y;
            gameObject.GetComponent<BoxCollider2D>().size = colSize;
        });

        dropdown.OnPointerClickAsObservable().Subscribe(_ => 
        {
            SetDropDownOptions();
        });

        dropdown.OnSelectAsObservable()
                .TakeUntilDestroy(this)
                .ThrottleFirst(TimeSpan.FromSeconds(0.5))
                .Subscribe( async _ => 
                {
                    await SetWindow(dropdown.value);
                });


        //dropdown.onValueChanged.AddListener(async value =>
        //{
        //    await SetWindow(value);
        //});

    }

    private void SetDropDownOptions()
    {
        AltTab.GetAltTabWindows();
        dropdown.ClearOptions();
        dropdown.AddOptions(AltTab.windowTitles);
    }

    async UniTask SetWindow(int value)
    {
        uwcWindowTexture.partialWindowTitle = dropdown.options[value].text;
        
        await UniTask.WaitForEndOfFrame(this);
        await UniTask.WaitForSeconds(0.03f);

        int windowHeight = AltTab.windows[uwcWindowTexture.partialWindowTitle].height;
        int windowWidth = AltTab.windows[uwcWindowTexture.partialWindowTitle].width;
        float longerSideLength = Mathf.Max(windowHeight, windowWidth);

        uwcWindowTexture.scalePer1000Pixel = 1000 / longerSideLength;

        await UniTask.WaitForEndOfFrame(this);
        await UniTask.WaitForSeconds(0.03f);
        Vector3 windowScale = uWindow.transform.localScale;

        RectTransform canvasTrans = canvas.GetComponent<RectTransform>();
        canvasTrans.SetWidth(windowScale.x);
        canvasTrans.SetHeight(windowScale.y);

        if(windowScale.x < 0.6f) canvasTrans.SetWidth(0.6f);
        if(windowScale.y < 0.6f) canvasTrans.SetHeight(0.6f);
        
        Vector2 colSize = gameObject.GetComponent<BoxCollider2D>().size;
        colSize.x = windowScale.x - 0.3f;
        colSize.y = windowScale.y - 0.3f;
        if (windowScale.x < 0.3f) colSize.x = windowScale.x;
        if (windowScale.y < 0.3f) colSize.y = windowScale.y;
            gameObject.GetComponent<BoxCollider2D>().size = colSize;
    }
}
