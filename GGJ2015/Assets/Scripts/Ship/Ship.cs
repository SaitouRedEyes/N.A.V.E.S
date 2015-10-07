using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour
{
    public Sprite shipIdle, shipReloading;
    public GameObject spawnPoint;

    private int score;
    private float speed, axisSensibility;
    private bool couldShot, dizzy;
    private GameObject shot;
    private Vector3 movementAxis;
    private Rigidbody2D myRigidbody;
    
    public int Score
    {
        set { score = value; }
        get { return score; }
    }

    void Start()
    {
        shot = (GameObject)Resources.Load("Prefabs/Shot", typeof(GameObject));
        score = 0;
        speed = 0.05f;
        axisSensibility = 0.4f;
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

    /// <summary>
    /// Ship Shot.
    /// </summary>
    private void Shot()
    {
        if ((((Input.GetAxisRaw("FIRESHIP1") <= -0.9) || Input.GetKeyDown(KeyCode.Q)) && this.CompareTag("YellowShip")) ||
             (((Input.GetAxisRaw("FIRESHIP2") <= -0.9) || Input.GetKeyDown(KeyCode.W)) && this.CompareTag("BlueShip")) ||
             (((Input.GetAxisRaw("FIRESHIP3") <= -0.9) || Input.GetKeyDown(KeyCode.E)) && this.CompareTag("WhiteShip")) ||
             (((Input.GetAxisRaw("FIRESHIP4") <= -0.9) || Input.GetKeyDown(KeyCode.R)) && this.CompareTag("RedShip")))
        {
            StartCoroutine(Shooting());
        }
    }

    /// <summary>
    /// Ship Shooting and reloading (CoolDown).
    /// </summary>
    /// <returns></returns>
    private IEnumerator Shooting()
    {
        //Instantiate a shot
        GameObject obj = (GameObject)Instantiate(shot, new Vector3(spawnPoint.transform.position.x,
                                                                   spawnPoint.transform.position.y,
                                                                   spawnPoint.transform.position.z),
                                                                   Quaternion.identity);
        obj.tag = this.tag + "Shot";

        //Fire rate (Cooldown).
        SetSprite(shipReloading, false);
        yield return new WaitForSeconds(1);
        SetSprite(shipIdle, true);
    }

    /// <summary>
    /// Change the ship sprite and state (idle or reloading) of the ship.
    /// </summary>
    /// <param name="sprite">Sprite of the ship.</param>
    /// <param name="state">State of the ship (idle or reloading). </param>
    private void SetSprite(Sprite sprite, bool state)
    {
        this.transform.GetComponent<SpriteRenderer>().sprite = sprite;
        couldShot = state;
    }

    /// <summary>
    /// Ship Move.
    /// </summary>
    private void Move()
    {
        if (this.CompareTag("YellowShip"))     movementAxis = new Vector3(Input.GetAxis("Horizontal1"), Input.GetAxis("Vertical1"), 0);
        else if (this.CompareTag("BlueShip"))  movementAxis = new Vector3(Input.GetAxis("Horizontal2"), Input.GetAxis("Vertical2"), 0);
        else if (this.CompareTag("WhiteShip")) movementAxis = new Vector3(Input.GetAxis("Horizontal3"), Input.GetAxis("Vertical3"), 0);
        else if (this.CompareTag("RedShip"))   movementAxis = new Vector3(Input.GetAxis("Horizontal4"), Input.GetAxis("Vertical4"), 0);

        if ((movementAxis.x > axisSensibility && movementAxis.y > axisSensibility) || (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.DownArrow)))        
                MoveShip(speed, -speed, 225.0f);
        else if ((movementAxis.x > axisSensibility && movementAxis.y < -axisSensibility) || (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.UpArrow)))
                MoveShip(speed, speed, 315.0f);
        else if ((movementAxis.x < -axisSensibility && movementAxis.y < -axisSensibility) || (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.UpArrow)))
                MoveShip(-speed, speed, 45.0f);
        else if ((movementAxis.x < -axisSensibility && movementAxis.y > axisSensibility)  || (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.DownArrow)))
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