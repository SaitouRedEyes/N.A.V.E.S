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
        if (other.gameObject.GetComponent<Projectil>()) DamageResult(other);
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
    /// Ship shooting and reloading (CoolDown).
    /// </summary>
    /// <returns></returns>
    private IEnumerator Shooting()
    {
        //Instantiate a shot
        GameObject obj = (GameObject)Instantiate(shot, new Vector3(spawnPoint.transform.position.x,
                                                                   spawnPoint.transform.position.y,
                                                                   spawnPoint.transform.position.z),
                                                                   Quaternion.identity);
        obj.GetComponent<Projectil>().ShipTag = this.tag;
        
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

    /// <summary>
    /// Move and rotate the ship in function of the player input.
    /// </summary>
    /// <param name="speedX"> speed on X-axis </param>
    /// <param name="speedY"> speed on Y-axis </param>
    /// <param name="rotationAngle"> the angle of the ship </param>
    private void MoveShip(float speedX, float speedY, float rotationAngle)
    {
        this.transform.localPosition = new Vector3(this.transform.localPosition.x + speedX,
                                                   this.transform.localPosition.y + speedY,
                                                   this.transform.localPosition.z);

        this.transform.localEulerAngles = new Vector3(this.transform.localRotation.x,
                                                        this.transform.localRotation.y,
                                                        rotationAngle);
    }

    /// <summary>
    /// The result of the shot x ship collision.
    /// </summary>
    /// <param name="other"> the shot </param>
    private void DamageResult(Collision2D other)
    {
        string shooterShipTag = other.gameObject.GetComponent<Projectil>().ShipTag;
        
        if (!shooterShipTag.Equals(this.tag))
        {
            Ship currShip = GameObject.FindGameObjectWithTag(shooterShipTag).GetComponent<Ship>();
            currShip.Score += 1;

            if (currShip.Score >= 3)
            {
                PlayerPrefs.SetString("Winner", shooterShipTag);
                Application.LoadLevel((int)GameController.Scenes.End);
            }
        }
    }

    /// <summary>
    /// If the player collides with asteroids belt, stun and knock back.
    /// </summary>
    /// <returns></returns>
    private IEnumerator KnockBack()
    {
        dizzy = true;
        myRigidbody.AddForceAtPosition(Vector2.one * 4.0f, Vector2.zero);
        myRigidbody.freezeRotation = false;

        if (score > 0) score--;

        yield return new WaitForSeconds(1);

        dizzy = false;
        myRigidbody.freezeRotation = true;
        myRigidbody.velocity = Vector3.zero;
    }
}