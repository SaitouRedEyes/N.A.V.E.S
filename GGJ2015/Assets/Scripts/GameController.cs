using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    private float vertExtent, vertOffsetExtent, horzExtent, horzOffsetExtent;
    private GameObject powerPrefab;
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

        StartCoroutine(DestroyItem(power));
    }

    private IEnumerator DestroyItem(GameObject power)
    {
        yield return new WaitForSeconds(2);

        if (power != null) Destroy(power);
        couldInstantiateItem = true;
    }
}