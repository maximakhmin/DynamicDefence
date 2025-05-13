using UnityEngine;
using UnityEngine.Audio;

public class SoundController : MonoBehaviour
{

    public AudioSource enemyDeath;
    public AudioSource button;
    public AudioSource upgrade;
    public AudioSource baseDamage;

    public AudioMixer audioMixer;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void playEmenyDeathSound()
    {
        enemyDeath.Play();
    }

    public void playButtonSound()
    {
        button.Play();
    }

    public void playUpgradeSound()
    {
        upgrade.Play();
    }
    public void playBaseDamageSound()
    {
        baseDamage.Play();
    }

    public void changeMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }

    public void changeSoundVolume(float volume)
    {
        audioMixer.SetFloat("SoundVolume", Mathf.Log10(volume) * 20);
    }

}
