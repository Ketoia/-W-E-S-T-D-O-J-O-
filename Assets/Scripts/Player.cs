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

    public Material PlayerOutline;
    //animations
    public int AnimationLength = 4;

    AnimationMainState animationMainState = AnimationMainState.Stay;
    AnimationMinorState animationAttackState = AnimationMinorState.Pre;

    EventsTransport PlayerCurrentFrame;
    EventsTransport PlayerAnimationsXFlip;

    private int Direction = 0;
    Vector2 dir = Vector2.zero;

    //variables to sync
    bool XFlip = false;
    bool IsAtacking = false;
    //bool IsDead = false;
    //bool IsJumping = false;
    int CurrentFrame = 0;

    void Awake()
    {
        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = PlayerSprite[0];
        spriteRenderer.material = PlayerOutline;
        rigidbody = gameObject.AddComponent<Rigidbody2D>();
        rigidbody.gravityScale = 0;

        //PositionEvent = new EventsTransport("PlayerPos_" + Steamworks.SteamClient.SteamId);
        //PlayerCurrentFrame = new EventsTransport("PlayerCurrentFrame_" + Steamworks.SteamClient.SteamId);
        //PlayerAnimationsXFlip = new EventsTransport("PlayerXFlip_" + Steamworks.SteamClient.SteamId);

        EventManager.StartListening(EventsTransport.GenerateSeededGuid(2), UpdateFrame);
    }

    private void Update()
    {
        MovementFSM();
    }

    void MovementFSM()
    {
        if (IsAtacking) return;
        dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        //CheckAttack
        if(dir == Vector2.zero)
        {
            ChangeAnimationState(AnimationMainState.Stay);
        }
        else
        {
            ChangeAnimationState(AnimationMainState.Walk);

            if (Input.GetMouseButtonDown(0))
            {
                IsAtacking = true;
                ChangeAnimationState(AnimationMainState.Attack);
            }
        }

    }

    void FixedUpdate()
    {
        UpdateMovementFSM();
    }

    void UpdateMovementFSM()
    {
        switch(animationMainState)
        {
            case AnimationMainState.Stay:
                rigidbody.velocity = Vector2.zero;
                break;

            case AnimationMainState.Walk:
                UpdateDirection();
                UpdateMovement();
                break;

            case AnimationMainState.Attack:
                UpdateAttack();
                break;

            case AnimationMainState.Die:

                break;
        }
    }

    #region Attack
    void UpdateAttack()
    {
        switch (animationAttackState)
        {
            case AnimationMinorState.Pre:
                animationAttackState = AnimationMinorState.Curr;
                break;

            case AnimationMinorState.Curr:
                CurrAttack();
                break;

            case AnimationMinorState.Post:
                animationAttackState = AnimationMinorState.Pre;
                ChangeAnimationState(AnimationMainState.Stay);
                IsAtacking = false;
                break;
        }
    }

    float time = 2;
    float dtime = 0;

    void CurrAttack()
    {
        if (dtime > time)
        {
            animationAttackState = AnimationMinorState.Post;
            dtime = 0;
        }
        else
        {
            UpdateMovement();
            dtime += Time.deltaTime;
        }
    }
    #endregion


    void UpdateMovement()
    {
        rigidbody.velocity = dir * 5;

        if (Position != gameObject.transform.position)
        {
            Position = gameObject.transform.position;
            PositionEvent.Value = Position;
        }
    }

    void UpdateDirection()
    {
        if (dir.x > 0) //Right
        {
            XFlip = false;
            UpdateXFlip();
        }
        else if (dir.x < 0) //Left
        {
            XFlip = true;
            UpdateXFlip();
        }

        if (dir.y > 0) //Up
        {
            if (Direction != 1)
            {
                Direction = 1;
                UpdateAnimationState();
            }
        }
        else if (dir.y < 0) //Down
        {
            if (Direction != 0)
            {
                Direction = 0;
                UpdateAnimationState();
            }
        }
    }

    #region Animations

    void UpdateXFlip()
    {
        if (spriteRenderer.flipX == XFlip) return;
        spriteRenderer.flipX = XFlip;
        //PlayerAnimationsXFlip.Value = XFlip;
        UpdateAnimationState();        
    }
    
    void ChangeAnimationState(AnimationMainState state)
    {
        if (animationMainState != state)
        {
            animationMainState = state;
            UpdateAnimationState();
        }
    }

    void UpdateAnimationState()
    {
        CurrentFrame = Direction * AnimationLength + (int)animationMainState * AnimationLength * 2;
        UpdateFrame(GameplayManager.instance.TimeFrame);

        //PlayerCurrentFrame.Value = CurrentFrame;
    }

    void UpdateFrame(object obj)
    {
        int Framu = (int)obj;
        spriteRenderer.sprite = PlayerSprite[CurrentFrame + (Framu % AnimationLength)];
    }
    #endregion
}
