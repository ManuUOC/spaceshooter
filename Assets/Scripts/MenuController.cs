using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.IO;


public class MenuController : MonoBehaviour
{
    public GameObject[] Naves;
    private PlayerData playerD = new PlayerData();


    // Start is called before the first frame update
    void Start()
    {
        Naves[0].transform.GetChild(0).GetComponent<Image>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SeleccionarNave(int nave)
    {
        if (Naves.Length <= 0)
        {
            return;
        }
        else
        {
            for (int i = 0; i < Naves.Length; i++)
            {
                if (Naves[i] != null)
                {
                    Naves[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                }
            }
            Naves[nave].transform.GetChild(0).GetComponent<Image>().enabled = true;
            if(UserController._user != null)
            {
                UserController._user.NaveActual = nave;
            }
        }
    }

    public void GoSpace()
    {
        SceneManager.LoadScene("Game");
    }

    public void Salir()
    {
        Application.Quit();
    }
}
