using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipBase : MonoBehaviour
{
    private List<Sprite> sprites;
    public GameObject ship;

    void Start()
    {
        SetupSprites();
    }

    void Update()
    {
        SetSprite(ship.GetComponent<Ship>().Score);
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

        for (int i = 0; i < 4; i++)
        {
            sprites.Add((Sprite)Resources.Load("Sprites/base-" + color + "-" + i.ToString(), typeof(Sprite)));
        }
    }

    private void SetSprite(int damage)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = sprites[damage];
    }
}