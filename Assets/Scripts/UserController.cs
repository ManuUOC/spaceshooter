using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UserController : MonoBehaviour
{
    public static UserController _user;
    public AudioSource _audio;
    public int NaveActual = 0;
    private void Awake()
    {
        if (_user != null && _user != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _user = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    public PlayerData ComprobarJson()
    {
        PlayerData _player = null;
        if (System.IO.File.Exists(Application.persistentDataPath + "\\data.json"))
        {
            string json = System.IO.File.ReadAllText(Application.persistentDataPath + "\\data.json");
            _player = JsonUtility.FromJson<PlayerData>(json);
        }
        return _player;
    }

    public void GuardarJson(PlayerData pd)
    {
        string jsonGuardar = JsonUtility.ToJson(pd);
        File.WriteAllText(Application.persistentDataPath + "\\data.json", jsonGuardar);
    }

}
