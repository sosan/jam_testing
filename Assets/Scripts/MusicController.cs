using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicController : MonoBehaviour
{

    [SerializeField] private AudioMixer mixer = null;
    [SerializeField] private AudioSource musicSource = null;
    [SerializeField] private AudioSource sfxSource = null;
    
   
    [Header("SFX")]
    public AudioClip[] sfx;

    [Header("Sounds")]
    public AudioClip[] sounds;

    public static MusicController MusicInstance { get; private set; } = null;

    private bool isMuted = false;

    void Awake()
    {

        if (MusicInstance != null && MusicInstance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            MusicInstance = this;
        }
        //DontDestroyOnLoad(this.gameObject);


    }

    public void PlayInGameMusic(AudioClip clip, bool loop = true)
    {
        if (clip == null)
        {
            Debug.LogError("Audio clip no existe");
            return;

        }

        musicSource.Stop();
        musicSource.loop = loop;
        musicSource.clip = clip;
        musicSource.Play();

    }


    public void PlayFXSound(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogError("Audio clip no existe");
            return;

        }

        sfxSource.clip = clip;
        sfxSource.Play();



    }


    public void SettingsMixer(bool isMuted, ushort volumenMain, ushort volumenMusic, ushort volumenSfx)
    {


        if (isMuted == true)
        {

            mixer.SetFloat("masterVolumen", -80f);

            return;

        }

        if (volumenMusic == 10)
        {

            mixer.SetFloat("soundsVolumen", -30f);
            return;
        }

        if (volumenMusic == 0)
        {

            mixer.SetFloat("soundsVolumen", -80f);
            return;
        }

        if (volumenSfx == 0)
        {

            mixer.SetFloat("fxVolumen", -80f);
            return;
        }

        if (volumenSfx == 10)
        {

            mixer.SetFloat("fxVolumen", -30f);
            return;
        }



        mixer.SetFloat("masterVolumen", Mathf.Log10((  volumenMain - 10) * 0.01f) * 20);
        mixer.SetFloat("soundsVolumen", Mathf.Log10((volumenMusic - 10) * 0.01f) * 20 );
        mixer.SetFloat("fxVolumen", Mathf.Log10((volumenSfx - 10) * 0.01f) * 20 );



    }


    public void Mute()
    {

        isMuted = !isMuted;

        musicSource.mute = isMuted;
        sfxSource.mute = isMuted;
       
    }

    public void MuteOff()
    {
        isMuted = false;
        musicSource.mute = false;
        sfxSource.mute = false;
    }

    public void SetVolumenMaster(ushort volumenMain)
    {


        if (volumenMain == 0)
        {
            
            mixer.SetFloat("masterVolumen", -80f);
            return;
        }

        if (volumenMain == 10)
        {

            mixer.SetFloat("masterVolumen", -30f);
            return;
        }

        mixer.SetFloat("masterVolumen", Mathf.Log10((volumenMain - 10) * 0.01f) * 20);

    }

    public void SetVolumenSounds(ushort volumenMusic)
    {



        if (volumenMusic == 0)
        {

            mixer.SetFloat("soundsVolumen", -80f);
            return;
        }


        if (volumenMusic == 10)
        {

            mixer.SetFloat("soundsVolumen", -30f);
            return;
        }

        mixer.SetFloat("soundsVolumen", Mathf.Log10((volumenMusic - 10) * 0.01f) * 20);

    }

    public void SetVolumenSfx(ushort volumenSfx)
    {


        if (volumenSfx == 0)
        {

            mixer.SetFloat("fxVolumen", -80f);
            return;
        }

        if (volumenSfx == 10)
        {

            mixer.SetFloat("fxVolumen", -30f);
            return;
        }

        mixer.SetFloat("fxVolumen", Mathf.Log10((volumenSfx - 10) * 0.01f) * 20);

    }



}
