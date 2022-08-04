using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    [SerializeField]
    public List<MySprite> sprites = new List<MySprite>();

    [System.Serializable]
    public struct MySprite
    { 
        public List<Sprite> sprites;

        public MySprite(List<Sprite> sprites)
        {
            this.sprites = sprites;
        }
    }

    public static SpriteManager instance;

    private void Start()
    {
        instance = this;
    }

    public List<Sprite> GetPlayerSprites()
    {
        return sprites[0].sprites;
    }
}
