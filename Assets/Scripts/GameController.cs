using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject Enemigo;
    public GameObject[] SpawnsEnemigos;
    public int nivel;
    public float CadendiaEnemigos;
    public static GameController _controller;
    public int puntuacion;
    private int ultimaPuntuacion;
    public int cadenciaDisparo;
    public float cadenciaBooster;

    public TMPro.TMP_Text PuntuacionTxt;
    public TMPro.TMP_Text LevelTxt;
    public TMPro.TMP_Text ScoreFinal;
    public TMPro.TMP_Text HighScoreActual;


    public GameObject playerObj;
    public GameObject Pop_Fin;
    public GameObject HighScoreObj;
    public GameObject DefectoObj;

    public bool delEnemy;
    public bool finJuego;

    public Sprite[] BgScore;
    public Sprite[] NavesSprites;

    public PlayerData pd;
    public PlayerController player;

    public GameObject[] TipoBoosters;

    public AudioClip musicaFondo;

    private void Awake()
    {
        _controller = this;
    }
    private void Start()
    {
        if (UserController._user != null)
        {
            pd = UserController._user.ComprobarJson();
            if (musicaFondo != null)
            {
                UserController._user._audio.clip = musicaFondo;
                UserController._user._audio.Play();
            }
        }
        if (HighScoreActual != null && pd != null)
        {
            cadenciaDisparo = 3;
            HighScoreActual.text = pd.Puntuacion.ToString();
            player.NaveActual.sprite = NavesSprites[UserController._user.NaveActual];
            player.SliderRecarga.maxValue = cadenciaDisparo;
        }
        StartCoroutine(PrepararEscenario());
        Invoke("AparecerEscudos", 10f);
    }
    private void Update()
    {
        if (delEnemy)
        {
            GameObject enemigo = GameObject.Find("Enemigo(Clone)"); ;
            if (enemigo == null)
            {
                delEnemy = false;
            }
            else
            {
                enemigo.GetComponent<EnemyController>().EliminarEnemigo(0);
            }
        }
    }

    public void AparecerEscudos()
    {
        StartCoroutine(SpawnEscudos());
    }

    public IEnumerator PrepararEscenario()
    {
        if (player != null)
        {
            player.SliderRecarga.maxValue = cadenciaDisparo;
        }
        GameObject EnemigoActual = Instantiate(Enemigo, SpawnsEnemigos[Random.Range(0, SpawnsEnemigos.Length)].transform);
        EnemigoActual.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        if (EnemigoActual != null)
        {
            EnemyController ControladorEnemigo = EnemigoActual.GetComponent<EnemyController>();
            switch (nivel)
            {
                case 0:
                    ControladorEnemigo.speed = 1000;
                    CadendiaEnemigos = 3.0f;
                    cadenciaDisparo = 3;
                    cadenciaBooster = 20f;
                    break;
                case 1:
                    ControladorEnemigo.speed = 1500;
                    CadendiaEnemigos = 1.5f;
                    cadenciaDisparo = 4;
                    cadenciaBooster = 15f;
                    break;
                case 2:
                    ControladorEnemigo.speed = 1700;
                    CadendiaEnemigos = 1.0f;
                    cadenciaDisparo = 5;
                    cadenciaBooster = 10f;
                    break;
                case 3:
                    ControladorEnemigo.speed = 2000;
                    CadendiaEnemigos = 0.5f;
                    cadenciaDisparo = 6;
                    cadenciaBooster = 8f;
                    break;
                case 4:
                    ControladorEnemigo.speed = 2100;
                    CadendiaEnemigos = 0.4f;
                    cadenciaDisparo = 7;
                    cadenciaBooster = 7f;
                    break;
                case 5:
                    ControladorEnemigo.speed = 2200;
                    CadendiaEnemigos = 0.3f;
                    cadenciaDisparo = 8;
                    cadenciaBooster = 7f;
                    break;
                case 6:
                    ControladorEnemigo.speed = 2300;
                    CadendiaEnemigos = 0.2f;
                    cadenciaDisparo = 9;
                    cadenciaBooster = 6f;
                    break;
                default:
                    ControladorEnemigo.speed = 2300;
                    CadendiaEnemigos = 0.2f;
                    cadenciaDisparo = 9;
                    cadenciaBooster = 6f;
                    break;
            }
        }
        yield return new WaitForSeconds(CadendiaEnemigos);
        if (finJuego == false)
        {
            StartCoroutine(PrepararEscenario());
        }
    }

    public IEnumerator SpawnEscudos()
    {
        if (!finJuego)
        {
            GameObject BoosterActual = Instantiate(TipoBoosters[Random.Range(0, TipoBoosters.Length)], SpawnsEnemigos[Random.Range(0, SpawnsEnemigos.Length)].transform);
            BoosterActual.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            yield return new WaitForSeconds(cadenciaBooster);
            StartCoroutine(SpawnEscudos());
        }
    }

    public void MatarEnemigo()
    {
        puntuacion++;
        if (PuntuacionTxt != null)
        {
            PuntuacionTxt.text = puntuacion.ToString();
        }
        if (puntuacion % 20 == 0 && puntuacion != ultimaPuntuacion && LevelTxt != null)
        {
            ultimaPuntuacion = puntuacion;
            nivel++;
            LevelTxt.text = nivel.ToString();
        }
    }

    public void PausarJuego()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
    }

    public void Salir()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
    public void Fin()
    {
        //TODO: Pantalla ScoreScreen, comprobar Json, Si la puntuacion en mayor, la sobrescribimos.
        if (player != null && Pop_Fin != null && ScoreFinal != null && HighScoreObj != null)
        {
            finJuego = true;
            delEnemy = true;
            playerObj.SetActive(false);
            Pop_Fin.SetActive(true);
            ScoreFinal.text = puntuacion.ToString();
            if (pd.Puntuacion < puntuacion)
            {
                HighScoreObj.SetActive(true);
                DefectoObj.SetActive(false);
                pd.Puntuacion = puntuacion;
                if (UserController._user != null)
                {
                    UserController._user.GuardarJson(pd);
                }
            }
        }
    }
}
