using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonExtension : MonoBehaviour
{

    public Text text;
    public Image icon;
    public Vector3 pressedOffset;
    public Color pressedColor;
    public Color highlightColor;
    public Color disabledColor;
    public Color normalColor;
    public Color buttonColor = Color.white;


    private Button b;
    private Image image;
    private Vector3 iconBasePosition;
    private Vector3 textBasePosition;
    private Texture pressedTexture;
    private Texture highlightTexture;
    private Texture disabledTexture;
    private Texture previous;

    // Use this for initialization
    void Start()
    {
        b = GetComponent<Button>();
        image = GetComponent<Image>();

        pressedTexture = b.spriteState.pressedSprite.texture;
        highlightTexture = b.spriteState.highlightedSprite.texture;
        disabledTexture = b.spriteState.disabledSprite.texture;

        if (text != null)
            textBasePosition = text.transform.localPosition;

        if (icon != null)
            iconBasePosition = icon.transform.localPosition;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Texture t = b.targetGraphic.mainTexture;
        if (previous == t) return;
        SetCorrectTexture();
        previous = t;
    }

    public void SetCorrectTexture()
    {
        Texture t = b.targetGraphic.mainTexture;
        if (t == disabledTexture)
        {
            SetDisabled();
        }
        else if (t == pressedTexture)
        {
            SetPressed();
        }
        else if (t == highlightTexture)
        {
            SetHighlighted();
        }
        else
        {
            SetNormal();
        }
    }

    void SetDisabled()
    {
        if (text != null)
        {
            text.transform.localPosition = textBasePosition;
            text.color = disabledColor;
        }
        if (icon != null)
        {
            icon.transform.localPosition = iconBasePosition;
            icon.color = disabledColor;
        }

        image.color = buttonColor;
    }

    void SetPressed()
    {
        if (text != null)
        {
            text.transform.localPosition = textBasePosition + pressedOffset;
            text.color = pressedColor;
        }
        if (icon != null)
        {
            icon.transform.localPosition = iconBasePosition + pressedOffset;
            icon.color = pressedColor;
        }

        image.color = Color.white;
    }

    void SetHighlighted()
    {
        if (text != null)
        {
            text.transform.localPosition = textBasePosition;
            text.color = highlightColor;
        }
        if (icon != null)
        {
            icon.transform.localPosition = iconBasePosition;
            icon.color = highlightColor;
        }

        image.color = Color.white;
    }

    void SetNormal()
    {
        if (text != null)
        {
            text.transform.localPosition = textBasePosition;
            text.color = normalColor;
        }
        if (icon != null)
        {
            icon.transform.localPosition = iconBasePosition;
            icon.color = normalColor;
        }

        image.color = buttonColor;
    }
}
