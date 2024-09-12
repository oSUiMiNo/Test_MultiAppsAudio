//using UnityEngine;
//using System.IO;
//using System.Collections;

//public class AudioPlayback : MonoBehaviour
//{
//    public string audioFilePath;
//    private AudioSource audioSource;

//    void Start()
//    {
//        audioFilePath = $@"{Application.dataPath}\CapturedAudio";
//        audioSource = GetComponent<AudioSource>();
//        StartCoroutine(LoadAudio());
//    }

//    private IEnumerator LoadAudio()
//    {
//        AudioCapture.StartCapture(audioFilePath);
//        yield return new WaitForSeconds(5);
//        AudioCapture.StopCapture();
//        Debug.Log("ò^âπèIóπ");

//        using (var www = new WWW("file://" + audioFilePath))
//        {
//            yield return www;

//            if (www.error == null)
//            {
//                audioSource.clip = www.GetAudioClip();
//                audioSource.Play();
//            }
//            else
//            {
//                Debug.LogError("Failed to load audio: " + www.error);
//            }
//        }
//    }
//}
