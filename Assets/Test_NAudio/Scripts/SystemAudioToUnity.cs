using UnityEngine;
using NAudio.Wave;
using System;
using System.Windows;
using NAudio.CoreAudioApi;
using System.Runtime.InteropServices;
using TMPro;

public class SystemAudioToUnity : MonoBehaviour
{
    private AudioSource audioSource;
    private WaveInEvent waveIn;
    private WasapiLoopbackCapture capture;
    private MMDeviceEnumerator deviceEnumerator;
    private MMDevice selectedDevice;
    private float[] audioBuffer;
    private int bufferSize = 1024;
    private int sampleRate = 44100;

    // Inspectorで録音デバイスを指定する場合の名前(未指定は既定)
    [SerializeField]
    int num;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        int selectDevice = 0;

        //deviceEnumerator = new MMDeviceEnumerator();

        //foreach (var a in Microphone.devices)
        //{
        //    Debug.Log(a);
        //}
        
        //// デバイスを列挙してIDを表示
        //foreach (var device in deviceEnumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active))
        //{
        //    Debug.Log("Device found: " + device.FriendlyName + " ID: " + device.ID + "     ||");
        //}


        // NAudioではマイクしか拾えない
        //// NAudioでループバックキャプチャを開始
        //waveIn = new WaveInEvent();
        //waveIn.WaveFormat = new WaveFormat(sampleRate, 1); // Monoでキャプチャ
        //waveIn.DeviceNumber = num;
        //waveIn.DataAvailable += OnDataAvailable;
        //waveIn.BufferMilliseconds = 100;
        //waveIn.StartRecording();

        // WasapiLoopbackCaptureを使用してシステム音声をキャプチャ
        deviceEnumerator = new MMDeviceEnumerator();


        // デバイスを列挙してIDを表示
        foreach (var device in deviceEnumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active))
        {
            // デバイスIDでデバイスを取得
            selectedDevice = deviceEnumerator.GetDevice("{0.0.0.00000000}.{ab05a8af-e5af-42fb-8932-ae47afcedeb7}");

            if (selectedDevice != null)
            {
                Debug.Log($"デバイス| {device.FriendlyName}\nID| {device.ID}\n");
                GameObject.Find("AudioDeviceText").GetComponent<TextMeshProUGUI>().text += $"デバイス| {device.FriendlyName}\nID| {device.ID}\n";
                var waveFormat = new WaveFormat(48000, 16); // 16ビット、48000Hzに設定
                // バッファサイズをデバイスに適合させる（例: 100ms）
                capture = new WasapiLoopbackCapture(selectedDevice) { WaveFormat = waveFormat};
                capture.ShareMode = AudioClientShareMode.Shared; // 共有モードに設定
                //capture.ShareMode = AudioClientShareMode.Exclusive; // 排他モードに設定
                capture.DataAvailable += OnDataAvailable;
                try
                {
                    capture.StartRecording();
                }
                catch (COMException e)
                {
                    Debug.LogError(e);
                    GameObject.Find("AudioDeviceText").GetComponent<TextMeshProUGUI>().text += $"エラー　{device.FriendlyName}\n\n";
                }
            }
            else
            {
                Debug.LogError("Selected device not found");
            }

            //await Delay.Second(1);
        }
        return;

        // AudioSource用のバッファを作成
        audioBuffer = new float[bufferSize];
        AudioClip audioClip = AudioClip.Create("SystemAudio", bufferSize, 1, sampleRate, true, OnAudioRead);
        audioSource.clip = audioClip;
        audioSource.loop = true;
        audioSource.mute = false; // ミュートになっていないことを確認
        audioSource.pitch = 0.5f; // 再生速度を遅くして低音に

        audioSource.Play();
    }

    // ループバックで得られたデータを処理
    void OnDataAvailable(object sender, WaveInEventArgs e)
    {
        // NAudioのbyte[]データをfloat[]に変換してaudioBufferにコピー
        for (int i = 0; i < e.BytesRecorded / 2; i++)
        {
            audioBuffer[i] = (float)BitConverter.ToInt16(e.Buffer, i * 2) / 32768f;
        }

        // データを確認するためにログを出力
        Debug.Log("Captured data: " + audioBuffer[0]);
    }

    // AudioSourceが再生するオーディオデータを提供
    void OnAudioRead(float[] data)
    {
        // audioBufferの内容をAudioSource用にコピー
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = audioBuffer[i];
        }
    }

    void OnDestroy()
    {
        //リソースの解放

        //if (waveIn != null)
        //{
        //    waveIn.StopRecording();
        //    waveIn.Dispose();
        //}
        if (capture != null)
        {
            capture.StopRecording();
            capture.Dispose();
        }
    }
}
