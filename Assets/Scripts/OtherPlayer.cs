using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayer : MonoBehaviour
{
    protected GameObject PlayersText;
    PlayerData playerData;

    AnimationMainState AnimationSate = AnimationMainState.Stay;

    public void InitialGameObject(PlayerData parameter, Sprite sprite)
    {
        playerData = parameter;
        //initialise Listeners
        EventManager.StartListening("PlayerPos_" + playerData.SteamID.ToString(), SetPosition);
        EventManager.StartListening("PlayerMainAnimation_" + playerData.SteamID.ToString(), SetAnimationMainState);


        gameObject.AddComponent<SpriteRenderer>().sprite = sprite;
        
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

    void SetAnimationMainState(object AnimationEnum)
    {
        AnimationSate = (AnimationMainState)AnimationEnum;
    }
    void SetAnimationMinorState(object AnimationEnum)
    {

    }
}