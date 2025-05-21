using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuItem : MonoBehaviour
{
    public TextMeshProUGUI levelName;
    public TextMeshProUGUI levelRecord;
    public Image levelImage;

    public void setPanel(string ln, Sprite i)
    {
        levelName.text = ln;
        int record = SaveManager.getLevelRecord(ln);
        if (record == -1)
        {
            levelRecord.text = "Record: N/A";
        }
        else
        {
            levelRecord.text = "Record: " + record;
        }

        levelImage.sprite = i;
    }

}
