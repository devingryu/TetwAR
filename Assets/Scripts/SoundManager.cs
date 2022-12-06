using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TAR
{
    public class SoundManager : Singleton<SoundManager>
    {
        public AudioSource BgmPlayer;
        public AudioSource EffectPlayer;

        public AudioClip breakFloor;
        public AudioClip clickButton;
        public AudioClip placeBlock;

        public float BGMVolume { get; set; }
        public float EffectVolume { get; set; }

        [SerializeField] private AudioClip[] EffectAudioClips;

        private Dictionary<string, AudioClip> EffectSoundDictionary = new Dictionary<string, AudioClip>();

        void Awake()
        {
            GameObject EffectTempObject = new GameObject("Effect");
            EffectTempObject.transform.SetParent(gameObject.transform);
            EffectPlayer = EffectTempObject.AddComponent<AudioSource>();

            GameObject BgmTempObject = new GameObject("Bgm");
            BgmTempObject.transform.SetParent(gameObject.transform);
            BgmPlayer = BgmTempObject.AddComponent<AudioSource>();

            foreach (AudioClip audioclip in EffectAudioClips)
            {
                EffectSoundDictionary.Add(audioclip.name, audioclip);
            }

            BGMVolume = 1f;
            EffectVolume = 0.3f;

            EffectSoundDictionary.Add("BreakFloor", breakFloor);
            EffectSoundDictionary.Add("ClickButton", clickButton);
            EffectSoundDictionary.Add("PlaceBlock", placeBlock);
        }

        void Start()
        {

        }

        public void PlayEffect(string name)
        {
            EffectPlayer.PlayOneShot(EffectSoundDictionary[name], EffectVolume);
        }

        public void PlayBgm(string name)
        {
            BgmPlayer.loop = true;
            BgmPlayer.volume = BGMVolume;

            BgmPlayer.clip = EffectSoundDictionary[name];
            BgmPlayer.Play();
        }

        public void StopBgm()
        {
            BgmPlayer.clip = null;
            BgmPlayer.Stop();
        }

    }
}