using UnityEngine;
using UnityEngine.SceneManagement;

public class StartController : MonoBehaviour
{
    public void startGame()
    {
        DataBaseRepository.CreateNewSaveIfNotExists();
        SceneManager.LoadScene("Menu");

        float[] volumes = DataBaseAdapter.getVolume();
        SoundController sc = GameObject.Find("Music").GetComponent<SoundController>();
        sc.playButtonSound();
        sc.changeMusicVolume(volumes[0]);
        sc.changeSoundVolume(volumes[1]);
    }
}
