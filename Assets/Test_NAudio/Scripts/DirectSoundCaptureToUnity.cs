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

        // DirectSound�ŃL���v�`���̊J�n
        waveIn = new WaveInEvent();
        waveIn.WaveFormat = new WaveFormat(sampleRate, 16); // �T���v�����[�g�ƃr�b�g�[�x�̐ݒ�
        waveIn.DataAvailable += OnDataAvailable;
        waveIn.StartRecording();

        // Unity��AudioSource�ɐݒ�
        AudioClip audioClip = AudioClip.Create("DirectSoundAudio", bufferSize, 1, sampleRate, true, OnAudioRead);
        audioSource.clip = audioClip;
        audioSource.loop = true;
        audioSource.Play();
    }

    // DirectSound�œ���ꂽ�f�[�^������
    void OnDataAvailable(object sender, WaveInEventArgs e)
    {
        // byte[]�f�[�^��float[]�ɕϊ�����audioBuffer�ɕۑ�
        for (int i = 0; i < e.BytesRecorded / 2; i++)
        {
            audioBuffer[i] = BitConverter.ToInt16(e.Buffer, i * 2) / 32768f;
        }
    }

    // Unity��AudioSource�Ƀf�[�^��n��
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
