using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public abstract class StaticScreen : MonoBehaviour
{
    protected void ChangeScene()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 clickPos = new Vector2(wp.x, wp.y);

            if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(clickPos)) Change();
        }
        else if (Input.GetButton("Start") || Input.GetButton("Start2") || Input.GetButton("Start3") || Input.GetButton("Start4")) Change();
    }

    private void Change()
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case (int)GameController.Scenes.Menu: Application.LoadLevel((int)GameController.Scenes.Game); break;
            case (int)GameController.Scenes.End: Application.LoadLevel((int)GameController.Scenes.Menu); break;
        }
    }
}