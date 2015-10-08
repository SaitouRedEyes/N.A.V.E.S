using UnityEngine;
using System.Collections;

public class Projectil : MonoBehaviour
{
    private float speed;
    private Transform father;
    private Vector3 direction;
    private string shipTag;

    public string ShipTag
    {
        get { return shipTag; }
        set { shipTag = value; }
    }

    void Start()
    {
        speed = 10.0f;
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
    /// Shot move.
    /// </summary>
    private void Move()
    {
        this.transform.Translate(direction * Time.deltaTime * speed);
    }
}