using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    private int paso;
    public Animator anim;
    public void ClipSiguiente()
    {
        paso++;
        if (paso >= 4)
        {
            paso = 4;
        }
        Ejecutar();
    }
    public void ClipAnterior()
    {
        paso--;
        if (paso <= 0)
        {
            paso = 0;
        }
        Ejecutar();
    }

    private void Ejecutar()
    {
        if (anim != null)
        {
            switch (paso)
            {
                case 0:
                    anim.Play("HowToPlay");
                    break;
                case 1:
                    anim.Play("Disparo");
                    break;
                case 2:
                    anim.Play("Daño");
                    break;
                case 3:
                    anim.Play("BoosterEscudo");
                    break;
                case 4:
                    anim.Play("BoosterDisparo");
                    break;
                default:
                    break;
            }
        }
    }

    public void Salir()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }
}
