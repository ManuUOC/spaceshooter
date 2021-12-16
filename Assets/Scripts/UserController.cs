using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserController : MonoBehaviour
{
    public static UserController _user;
    public PlayerData _player;
   
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

    /// <summary>
    /// TODO: Cambiar por registro en Firebase
    /// </summary>
//    public void CrearUsuario(PlayerData pd)
//    {
//        PlayerData newPD = new PlayerData();
//    }
}
