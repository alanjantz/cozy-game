using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(playEnvironmentMusic());
    }

    IEnumerator playEnvironmentMusic()
    {
        foreach (var audio in MainAssets.Instance.environmentMusic)
        {
            GetComponent<AudioSource>().clip = audio;
            GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(audio.length);
        }
    }
}
