using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkSpirte
{
    private List<Sprite> PlayerSprite;
    private SpriteRenderer Renderer;
    
    private int AnimationLength = 4;

    private Vector3 _velocity;
    public Vector3 velocity
    {
        get 
        { 
            return _velocity; 
        }
        set 
        { 
            _velocity = value;

            if (value.x > 0)
                Renderer.flipX = false;
            else if(value.x < 0)
                Renderer.flipX = true;

            if (value.y > 0)
                AnimationYFlip = true;
            else if (value.y < 0)
                AnimationYFlip = false;

            //if (CanChangeAnimation())
                ChangeAnimationsState();
        }
    }

    private bool AnimationYFlip = false;

    public EntityAnimationState entityState;

    public NetworkSpirte(GameObject parent)
    {
        GameObject gameObject = new GameObject("Sprite");
        gameObject.transform.SetParent(parent.transform, false);

        PlayerSprite = SpriteManager.instance.GetPlayerSprites();

        Renderer = gameObject.AddComponent<SpriteRenderer>();

        EventManager.StartListening(EventsTransport.GenerateSeededGuid((int)SingleEventsSeed.UpdateFrame), UpdateFrame);
    }

    //public void Init(GameObject parent)
    //{
    //    GameObject gameObject = new GameObject("Sprite");
    //    gameObject.transform.SetParent(parent.transform, false);

    //    PlayerSprite = SpriteManager.instance.GetPlayerSprites();

    //    Renderer = gameObject.AddComponent<SpriteRenderer>();

    //    EventManager.StartListening(EventsTransport.GenerateSeededGuid((int)SingleEventsSeed.UpdateFrame), UpdateFrame);
    //}

    public void OnDestroy()
    {        
        EventManager.StopListening(EventsTransport.GenerateSeededGuid((int)SingleEventsSeed.UpdateFrame), UpdateFrame);
    }

    public void StartAttacking()
    {
        entityState = EntityAnimationState.Attack;
    }

    public void StopAttacking()
    {
        ChangeAnimationsState();
    }

    public void StartDying()
    {
        entityState = EntityAnimationState.Dieing;
    }

    public void StopDying()
    {
        entityState = EntityAnimationState.Dead;
    }

    private void UpdateFrame(object Value)
    {
        //if (!CanChangeAnimation()) return;

        int Framu = (int)Value;
        int CurrentState = (AnimationYFlip ? AnimationLength : 0) + (int)entityState * AnimationLength;
        Renderer.sprite = PlayerSprite[CurrentState + (Framu % AnimationLength)];
    }

    private bool CanChangeAnimation()
    {
        if (entityState == EntityAnimationState.Walk ||
            entityState == EntityAnimationState.Idle) return true;
        return false;
    }

    private void ChangeAnimationsState()
    {
        if (velocity.x == 0 && velocity.y == 0)
        {
            entityState = EntityAnimationState.Idle;
        }
        else
        {
            entityState = EntityAnimationState.Walk;
        }
    }

    public enum EntityAnimationState
    {
        Idle = 0,
        Attack = 2,
        Walk = 4,
        Jump = 6,
        Dieing = 8,
        Dead = 9
    }
}