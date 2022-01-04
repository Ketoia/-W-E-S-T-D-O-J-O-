using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<Sprite> PlayerSprite;

    Vector3S Position;
    new Rigidbody2D rigidbody;
    EventsTransport PositionEvent;
    SpriteRenderer spriteRenderer;

    //animations
    public int AnimationLength = 4;

    AnimationMainState animationMainState = AnimationMainState.Stay;
    AnimationMinorState MinorAttack = AnimationMinorState.Pre;

    EventsTransport PlayerCurrentFrame;
    EventsTransport PlayerAnimationsXFlip;

    private int Direction = 0;
    Vector2 dir = Vector2.zero;

    //variables to sync
    bool XFlip = false;
    bool IsAtacking = false;
    bool IsDead = false;
    bool IsJumping = false;
    int CurrentFrame = 0;

    void Awake()
    {
        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = PlayerSprite[0];
        rigidbody = gameObject.AddComponent<Rigidbody2D>();
        rigidbody.gravityScale = 0;

        PositionEvent = new EventsTransport("PlayerPos_" + Steamworks.SteamClient.SteamId);
        PlayerCurrentFrame = new EventsTransport("PlayerCurrentFrame_" + Steamworks.SteamClient.SteamId);
        PlayerAnimationsXFlip = new EventsTransport("PlayerXFlip_" + Steamworks.SteamClient.SteamId);

        EventManager.StartListening("TimeFrameRateTick", UpdateFrame);
    }

    void FixedUpdate()
    {
        rigidbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * 10;
        rigidbody.velocity = dir * 10;
        UpdateDirection();
        if (Position != gameObject.transform.position)
        {
            Position = gameObject.transform.position;
            PositionEvent.Object = Position;
            PositionEvent.AutomaticEventSend();
        }
    }

    private void UpdateDirection()
    {
        dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if (dir.x != 0 || dir.y != 0)
        {
            if (dir.x > 0 && dir.y <= 0)
            {
                XFlip = false;
                Direction = 0;
            }
            else if (dir.x > 0 && dir.y > 0)
            {
                XFlip = false;
                Direction = 1;
            }
            else if (dir.x <= 0 && dir.y > 0)
            {
                XFlip = true;
                Direction = 1;
            }
            else
            {
                XFlip = true;                                
                Direction = 0;
            }

            UpdateXFlip();

            if (animationMainState != AnimationMainState.Walk)
            {
                animationMainState = AnimationMainState.Walk;
                UpdateAnimationState();
            }
        }
        else
        {
            if (animationMainState != AnimationMainState.Stay)
            {
                animationMainState = AnimationMainState.Stay;
                UpdateAnimationState();
            }

        }
    }

    #region Animations

    void UpdateXFlip()
    {
        if (XFlip != spriteRenderer.flipX)
        {
            spriteRenderer.flipX = XFlip;
            PlayerAnimationsXFlip.Object = XFlip;
            PlayerAnimationsXFlip.AutomaticEventSend();
            UpdateAnimationState();
        }
    }

    void UpdateAnimationState()
    {
        CurrentFrame = Direction * AnimationLength + (int)animationMainState * AnimationLength * 2;
        UpdateFrame(GameplayManager.instance.TimeFrame);

        PlayerCurrentFrame.Object = CurrentFrame;
        PlayerCurrentFrame.AutomaticEventSend();
    }

    void UpdateFrame(object obj)
    {
        int Framu = (int)obj;
        spriteRenderer.sprite = PlayerSprite[CurrentFrame + (Framu % AnimationLength)];
    }
    #endregion
}
