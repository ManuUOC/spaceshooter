using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.IO;


public class MenuController : MonoBehaviour
{
    public TMP_InputField InputNombreUsuario;
    public GameObject[] Naves;
    private  PlayerData playerD = new PlayerData();
    

    // Start is called before the first frame update
    void Start()
    {

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
            playerD.NaveSeleccionada = nave;
        }
    }

    public void GoSpace()
    {
        if(InputNombreUsuario != null)
        {
            if (!string.IsNullOrEmpty((InputNombreUsuario.text)) && playerD.NaveSeleccionada != -1)
            {
                string json = JsonUtility.ToJson(playerD);
                File.WriteAllText(Application.persistentDataPath + "\\data.json", json); //Application.persistentDataPath
                SceneManager.LoadScene("Game");
            }
        }     
    }
}
