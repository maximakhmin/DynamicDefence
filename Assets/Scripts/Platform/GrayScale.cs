using UnityEngine;
using UnityEngine.UI;

public class GrayScale : MonoBehaviour
{
    private Color color;
    private Color grayColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        color = GetComponent<RawImage>().color;
        float gray = (76.2195f * color.r + 149.685f * color.g + 29.07f * color.b) / 255;
        grayColor = new Color(gray, gray, gray);
    }

    public void setGray()
    {
        GetComponent<RawImage>().color = grayColor;
        transform.GetChild(0).GetComponent<RawImage>().color = grayColor;
    }

    public void setColor()
    {
        GetComponent<RawImage>().color = color;
        transform.GetChild(0).GetComponent<RawImage>().color = color;
    }

}
