using UnityEngine;
using System.Collections;

public class Shot : MonoBehaviour
{
    private float speed;
    private Transform father;
    private Vector3 direction;
    private string shipTag;

    void Start()
    {
        speed = 10.0f;
        shipTag = this.tag.Remove(this.tag.Length - 4);
        direction = GameObject.FindGameObjectWithTag(shipTag).transform.up;
    }

    void Update()
    {
        Move();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(!this.tag.Remove(this.tag.Length - 4).Equals(other.gameObject.tag)) DestroyObject(this.gameObject);
    }

    private void Move()
    {
        this.transform.Translate(direction * Time.deltaTime * speed);
    }
}