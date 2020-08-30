using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            switch (SceneManager.GetActiveScene().name)
            {
                case "Lobby":
                    SceneManager.LoadScene("1F");
                    break;
                case "1F":
                    SceneManager.LoadScene("2F");
                    break;
                case "2F":
                    SceneManager.LoadScene("3F");
                    break;
                case "3F":
                    SceneManager.LoadScene("4F");
                    break;
                case "4F":
                    //GameManager.instance.PlayerReset();
                    SceneManager.LoadScene("GameClear");
                    break;

                default:
                    break;
            }
        }
    }
}
