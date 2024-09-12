using System;
using NAudio.Wave;
using UnityEngine;


public class DirectSoundCaptureToUnity : MonoBehaviour
{
    private WaveInEvent waveIn;
    private AudioSource audioSource;
    private float[] audioBuffer;
    private int bufferSize = 1024;
    private int sampleRate = 44100;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioBuffer = new float[bufferSize];

        // DirectSoundでキャプチャの開始
        waveIn = new WaveInEvent();
        waveIn.WaveFormat = new WaveFormat(sampleRate, 16); // サンプルレートとビット深度の設定
        waveIn.DataAvailable += OnDataAvailable;
        waveIn.StartRecording();

        // UnityのAudioSourceに設定
        AudioClip audioClip = AudioClip.Create("DirectSoundAudio", bufferSize, 1, sampleRate, true, OnAudioRead);
        audioSource.clip = audioClip;
        audioSource.loop = true;
        audioSource.Play();
    }

    // DirectSoundで得られたデータを処理
    void OnDataAvailable(object sender, WaveInEventArgs e)
    {
        // byte[]データをfloat[]に変換してaudioBufferに保存
        for (int i = 0; i < e.BytesRecorded / 2; i++)
        {
            audioBuffer[i] = BitConverter.ToInt16(e.Buffer, i * 2) / 32768f;
        }
    }

    // UnityのAudioSourceにデータを渡す
    void OnAudioRead(float[] data)
    {
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = audioBuffer[i];
        }
    }

    void OnDestroy()
    {
        if (waveIn != null)
        {
            waveIn.StopRecording();
            waveIn.Dispose();
        }
    }
}
