using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public string Nombre;
    public int NaveActual;

    public PlayerData(PlayerData pd)
    {
        Nombre = "";
        NaveActual = 0;
    }
}
