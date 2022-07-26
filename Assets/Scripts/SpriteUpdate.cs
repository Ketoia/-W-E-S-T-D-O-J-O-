using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteUpdate : NetworkBehaviour
{
    private List<Sprite> PlayerSprite;

    private bool IsInitialised = false;

    private SpriteRenderer Renderer;
    [SerializeField]
    private EntityState animationStates = EntityState.Idle;
    private Vector3 DeltaPosition = new Vector3(0, 0, 0);
    private Vector3 PositionCache = new Vector3(0, 0, 0);
    private int AnimationLength = 4;
    private int YFlip = 0;
    private bool XFlip = false;

    public bool IsAttacking = false;

    protected override void Start()
    {
        base.Start();
        PlayerSprite = SpriteManager.instance.GetPlayerSprites();

        Renderer = gameObject.AddComponent<SpriteRenderer>();

        IsInitialised = true;

        EventManager.StartListening(EventsTransport.GenerateSeededGuid(2), OnUpdateFrame);
    }

    protected override void OnDestroy()
    {
        Destroy(Renderer);
        EventManager.StopListening(EventsTransport.GenerateSeededGuid(2), OnUpdateFrame);
        base.OnDestroy();
    }

    private void Update()
    {
        if (!IsInitialised) return;

        ChangeAnimationsState();
    }

    private void OnUpdateFrame(object Value)
    {
        int Framu = (int)Value;
        int CurrentState = YFlip * AnimationLength + (int)animationStates * AnimationLength * 2;
        Renderer.sprite = PlayerSprite[CurrentState + (Framu % AnimationLength)];

    }

    private void ChangeAnimationsState()
    {
        if (IsAttacking)
        {
            animationStates = EntityState.Attack;
            return;
        }

        DeltaPosition = transform.position - PositionCache;
        PositionCache = transform.position;
        Vector2 direction = ((Vector2)DeltaPosition).normalized;

        if(direction == Vector2.zero)
        {
            animationStates = EntityState.Idle;
        }
        else
        {
            animationStates = EntityState.Walk;
            UpdateAnimationDirection(direction);
        }
    }

    void UpdateAnimationDirection(Vector2 direction)
    {
        if (direction.x > 0) //Right
        {
            if(XFlip)
            {
                Renderer.flipX = false;
            }
            XFlip = false;
        }
        else if (direction.x < 0) //Left
        {
            if (!XFlip)
            {
                Renderer.flipX = true;
            }
            XFlip = true;
        }

        if (direction.y > 0) //Up
        {
            YFlip = 1;
        }
        else if (direction.y < 0) //Down
        {
            YFlip = 0;
        }
    }

    private enum EntityState
    {
        Idle,
        Attack,
        Walk,
        Die,
        Vanish,
        Start
    }
}
