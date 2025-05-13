using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public string[] scenes;
    public Sprite[] sprites;

    public GameObject field;
    public GameObject itemPrefab;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (Transform child in field.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < scenes.Length; i++)
        {
            GameObject gm = Instantiate(itemPrefab, field.transform);
            gm.GetComponent<MenuItem>().setPanel(scenes[i], sprites[i]);
            int temp = i;
            gm.GetComponent<Button>().onClick.AddListener(() => startScene(scenes[temp]));
        }
    }

   void startScene(string n)
   {
        SceneManager.LoadScene(n);

        GameObject gm = GameObject.Find("Music");
        if (gm) gm.GetComponent<SoundController>().playButtonSound();
    }
}
