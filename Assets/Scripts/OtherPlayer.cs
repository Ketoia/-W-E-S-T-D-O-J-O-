using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayer : MonoBehaviour
{
    protected GameObject PlayersText;
    PlayerData playerData;
    public List<Sprite> PlayerSprite;
    SpriteRenderer spriteRenderer;

    //animations
    public int AnimationLength = 4;

    //variables to sync
    bool IsAtacking = false;
    bool IsDead = false;
    bool IsJumping = false;
    int CurrentFrame = 0;


    public void InitialGameObject(PlayerData parameter, List<Sprite> sprites)
    {
        playerData = parameter;
        //initialise Listeners
        EventManager.StartListening("PlayerPos_" + playerData.SteamID.ToString(), SetPosition);
        EventManager.StartListening("PlayerCurrentFrame_" + Steamworks.SteamClient.SteamId, SetAnimationState);
        EventManager.StartListening("PlayerXFlip_" + Steamworks.SteamClient.SteamId, SetXFlip);
        EventManager.StartListening("TimeFrameRateTick", SetFrame);

        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        PlayerSprite = sprites;
        spriteRenderer.sprite = sprites[0];
        
        //initialise Player's Names
        PlayersText = new GameObject("PlayersText", typeof(TextMesh));
        PlayersText.transform.parent = transform;
        PlayersText.transform.position = new Vector3(0, -0.75f);
        TextMesh textmesh = PlayersText.GetComponent<TextMesh>();
        textmesh.text = playerData.Name;
        textmesh.anchor = TextAnchor.MiddleCenter;
        textmesh.characterSize = 0.2f;
        textmesh.fontSize = 20;
    }

    void SetPosition(object Pos)
    {
        Vector3S Position = (Vector3S)Pos;
        transform.position = Position;
    }

    void SetXFlip(object Xflip)
    {
        spriteRenderer.flipX = (bool)Xflip;
        SetFrame(GameplayManager.instance.TimeFrame);
    }

    void SetAnimationState(object CurFramu)
    {
        CurrentFrame = (int)CurFramu;
        SetFrame(GameplayManager.instance.TimeFrame);
    }

    void SetFrame(object obj)
    {
        int Framu = (int)obj;
        spriteRenderer.sprite = PlayerSprite[CurrentFrame + (Framu % AnimationLength)];
    }
}