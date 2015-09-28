using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    private int itensNumber = 4;
    private float vertExtent, vertOffsetExtent, horzExtent, horzOffsetExtent;
    private GameObject powerPrefab;
    private bool couldInstantiateItem;

    public enum Scenes
    {
        Menu = 0, Game = 1, End = 2
    }

    public static string winner = string.Empty;

    void Start()
    {
        vertExtent = Camera.main.orthographicSize;
        horzExtent = vertExtent * Screen.width / Screen.height;
        horzOffsetExtent = 3.0f;
        vertOffsetExtent = 2.0f;

        powerPrefab = (GameObject)Resources.Load("Prefabs/Power");
        couldInstantiateItem = true;
    }
    
    void Update()
    {
        if (couldInstantiateItem)
        {
            StartCoroutine(InstantiateItem());
        }
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
            case 0: power.tag = "SpeedPlus"; power.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/SpeedPlus"); break;
            case 1: power.tag = "SpeedMinus"; power.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/SpeedMinus"); break;
            case 2: power.tag = "AmmoPlus"; power.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/AmmoPlus"); break;
            case 3: power.tag = "AmmoMinus"; power.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/AmmoMinus"); break;
        }

        StartCoroutine(DestroyItem(power));
        couldInstantiateItem = true;
    }

    private IEnumerator DestroyItem(GameObject power)
    {
        yield return new WaitForSeconds(2);

        if (power != null) Destroy(power);
    }
}