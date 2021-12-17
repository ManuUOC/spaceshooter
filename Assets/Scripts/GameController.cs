using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject Enemigo;
    public GameObject[] SpawnsEnemigos;
    public int nivel;
    public float CadendiaEnemigos;
    public static GameController _controller;
    public int puntuacion;
    private int ultimaPuntuacion;

    public TMPro.TMP_Text PuntuacionTxt;
    public TMPro.TMP_Text LevelTxt;

    public bool delEnemy;
    public bool finJuego;

    public GameObject player;
    public GameObject Pop_Fin;
    public GameObject HighScoreObj;
    public GameObject DefectoObj;
    public TMPro.TMP_Text ScoreFinal;

    public Sprite[] BgScore;

    private void Awake()
    {
        _controller = this;
    }
    private void Start()
    {
        StartCoroutine(PrepararEscenario());
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
    public IEnumerator PrepararEscenario()
    {
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
                    break;
                case 1:
                    ControladorEnemigo.speed = 1500;
                    CadendiaEnemigos = 1.5f;
                    break;
                case 2:
                    ControladorEnemigo.speed = 1700;
                    CadendiaEnemigos = 1.0f;
                    break;
                case 3:
                    ControladorEnemigo.speed = 2000;
                    CadendiaEnemigos = 0.5f;
                    break;
                default:
                    ControladorEnemigo.speed = 2000;
                    CadendiaEnemigos = 0.5f;
                    break;
            }
        }
        yield return new WaitForSeconds(CadendiaEnemigos);
        if (finJuego == true)
        {

        }
        else
        {
            StartCoroutine(PrepararEscenario());
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
            player.SetActive(false);
            Pop_Fin.SetActive(true);
            ScoreFinal.text = puntuacion.ToString();
            if (System.IO.File.Exists(Application.persistentDataPath + "\\data.json"))
            {
                string json = System.IO.File.ReadAllText(Application.persistentDataPath + "\\data.json");
                PlayerData pd = JsonUtility.FromJson<PlayerData>(json);
                if (pd.Puntuacion <= puntuacion)
                {
                    HighScoreObj.SetActive(true);
                    DefectoObj.SetActive(false);
                    pd.Puntuacion = puntuacion;
                    string jsonGuardar = JsonUtility.ToJson(pd);
                    File.WriteAllText(Application.persistentDataPath + "\\data.json", jsonGuardar);
                }
            }
        }
    }
}
