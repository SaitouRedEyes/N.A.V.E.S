using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    private int itensNumber = 4;
    private float vertExtent, vertOffsetExtent, horzExtent, horzOffsetExtent;
    private GameObject powerPrefab;
    private Sprite[] powers;
    private bool couldInstantiateItem;

    public enum Scenes
    {
        Menu = 0, Game = 1, End = 2
    }

    void Start()
    {
        vertExtent = Camera.main.orthographicSize;
        horzExtent = vertExtent * Screen.width / Screen.height;
        horzOffsetExtent = 3.0f;
        vertOffsetExtent = 2.0f;

        powerPrefab = (GameObject)Resources.Load("Prefabs/Power");
        powers = new Sprite[] { Resources.Load<Sprite>("Sprites/SpeedPlus"), 
                                Resources.Load<Sprite>("Sprites/SpeedMinus"),
                                Resources.Load<Sprite>("Sprites/AmmoPlus"),
                                Resources.Load<Sprite>("Sprites/AmmoMinus")};

        couldInstantiateItem = true;
    }
    
    void Update()
    {
        if (couldInstantiateItem) StartCoroutine(InstantiateItem());
    }

    private IEnumerator InstantiateItem()
    {
        couldInstantiateItem = false;
        yield return new WaitForSeconds(3);

        GameObject power = (GameObject)Instantiate(powerPrefab, new Vector3(Random.Range(-horzExtent + horzOffsetExtent, horzExtent - horzOffsetExtent),
                                                                            Random.Range(-vertExtent + vertOffsetExtent, vertExtent - vertOffsetExtent),
                                                                            powerPrefab.transform.localPosition.z),
                                                                Quaternion.identity);

        int whatPower = Random.Range(0, itensNumber);

        switch (whatPower)
        {
            case 0: power.tag = "SpeedPlus"; power.GetComponent<SpriteRenderer>().sprite = powers[0]; break;
            case 1: power.tag = "SpeedMinus"; power.GetComponent<SpriteRenderer>().sprite = powers[1]; break;
            case 2: power.tag = "AmmoPlus"; power.GetComponent<SpriteRenderer>().sprite = powers[2]; break;
            case 3: power.tag = "AmmoMinus"; power.GetComponent<SpriteRenderer>().sprite = powers[3]; break;
        }

        StartCoroutine(DestroyItem(power));
    }

    private IEnumerator DestroyItem(GameObject power)
    {
        yield return new WaitForSeconds(2);

        if (power != null) Destroy(power);
        couldInstantiateItem = true;
    }
}