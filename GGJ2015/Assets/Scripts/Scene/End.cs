using UnityEngine;
using System.Collections;

public class End : StaticScreen
{
    public GameObject background;

    void Start()
    {
        switch (PlayerPrefs.GetString("Winner"))
        {
            case "YellowShip": background.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load("Sprites/win_amarelo", typeof(Sprite)); break;
            case "BlueShip": background.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load("Sprites/win_azul", typeof(Sprite)); break;
            case "WhiteShip": background.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load("Sprites/win_branco", typeof(Sprite)); break;
            case "RedShip": background.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load("Sprites/win_vermelho", typeof(Sprite)); break;
        }
    }

    void Update()
    {
        ChangeScene();
    }
}