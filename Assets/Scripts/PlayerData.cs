﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public string Nombre;
    public int NaveActual;
    public int NaveSeleccionada;
    public int Puntuacion;
   

    public PlayerData()
    {
        Nombre = "";
        NaveActual = 0;
        NaveSeleccionada = 0;
        Puntuacion = 0;
       
    }
}
