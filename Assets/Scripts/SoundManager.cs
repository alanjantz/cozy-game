using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(PlayEnvironmentMusic());
    }

    private IEnumerator PlayEnvironmentMusic()
    {
        foreach (var audio in MainAssets.Instance.environmentMusic.OrderBy(_ => Guid.NewGuid()))
        {
            GetComponent<AudioSource>().clip = audio;
            GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(audio.length);
        }
    }
}
