using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipBase : MonoBehaviour
{
    public Ship ship;

    private List<Sprite> sprites;
    private SpriteRenderer sr;

    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>(); 
        SetupSprites();
    }

    void Update()
    {
        SetSprite(ship.Score);
    }

    private void SetupSprites()
    {
        sprites = new List<Sprite>();
        string color = "amarela";

        switch (ship.tag)
        {
            case "YellowShip": color = "amarela"; break;
            case "BlueShip": color = "azul"; break;
            case "WhiteShip": color = "branca"; break;
            case "RedShip": color = "vermelha"; break;
        }

        for (int i = 0; i < 4; i++) sprites.Add((Sprite)Resources.Load("Sprites/base-" + color + "-" + i.ToString(), typeof(Sprite)));
    }

    private void SetSprite(int damage)
    {
        sr.sprite = sprites[damage];
    }
}