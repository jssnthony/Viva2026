using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

public class TouchUI : MonoBehaviour
{
    void Awake()
    {
        EventSystem es = Object.FindFirstObjectByType<EventSystem>();
        if (es == null)
        {
            GameObject esGO = new GameObject("EventSystem");
            esGO.AddComponent<EventSystem>();
            esGO.AddComponent<InputSystemUIInputModule>();
        }

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

        CreateButton(canvasGO, handler, "LeftBtn", new Vector2(120, 80), new Vector2(0f, 0f), new Vector2(0f, 0f), 140, 100, "\u25C0", handler.OnLeftDown, null);
        CreateButton(canvasGO, handler, "RightBtn", new Vector2(280, 80), new Vector2(0f, 0f), new Vector2(0f, 0f), 140, 100, "\u25B6", handler.OnRightDown, null);
        CreateButton(canvasGO, handler, "KickBtn", new Vector2(Screen.width - 140, 80), new Vector2(1f, 0f), new Vector2(1f, 0f), 120, 100, "PATADA", null, handler.OnKick);
        CreateButton(canvasGO, handler, "JumpBtn", new Vector2(Screen.width - 280, 80), new Vector2(1f, 0f), new Vector2(1f, 0f), 120, 100, "SALTO", null, handler.OnJump);
        CreateButton(canvasGO, handler, "SlideBtn", new Vector2(Screen.width - 420, 80), new Vector2(1f, 0f), new Vector2(1f, 0f), 120, 100, "SLIDE", null, handler.OnSlide);
    }

    void CreateButton(GameObject parent, TouchInputHandler handler, string name, Vector2 pos, Vector2 anchorMin, Vector2 anchorMax, float width, float height, string label,
        System.Action onPress, System.Action onClick)
    {
        GameObject btnGO = new GameObject(name);
        btnGO.transform.SetParent(parent.transform, false);

        RectTransform rt = btnGO.AddComponent<RectTransform>();
        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
        rt.pivot = new Vector2(anchorMin.x, anchorMin.y);
        rt.anchoredPosition = pos;
        rt.sizeDelta = new Vector2(width, height);

        Image img = btnGO.AddComponent<Image>();
        img.color = new Color(0.1f, 0.1f, 0.1f, 0.6f);

        Button btn = btnGO.AddComponent<Button>();
        btn.targetGraphic = img;
        btn.transition = Selectable.Transition.ColorTint;
        ColorBlock colors = btn.colors;
        colors.normalColor = new Color(0.2f, 0.2f, 0.2f, 0.6f);
        colors.pressedColor = new Color(0.4f, 0.4f, 0.4f, 0.8f);
        colors.highlightedColor = new Color(0.3f, 0.3f, 0.3f, 0.7f);
        btn.colors = colors;

        EventTrigger trigger = btnGO.AddComponent<EventTrigger>();

        if (onPress != null)
        {
            EventTrigger.Entry pressEntry = new EventTrigger.Entry();
            pressEntry.eventID = EventTriggerType.PointerDown;
            pressEntry.callback.AddListener((data) => onPress());
            trigger.triggers.Add(pressEntry);

            EventTrigger.Entry releaseEntry = new EventTrigger.Entry();
            releaseEntry.eventID = EventTriggerType.PointerUp;
            releaseEntry.callback.AddListener((data) => handler.OnRelease());
            trigger.triggers.Add(releaseEntry);
        }

        if (onClick != null)
        {
            EventTrigger.Entry clickEntry = new EventTrigger.Entry();
            clickEntry.eventID = EventTriggerType.PointerClick;
            clickEntry.callback.AddListener((data) => onClick());
            trigger.triggers.Add(clickEntry);
        }

        GameObject textGO = new GameObject("Text");
        textGO.transform.SetParent(btnGO.transform, false);
        Text text = textGO.AddComponent<Text>();
        text.text = label;
        Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (font == null)
            font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        if (font == null)
            font = Font.CreateDynamicFontFromOSFont("Arial", 24);
        text.font = font;
        text.fontSize = 24;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;
        text.fontStyle = FontStyle.Bold;

        RectTransform textRT = textGO.GetComponent<RectTransform>();
        textRT.anchorMin = Vector2.zero;
        textRT.anchorMax = Vector2.one;
        textRT.sizeDelta = Vector2.zero;
        textRT.anchoredPosition = Vector2.zero;
    }
}
