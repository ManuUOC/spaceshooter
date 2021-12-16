using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GestorSplash : MonoBehaviour
{
    public void Start()
    {
        Invoke("GoMenu", 3f);
    }
    public void GoMenu()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }
}
