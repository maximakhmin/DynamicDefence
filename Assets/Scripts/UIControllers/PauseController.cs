using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    public Slider musicSlider;
    public Slider soundSlider;

    private bool isOpen;
    private float timeScale;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        musicSlider.onValueChanged.AddListener(delegate { changeMusicVolume(); });
        soundSlider.onValueChanged.AddListener(delegate { changeSoundVolume(); });

        float[] volumes = SaveManager.getVolume();
        musicSlider.value = volumes[0];
        soundSlider.value = volumes[1];

        changeMusicVolume();
        changeSoundVolume();

        transform.GetChild(0).gameObject.SetActive(false);
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isOpen) closePanel();
            else openPanel();
        }
    }

    void changeMusicVolume()
    {
        GameObject gm = GameObject.Find("Music");
        if (gm) gm.GetComponent<SoundController>().changeMusicVolume(musicSlider.value);
    }

    void changeSoundVolume()
    {
        GameObject gm = GameObject.Find("Music");
        if (gm) gm.GetComponent<SoundController>().changeSoundVolume(soundSlider.value);
    }

    public void openPanel()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        timeScale = Time.timeScale;
        Time.timeScale = 0f;
        isOpen = true;

        GameObject gm = GameObject.FindGameObjectWithTag("Platform");
        if (gm) gm.GetComponent<Platform>().closePanels();
        gm = GameObject.Find("EnemySpawn");
        if (gm) gm.GetComponent<EnemySpawnPanel>().closePanel();
        gm = GameObject.Find("Music");
        if (gm) gm.GetComponent<SoundController>().playButtonSound();
    }

    public void closePanel()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        Time.timeScale = timeScale;
        isOpen = false;

        SaveManager.setVolume(musicSlider.value, soundSlider.value);

        GameObject gm = GameObject.Find("Music");
        if (gm) gm.GetComponent<SoundController>().playButtonSound();
    }


    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        GameObject gm = GameObject.Find("Music");
        if (gm) gm.GetComponent<SoundController>().playButtonSound();
    }

    public void menu()
    {
        SceneManager.LoadScene("Menu");

        GameObject gm = GameObject.Find("Music");
        if (gm) gm.GetComponent<SoundController>().playButtonSound();
    }

    public void exit()
    {
        Application.Quit();

        GameObject gm = GameObject.Find("Music");
        if (gm) gm.GetComponent<SoundController>().playButtonSound();
    }



}
