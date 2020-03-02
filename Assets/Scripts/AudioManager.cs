using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public AudioClip oneshotBGM;
    public AudioClip BGM;
    public AudioClip calm;
    public AudioClip fast;
    public float fadeInDuration, fadeOutDuration;
    public float activationThreshold;
    public float deactivationThreshold;
    public GameObject audioSourcePrefab;

    public static AudioManager instance;

    private bool active = false;
    [HideInInspector]
    public Rigidbody player;
    private readonly List<AudioSource> allClips = new List<AudioSource>();
    private readonly HashSet<AudioSource> activeChannels = new HashSet<AudioSource>();
    private readonly Dictionary<AudioClip, AudioSource> SourceMap = new Dictionary<AudioClip, AudioSource>();
    private readonly Dictionary<AudioClip, Coroutine> crossfadeCoroutines = new Dictionary<AudioClip, Coroutine>();

    // Use this for initialization
    void Start()
    {
        if (instance != null)
        {
            instance.player = GameObject.Find("Player").GetComponent<Rigidbody>();
            Destroy(gameObject);
            return;
        }
        instance = this;
        player = GameObject.Find("Player").GetComponent<Rigidbody>();
        DontDestroyOnLoad(gameObject);

        if(oneshotBGM != null)
        {
            InitClip(oneshotBGM);
            InitClip(BGM);
            SourceMap[oneshotBGM].loop = false;
            PlayImmediately(oneshotBGM);
            Invoke("StartBGM", oneshotBGM.length);
        }
        else
        {
            StartBGM();
        }
        InitClip(calm);
        InitClip(fast);
        
        PlayImmediately(calm);
    }

    private void StartBGM()
    {
        StopImmediately(oneshotBGM);
        if (BGM != null)
        {
            SourceMap[BGM].time = 0;
        }
        PlayImmediately(BGM);
    }

    private void InitClip(AudioClip clip)
    {
        if (SourceMap.ContainsKey(clip) || clip == null)
        {
            return;
        }
        var source = Instantiate(audioSourcePrefab, transform).GetComponent<AudioSource>();
        SourceMap[clip] = source;
        source.loop = true;
        source.volume = 0;
        source.clip = clip;
        source.Play();
        allClips.Add(source);
    }

    private void enable()
    {
        CrossfadeIn(fast, fadeInDuration);
        CrossfadeOut(calm, fadeInDuration);
    }

    private void disable()
    {
        CrossfadeOut(fast, fadeOutDuration);
        CrossfadeIn(calm, fadeOutDuration);
    }

    void Update()
    {
        if (active)
        {
            if (player.velocity.magnitude < deactivationThreshold)
            {
                active = false;
                disable();
            }
        }
        else
        {
            if (player.velocity.magnitude > activationThreshold)
            {
                active = true;
                enable();
            }
        }
    }

    private void PlayImmediately(AudioClip clip)
    {
        if(clip == null)
        {
            return;
        }
        if (!SourceMap.ContainsKey(clip))
        {
            Debug.Log("No clip " + clip.name + " found.\nDid you forget to Initilize it?");
            return;
        }
        var source = SourceMap[clip];
        if (crossfadeCoroutines.ContainsKey(clip))
        {
            StopCoroutine(crossfadeCoroutines[clip]);
            crossfadeCoroutines.Remove(clip);
        }
        source.volume = 1;
    }

    private void StopImmediately(AudioClip clip)
    {
        if (clip == null)
        {
            return;
        }
        if (!SourceMap.ContainsKey(clip))
        {
            Debug.Log("No clip " + clip.name + " found.\nDid you forget to Initilize it?");
            return;
        }
        var source = SourceMap[clip];
        if (crossfadeCoroutines.ContainsKey(clip))
        {
            StopCoroutine(crossfadeCoroutines[clip]);
            crossfadeCoroutines.Remove(clip);
        }
        source.volume = 0;
    }

    private void CrossfadeIn(AudioClip clip, float crossfadeDuration)
    {
        if (clip == null)
        {
            return;
        }
        if (!SourceMap.ContainsKey(clip))
        {
            Debug.Log("No clip " + clip.name + " found.\nDid you forget to Initilize it?");
            return;
        }
        if (crossfadeCoroutines.ContainsKey(clip))
        {
            StopCoroutine(crossfadeCoroutines[clip]);
            crossfadeCoroutines.Remove(clip);
        }
        crossfadeCoroutines.Add(clip, StartCoroutine(CrossfadeVolume(clip, crossfadeDuration)));
    }

    private void CrossfadeOut(AudioClip clip, float crossfadeDuration)
    {
        if (clip == null)
        {
            return;
        }
        if (!SourceMap.ContainsKey(clip))
        {
            Debug.Log("No clip " + clip.name + " found.\nDid you forget to Initilize it?");
            return;
        }
        if (crossfadeCoroutines.ContainsKey(clip))
        {
            StopCoroutine(crossfadeCoroutines[clip]);
            crossfadeCoroutines.Remove(clip);
        }
        crossfadeCoroutines.Add(clip, StartCoroutine(CrossfadeVolume(clip, crossfadeDuration, true)));
    }

    private IEnumerator CrossfadeVolume(AudioClip clip, float duration, bool fadeOut = false)
    {
        var source = SourceMap[clip];
        var rate = (fadeOut ? -1 : 1) / duration;
        var timeElapsedSoFar = 0f;
        while (timeElapsedSoFar < duration)
        {
            source.volume = Mathf.Clamp01(source.volume + rate * Time.deltaTime);
            yield return new WaitForEndOfFrame();
            timeElapsedSoFar += Time.deltaTime;
        }
        crossfadeCoroutines.Remove(clip);
    }

}
