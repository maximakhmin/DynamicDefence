using System.Collections;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioClip[] audioClips;
    private int iter;

    private static float clipTime = 150f;
    private static float clipDelta = 5f;
    private float deltaTime = 0f;

    private AudioSource audioSource;
    private Coroutine offVolumeCoroutine;


    void Start()
    {
        iter = Random.Range(0, audioClips.Length - 1);

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.volume = 0f;
        audioSource.resource = audioClips[iter];
        audioSource.Play();
        StartCoroutine(onVolume());
    }

    
    void Update()
    {
        if (deltaTime > (clipTime - clipDelta - 1) && offVolumeCoroutine==null)
        {
            offVolumeCoroutine = StartCoroutine(offVolume());
        }
        if (deltaTime > clipTime)
        {
            int newIter = Random.Range(0, audioClips.Length - 1);
            if (iter == newIter) newIter = Random.Range(0, audioClips.Length - 1);
            iter = newIter;
            audioSource.resource = audioClips[iter];
            audioSource.Play();
            deltaTime = 0;
            StartCoroutine(onVolume());
            offVolumeCoroutine = null;

            deltaTime += Time.unscaledDeltaTime;
        }
    }

    IEnumerator offVolume()
    {
        float delta = 0f;
        while (delta < clipDelta)
        {
            audioSource.volume = 1 - delta / clipDelta;
            delta += Time.unscaledDeltaTime;
            yield return null;
        }
        audioSource.volume = 0f;
    }
    IEnumerator onVolume()
    {
        float delta = 0f;
        while (delta < clipDelta)
        {
            audioSource.volume = delta / clipDelta;
            delta += Time.unscaledDeltaTime;
            yield return null;
        }
        audioSource.volume = 1f;
    }
}
