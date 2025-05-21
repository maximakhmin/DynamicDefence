using UnityEngine;
using UnityEngine.SceneManagement;

public class StartController : MonoBehaviour
{
    SoundController sc;
    float v;

    private void Start()
    {
        SaveFileHandler.CreateNewSaveIfNotExists();
        sc = GameObject.Find("Music").GetComponent<SoundController>();
        float[] volumes = SaveManager.getVolume();
        sc.changeMusicVolume(volumes[0]);
        sc.changeSoundVolume(0.0001f);
        v = volumes[1];
    }

    public void startGame()
    {
        SceneManager.LoadScene("Menu");
        sc.changeSoundVolume(v);
        sc.playButtonSound();
    }

    public void exit()
    {
        Application.Quit();
    }
        
}
