using UnityEngine;

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
            return;
        }

        float moveX = 0f;
        if (leftHeld) moveX = -1f;
        if (rightHeld) moveX = 1f;
        if (leftHeld && rightHeld) moveX = 0f;

        playerCtrl.SetMoveInput(moveX);
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
