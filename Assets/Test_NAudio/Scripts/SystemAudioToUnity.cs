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

    // Inspector�Ř^���f�o�C�X���w�肷��ꍇ�̖��O(���w��͊���)
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
        
        //// �f�o�C�X��񋓂���ID��\��
        //foreach (var device in deviceEnumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active))
        //{
        //    Debug.Log("Device found: " + device.FriendlyName + " ID: " + device.ID + "     ||");
        //}


        // NAudio�ł̓}�C�N�����E���Ȃ�
        //// NAudio�Ń��[�v�o�b�N�L���v�`�����J�n
        //waveIn = new WaveInEvent();
        //waveIn.WaveFormat = new WaveFormat(sampleRate, 1); // Mono�ŃL���v�`��
        //waveIn.DeviceNumber = num;
        //waveIn.DataAvailable += OnDataAvailable;
        //waveIn.BufferMilliseconds = 100;
        //waveIn.StartRecording();

        // WasapiLoopbackCapture���g�p���ăV�X�e���������L���v�`��
        deviceEnumerator = new MMDeviceEnumerator();


        // �f�o�C�X��񋓂���ID��\��
        foreach (var device in deviceEnumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active))
        {
            // �f�o�C�XID�Ńf�o�C�X���擾
            selectedDevice = deviceEnumerator.GetDevice("{0.0.0.00000000}.{ab05a8af-e5af-42fb-8932-ae47afcedeb7}");

            if (selectedDevice != null)
            {
                Debug.Log($"�f�o�C�X| {device.FriendlyName}\nID| {device.ID}\n");
                GameObject.Find("AudioDeviceText").GetComponent<TextMeshProUGUI>().text += $"�f�o�C�X| {device.FriendlyName}\nID| {device.ID}\n";
                var waveFormat = new WaveFormat(48000, 16); // 16�r�b�g�A48000Hz�ɐݒ�
                // �o�b�t�@�T�C�Y���f�o�C�X�ɓK��������i��: 100ms�j
                capture = new WasapiLoopbackCapture(selectedDevice) { WaveFormat = waveFormat};
                capture.ShareMode = AudioClientShareMode.Shared; // ���L���[�h�ɐݒ�
                //capture.ShareMode = AudioClientShareMode.Exclusive; // �r�����[�h�ɐݒ�
                capture.DataAvailable += OnDataAvailable;
                try
                {
                    capture.StartRecording();
                }
                catch (COMException e)
                {
                    Debug.LogError(e);
                    GameObject.Find("AudioDeviceText").GetComponent<TextMeshProUGUI>().text += $"�G���[�@{device.FriendlyName}\n\n";
                }
            }
            else
            {
                Debug.LogError("Selected device not found");
            }

            //await Delay.Second(1);
        }
        return;

        // AudioSource�p�̃o�b�t�@���쐬
        audioBuffer = new float[bufferSize];
        AudioClip audioClip = AudioClip.Create("SystemAudio", bufferSize, 1, sampleRate, true, OnAudioRead);
        audioSource.clip = audioClip;
        audioSource.loop = true;
        audioSource.mute = false; // �~���[�g�ɂȂ��Ă��Ȃ����Ƃ��m�F
        audioSource.pitch = 0.5f; // �Đ����x��x�����Ēቹ��

        audioSource.Play();
    }

    // ���[�v�o�b�N�œ���ꂽ�f�[�^������
    void OnDataAvailable(object sender, WaveInEventArgs e)
    {
        // NAudio��byte[]�f�[�^��float[]�ɕϊ�����audioBuffer�ɃR�s�[
        for (int i = 0; i < e.BytesRecorded / 2; i++)
        {
            audioBuffer[i] = (float)BitConverter.ToInt16(e.Buffer, i * 2) / 32768f;
        }

        // �f�[�^���m�F���邽�߂Ƀ��O���o��
        Debug.Log("Captured data: " + audioBuffer[0]);
    }

    // AudioSource���Đ�����I�[�f�B�I�f�[�^���
    void OnAudioRead(float[] data)
    {
        // audioBuffer�̓��e��AudioSource�p�ɃR�s�[
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = audioBuffer[i];
        }
    }

    void OnDestroy()
    {
        //���\�[�X�̉��

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
