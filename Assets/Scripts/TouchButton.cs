using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TouchButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public System.Action onDown;
    public System.Action onUp;

    private Image img;
    private Color normalColor;
    private Color pressedColor;

    void Awake()
    {
        img = GetComponent<Image>();
        if (img != null)
        {
            normalColor = img.color;
            pressedColor = new Color(img.color.r + 0.2f, img.color.g + 0.2f, img.color.b + 0.2f, img.color.a + 0.2f);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (img != null) img.color = pressedColor;
        if (onDown != null) onDown();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (img != null) img.color = normalColor;
        if (onUp != null) onUp();
    }
}
