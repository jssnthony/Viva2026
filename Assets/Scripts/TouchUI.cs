using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

public class TouchUI : MonoBehaviour
{
    private Sprite circleSprite;

    void Awake()
    {
        EventSystem es = Object.FindFirstObjectByType<EventSystem>();
        if (es == null)
        {
            GameObject esGO = new GameObject("EventSystem");
            esGO.AddComponent<EventSystem>();
            esGO.AddComponent<InputSystemUIInputModule>();
        }

        circleSprite = CreateCircleSprite();

        GameObject canvasGO = new GameObject("TouchCanvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;

        CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.matchWidthOrHeight = 0.5f;

        canvasGO.AddComponent<GraphicRaycaster>();

        TouchInputHandler handler = canvasGO.AddComponent<TouchInputHandler>();

        CreateCircleButton(canvasGO, handler.OnLeftDown, handler.OnRelease, "LeftBtn", Vector2.zero, new Vector2(100, 80), 130, "\u25C0");
        CreateCircleButton(canvasGO, handler.OnRightDown, handler.OnRelease, "RightBtn", Vector2.zero, new Vector2(260, 80), 130, "\u25B6");
        CreateCircleButton(canvasGO, handler.OnJump, null, "JumpBtn", Vector2.right, new Vector2(-150, 80), 150, "\u2B06");
        CreateCircleButton(canvasGO, handler.OnKick, null, "KickBtn", Vector2.right, new Vector2(-330, 100), 130, "\u26BD");
        CreateCircleButton(canvasGO, handler.OnSlide, null, "SlideBtn", Vector2.right, new Vector2(-330, 250), 130, "\u2B07");
    }

    Sprite CreateCircleSprite()
    {
        int size = 64;
        Texture2D tex = new Texture2D(size, size);
        Vector2 center = new Vector2(size / 2f, size / 2f);
        float radius = size / 2f - 0.5f;
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float dist = Vector2.Distance(new Vector2(x + 0.5f, y + 0.5f), center);
                tex.SetPixel(x, y, dist <= radius ? Color.white : Color.clear);
            }
        }
        tex.Apply();
        tex.filterMode = FilterMode.Bilinear;
        return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
    }

    void CreateCircleButton(GameObject parent, System.Action onDown, System.Action onUp, string name, Vector2 anchorCorner, Vector2 pos, float size, string label)
    {
        GameObject btnGO = new GameObject(name);
        btnGO.transform.SetParent(parent.transform, false);

        RectTransform rt = btnGO.AddComponent<RectTransform>();
        rt.anchorMin = anchorCorner;
        rt.anchorMax = anchorCorner;
        rt.pivot = anchorCorner;
        rt.anchoredPosition = pos;
        rt.sizeDelta = new Vector2(size, size);

        Image img = btnGO.AddComponent<Image>();
        img.sprite = circleSprite;
        img.color = new Color(1f, 1f, 1f, 0.35f);

        TouchButton tb = btnGO.AddComponent<TouchButton>();
        tb.onDown = onDown;
        tb.onUp = onUp;

        GameObject textGO = new GameObject("Text");
        textGO.transform.SetParent(btnGO.transform, false);
        Text text = textGO.AddComponent<Text>();
        text.text = label;
        text.font = GetFont();
        text.fontSize = 42;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;
        text.fontStyle = FontStyle.Bold;

        RectTransform textRT = textGO.GetComponent<RectTransform>();
        textRT.anchorMin = Vector2.zero;
        textRT.anchorMax = Vector2.one;
        textRT.sizeDelta = Vector2.zero;
        textRT.anchoredPosition = Vector2.zero;
    }

    Font GetFont()
    {
        Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (font == null) font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        if (font == null) font = Font.CreateDynamicFontFromOSFont("Arial", 24);
        return font;
    }
}
