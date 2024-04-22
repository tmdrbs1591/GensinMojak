using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] clips;
    public AudioClip[] songs;
    public GameObject audioObject;
    public static AudioManager instance;
    AudioSource aud;
    float initVolume;
    int curSong;

    void Awake()
    {
        AudioListener existingListener = FindObjectOfType<AudioListener>();

        // 이미 오디오 리스너가 있으면 이를 비활성화
        if (existingListener != null)
        {
            existingListener.enabled = false;
        }
        else
        {
            // 오디오 리스너가 없으면 생성
            gameObject.AddComponent<AudioListener>();
        }
        instance = this;
        aud = gameObject.GetComponent<AudioSource>();

        // Check if AudioSource component is attached
        if (aud == null)
        {
            // If not, add AudioSource component
            aud = gameObject.AddComponent<AudioSource>();
        }

        initVolume = aud.volume;
    }


    public IEnumerator SwitchSong(int index)
    {
        if (curSong == index) yield break;
        for (float i = 0; i < 1; i += Time.unscaledDeltaTime)
        {
            aud.volume = Mathf.Lerp(initVolume, 0, i);

            yield return null;
        }
        aud.Stop();
        aud.clip = songs[index];
        aud.Play();
        for (float i = 0; i < 1; i += Time.unscaledDeltaTime)
        {
            aud.volume = Mathf.Lerp(0, initVolume, i);
            yield return null;
        }
        curSong = index;
        yield break;

    }

    public void PlaySound(Vector3 position, int index, float pitch = 1, float volume = 1, Transform follower = null)
    {
        AudioObject aud = GameObject.Instantiate(audioObject, new Vector3(position.x, position.y, -5), Quaternion.identity).GetComponent<AudioObject>();
        aud.follow = follower;
        aud.clip = clips[index];
        aud.pitch = pitch;
        aud.volume = volume;

    }
}
