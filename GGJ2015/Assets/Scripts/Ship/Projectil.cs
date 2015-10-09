using UnityEngine;
using System.Collections;

public class Projectil : MonoBehaviour
{
    private float speed = 10.0f;
    private Transform father;
    private Vector3 direction;
    private string shipTag;

    public float Speed
    {
        get { return speed; }
        set { if (value <= 5.0f) speed = value; }
    }

    public string ShipTag
    {
        get { return shipTag; }
        set { shipTag = value; SetupSprite(); }
    }

    void Start()
    {
        direction = GameObject.FindGameObjectWithTag(shipTag).transform.up;
    }

    void Update()
    {
        Move();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (!shipTag.Equals(other.gameObject.tag)) DestroyObject(this.gameObject);
    }

    /// <summary>
    /// Setup the sprite of the shot.
    /// </summary>
    private void SetupSprite()
    {
        switch (shipTag)
        {
            case "YellowShip": this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Shots/shoot_yellow"); break;
            case "BlueShip": this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Shots/shoot_blue"); break;
            case "WhiteShip": this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Shots/shoot_white"); break;
            case "RedShip": this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Shots/shoot_red"); break;
        }
    }
    
    /// <summary>
    /// Shot move.
    /// </summary>
    private void Move()
    {
        this.transform.Translate(direction * Time.deltaTime * speed);
    }
}