using UnityEngine;
using UnityEngine.InputSystem;

public class TouchInputHandler : MonoBehaviour
{
    private PlayerController playerCtrl;
    private PlayerKick playerKick;

    private bool leftHeld;
    private bool rightHeld;

    void Start()
    {
        FindPlayer();
    }

    void FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerCtrl = player.GetComponent<PlayerController>();
            playerKick = player.GetComponent<PlayerKick>();
        }
    }

    void Update()
    {
        if (playerCtrl == null)
        {
            FindPlayer();
            if (playerCtrl == null) return;
        }

        float moveX = 0f;
        if (leftHeld) moveX = -1f;
        if (rightHeld) moveX = 1f;
        if (leftHeld && rightHeld) moveX = 0f;
        playerCtrl.SetMoveInput(moveX);

        var touch = Touchscreen.current;
        if (touch != null)
        {
            var touchState = touch.primaryTouch;
            if (touchState.press.wasPressedThisFrame)
            {
                Vector2 pos = touchState.position.ReadValue();
                HandleTouchPress(pos);
            }
        }
    }

    void HandleTouchPress(Vector2 touchPos)
    {
        float halfScreen = Screen.width * 0.5f;

        if (touchPos.x < halfScreen)
        {
            return;
        }

        float h = Screen.height;
        float jumpZone = h * 0.35f;
        float kickZone = h * 0.65f;

        if (touchPos.y < jumpZone)
        {
            if (playerCtrl != null) playerCtrl.DoJump();
        }
        else if (touchPos.y < kickZone)
        {
            if (playerKick != null) playerKick.DoKick();
        }
        else
        {
            if (playerCtrl != null) playerCtrl.DoSlide();
        }
    }

    public void OnLeftDown()
    {
        leftHeld = true;
    }

    public void OnRightDown()
    {
        rightHeld = true;
    }

    public void OnRelease()
    {
        leftHeld = false;
        rightHeld = false;
    }

    public void OnJump()
    {
        if (playerCtrl != null)
            playerCtrl.DoJump();
    }

    public void OnSlide()
    {
        if (playerCtrl != null)
            playerCtrl.DoSlide();
    }

    public void OnKick()
    {
        if (playerKick != null)
            playerKick.DoKick();
    }
}
