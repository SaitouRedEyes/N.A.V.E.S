using UnityEngine;
using System.Collections;

public class Power : MonoBehaviour 
{
    public Sprite[] powersSprites;

    private Effects effect;
    private int itensNumber = 4;

    public enum Effects
    {
        SpeedPlus = 0, SpeedMinus = 1, AmmoPlus = 2, AmmoMinus = 3
    }

    public Effects GetEffect
    {
        get { return effect; }
    }
	
	void Start () 
    {
        int whatPower = Random.Range(0, itensNumber);

        switch (whatPower)
        {
            case 0: effect = Effects.SpeedPlus; this.GetComponent<SpriteRenderer>().sprite = powersSprites[0]; break;
            case 1: effect = Effects.SpeedMinus; this.GetComponent<SpriteRenderer>().sprite = powersSprites[1]; break;
            case 2: effect = Effects.AmmoPlus; this.GetComponent<SpriteRenderer>().sprite = powersSprites[2]; break;
            case 3: effect = Effects.AmmoMinus; this.GetComponent<SpriteRenderer>().sprite = powersSprites[3]; break;
        }
	}
}
