using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip[] tracks;
    int currentTrackNum = -1;
    bool currentlyWarbling;

    Coroutine fadeIn;

    public void SetTrack(int i, bool fadeMusic = true)
    {
        if (currentTrackNum == i) return; // Don't interrupt the current track if it's the same

        currentTrackNum = i;
        source.clip = tracks[i];
        source.Play();

        if (fadeMusic)
        {
            source.volume = 0.1f;
            if (fadeIn != null)
            {
                StopCoroutine(fadeIn);
            }
            fadeIn = StartCoroutine(FadeInMusic(source, 2f, 1f));
        }
    }

    public int GetTrackNum()
    {
        return currentTrackNum;
    }

    public void StopMusic()
    {
        source.clip = null;
        currentTrackNum = -1;
    }

    public void PitchBend(float amt, float dur)
    {
        if (currentlyWarbling) return;
        if (source.clip == null) return;

        StartCoroutine(BendPitch(amt, dur));
    }

    // https://johnleonardfrench.com/how-to-fade-audio-in-unity-i-tested-every-method-this-ones-the-best/
    private static IEnumerator FadeInMusic(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }

    private IEnumerator BendPitch(float amount, float duration)
    {
        currentlyWarbling = true;

        for (float i = 0; i < duration; i += 0.1f)
        {
            source.pitch = 1f + (Random.value * amount * 2f - amount);
            yield return new WaitForSeconds(0.1f);
        }
        source.pitch = 1f;
    }
}
