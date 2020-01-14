 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;

    public GameObject P1;
    public GameObject P2;

    private AudioSource P1AS;
    private AudioSource P2AS;

    public AudioClip mainTrack;

    public AudioClip[] ambientMusic;

    public AudioClip[] menuSelectSounds;
    public AudioClip[] menuNavigateSounds;

    public AudioClip[] catSounds;

    public AudioClip[] dogBarkSounds;
    public AudioClip[] dogConfusedSounds;
    public AudioClip[] dogPantSounds;

    public AudioClip[] movingSounds;
        
    public AudioClip[] jumpFallSounds;

    public AudioClip[] fatherSleepySounds;
    public AudioClip[] fatherDialogueSounds;

    public AudioClip[] interactSounds;
    public AudioClip[] throwSounds;

    private AudioSource mainAS;

    void SetInitialReferences()
    {
        instance = this;
        mainAS = GetComponent<AudioSource>();
        DontDestroyOnLoad(this);
    }

    public void Awake()
    {
        SetInitialReferences();
    }

    void OnLevelWasLoaded()
    {
        UpdateCharacters();
    }

    void UpdateCharacters()
    {
        P1 = GameObject.FindGameObjectWithTag("P1");
        if (P1)
        {
            P1AS = P1.GetComponent<AudioSource>();
        }
        P2 = GameObject.FindGameObjectWithTag("P2");
        if (P2)
        {
            P2AS = P2.GetComponent<AudioSource>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (P1AS != null)
        {
            Vector3 P1Velocity = P1.GetComponent<CharacterController>().velocity;
            if (P1Velocity != Vector3.zero && P1Velocity.y == 0)
            {
                StartCoroutine(TransitionTrack(P2AS, movingSounds));
            }
        }

        if (P2AS != null)
        {
            Vector3 P2Velocity = P2.GetComponent<CharacterController>().velocity;


            if (P2Velocity != Vector3.zero && P2Velocity.y == 0)
            {
                StartCoroutine(TransitionTrack(P2AS, movingSounds));
            }
        }
    }

    public IEnumerator TransitionTrack(AudioSource _AS, AudioClip[] _collection)
    {
        if (!_AS.isPlaying)
        {
            //_AS.PlayOneShot(_collection[Random.Range(0, _collection.Length)]);
            yield return null;
        }
    }

    public void EventTrack(Collider _trigger, AudioClip[] _collection)
    {
        AudioSource.PlayClipAtPoint(_collection[Random.Range(0, _collection.Length)], _trigger.gameObject.transform.position);
    }
}
