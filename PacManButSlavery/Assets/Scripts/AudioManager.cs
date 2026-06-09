using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [Header("Prefabs")]
    public GameObject audioSourcePrefab;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void PlaySound(AudioClip clip, float volume = 1f, float startTime = 0f, float endTime = 1f)
    {
        GameObject audioSourceObj = Instantiate(audioSourcePrefab, transform);
        AudioSource audioSource = audioSourceObj.GetComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.time = startTime;
        audioSource.Play();
        
        Destroy(audioSourceObj, clip.length * (endTime - startTime));
    }

    public void PlayRandomSound(AudioClip[] clips, float volume = 1f, float startTime = 0f, float endTime = 1f)
    {
        if (clips.Length == 0) return;
        int index = Random.Range(0, clips.Length);
        PlaySound(clips[index], volume, startTime, endTime);
    }
}
