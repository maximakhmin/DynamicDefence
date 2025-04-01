using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TimeScaler : MonoBehaviour
{
    public Button[] buttons;
    private static Color offColor = new Color(0.5f, 0.5f, 0.5f, 1f);
    private static Color onColor = new Color(1f, 1f, 1f, 1f);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int temp = i;
            buttons[i].onClick.AddListener(() => setTimeScale(temp));
        }
        setTimeScale(1);
    }

    void setTimeScale(int num)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i==num)
            {
                buttons[i].enabled = false;
                buttons[i].gameObject.GetComponent<Image>().color = onColor;
            }
            else
            {
                buttons[i].enabled = true;
                buttons[i].gameObject.GetComponent<Image>().color = offColor;
            }
        }

        switch (num)
        {
            case 0:
                Time.timeScale = 0f;
                break;
            case 1:
                Time.timeScale = 1f;
                break;
            case 2:
                Time.timeScale = 2f;
                break;
            case 3:
                Time.timeScale = 4f;
                break;
        }

    }
}
