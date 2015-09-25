using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour
{
    private int score;
    private float speed, axisSensibility;
    private bool couldShot, dizzy;
    public GameObject spawnPoint, shot;
    private Vector3 movementAxis;
    private Rigidbody2D myRigidbody;

    public int Score
    {
        set { score = value; }
        get { return score; }
    }

    void Start()
    {
        score = 0;
        speed = 0.05f;
        axisSensibility = 0.4f;
        shot = (GameObject)Resources.Load("Prefabs/Shot", typeof(GameObject));
        couldShot = true;
        dizzy = false;
        myRigidbody = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (couldShot && !dizzy) Shot();
        if (!dizzy) Move();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.tag.Equals("Untagged") && other.gameObject.tag.Substring(other.gameObject.tag.Length - 4).Equals("Shot"))
        {
            string shipTag = other.gameObject.tag.Remove(other.gameObject.tag.Length - 4);

            if (!shipTag.Equals(this.tag))
            {
                Ship currShip = GameObject.FindGameObjectWithTag(shipTag).GetComponent<Ship>();
                currShip.Score += 1;

                if (currShip.Score >= 3)
                {
                    GameController.winner = shipTag;
                    Application.LoadLevel((int)GameController.Scenes.End);
                }
            }
        }
        else if (other.gameObject.tag.Equals("AsteroidsBelt")) StartCoroutine(KnockBack());
    }

    private void Shot()
    {
        if (((Input.GetAxisRaw("FIRESHIP1") <= -0.9) || Input.GetKeyDown(KeyCode.Q)) && this.tag.Equals("YellowShip"))
        {
            InstantiateShot();
            StartCoroutine(Cooldown("Sprites/nave-amarela", "Sprites/nave-amarela-ready"));
        }
        else if (((Input.GetAxisRaw("FIRESHIP2") <= -0.9) || Input.GetKeyDown(KeyCode.W)) && this.tag.Equals("BlueShip"))
        {
            InstantiateShot();
            StartCoroutine(Cooldown("Sprites/nave-azul", "Sprites/nave-azul-ready"));
        }
        else if (((Input.GetAxisRaw("FIRESHIP3") <= -0.9) || Input.GetKeyDown(KeyCode.E)) && this.tag.Equals("WhiteShip"))
        {
            InstantiateShot();
            StartCoroutine(Cooldown("Sprites/nave-branca", "Sprites/nave-branca-ready"));
        }
        else if (((Input.GetAxisRaw("FIRESHIP4") <= -0.9) || Input.GetKeyDown(KeyCode.R)) && this.tag.Equals("RedShip"))
        {
            InstantiateShot();
            StartCoroutine(Cooldown("Sprites/nave-vermelha", "Sprites/nave-vermelha-ready"));
        }
    }

    private void Move()
    {
        if (this.tag.Equals("YellowShip"))     movementAxis = new Vector3(Input.GetAxis("Horizontal1"), Input.GetAxis("Vertical1"), 0);
        else if (this.tag.Equals("BlueShip"))  movementAxis = new Vector3(Input.GetAxis("Horizontal2"), Input.GetAxis("Vertical2"), 0);
        else if (this.tag.Equals("WhiteShip")) movementAxis = new Vector3(Input.GetAxis("Horizontal3"), Input.GetAxis("Vertical3"), 0);
        else if (this.tag.Equals("RedShip"))   movementAxis = new Vector3(Input.GetAxis("Horizontal4"), Input.GetAxis("Vertical4"), 0);

        if ((movementAxis.x > axisSensibility && movementAxis.y > axisSensibility) ||
            (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.DownArrow)))        
                MoveShip(speed, -speed, 225.0f);
        else if ((movementAxis.x > axisSensibility && movementAxis.y < -axisSensibility) ||
            (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.UpArrow)))
                MoveShip(speed, speed, 315.0f);
        else if ((movementAxis.x < -axisSensibility && movementAxis.y < -axisSensibility) ||
            (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.UpArrow)))
                MoveShip(-speed, speed, 45.0f);
        else if ((movementAxis.x < -axisSensibility && movementAxis.y > axisSensibility)  ||
            (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.DownArrow)))
                MoveShip(-speed, -speed, 135.0f);
        else if ((movementAxis.x > axisSensibility) || Input.GetKey(KeyCode.RightArrow))
                MoveShip(speed, 0, 270.0f);
        else if ((movementAxis.x < -axisSensibility) || Input.GetKey(KeyCode.LeftArrow)) 
                MoveShip(-speed, 0, 90.0f);
        else if ((movementAxis.y > axisSensibility) || Input.GetKey(KeyCode.DownArrow)) 
                MoveShip(0, -speed, 180.0f);
        else if ((movementAxis.y < -axisSensibility) || Input.GetKey(KeyCode.UpArrow)) 
                MoveShip(0, speed, 0.0f);
    }

    private void MoveShip(float speedX, float speedY, float rotationAngle)
    {
        this.transform.localPosition = new Vector3(this.transform.localPosition.x + speedX,
                                                   this.transform.localPosition.y + speedY,
                                                   this.transform.localPosition.z);

        this.transform.localEulerAngles = new Vector3(this.transform.localRotation.x,
                                                        this.transform.localRotation.y,
                                                        rotationAngle);
    }

    private void InstantiateShot()
    {
        GameObject obj = (GameObject)Instantiate(shot, new Vector3(spawnPoint.transform.position.x,
                                                                   spawnPoint.transform.position.y,
                                                                   spawnPoint.transform.position.z),
                                                                   Quaternion.identity);
        obj.tag = this.tag + "Shot";
    }

    private IEnumerator Cooldown(string path, string pathReady)
    {
        SetSprite(path);
        couldShot = false;

        yield return new WaitForSeconds(1);

        SetSprite(pathReady);        
        couldShot = true;
    }

    private void SetSprite(string path)
    {
        this.transform.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load(path, typeof(Sprite));
    }

    private IEnumerator KnockBack()
    {
        dizzy = true;
        myRigidbody.AddForceAtPosition(Vector2.one * 4.0f, Vector2.zero);
        myRigidbody.freezeRotation = false;

        if (score > 0) score--;

        yield return new WaitForSeconds(2);

        dizzy = false;
        myRigidbody.freezeRotation = true;
        myRigidbody.velocity = Vector3.zero;
    }
}