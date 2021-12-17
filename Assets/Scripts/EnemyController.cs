using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public Sprite[] MeteoritosSprite;
    public Image MeteoritoImg;
    public GameObject AnimExplosion;
    public GameObject AnimColisionJugador;
    public float speed;
    public bool mover;

    public void Awake()
    {
        mover = true;
    }

    private void Start()
    {

        InstanciarEnemigo();
    }
    private void Update()
    {
        if (mover)
        {
            this.gameObject.transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
    }

    public void InstanciarEnemigo()
    {
        if (MeteoritoImg != null && MeteoritosSprite.Length > 0)
        {
            MeteoritoImg.sprite = MeteoritosSprite[Random.Range(0, MeteoritosSprite.Length)];
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Destruir")
        {
            Destroy(this.gameObject);
        }
        if (collision.gameObject.name == "Jugador")
        {
            collision.gameObject.GetComponent<PlayerController>().RecibirGolpe();
            EliminarEnemigo(1);
        }
    }

    public void EliminarEnemigo(int enemigo)
    {
        //TODO: Cambiamos el sprite por uno de explosion;
        mover = false;
        if (enemigo == 0)//BALA
        {
            if (AnimExplosion != null)
            {
                MeteoritoImg.enabled = false;
                Vector2 Meteorito;
                Meteorito.x = this.gameObject.transform.position.x;
                Meteorito.y = this.gameObject.transform.position.y;
                Vector3 MeteoritoWorld = Camera.main.ScreenToWorldPoint(new Vector3(Meteorito.x, Meteorito.y, Camera.main.nearClipPlane + 10));
                AnimExplosion.transform.position = MeteoritoWorld;
                AnimExplosion.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                AnimExplosion.SetActive(true);
                this.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
        else
        {
            if (AnimColisionJugador != null)
            {
                MeteoritoImg.enabled = false;
                Vector2 Meteorito;
                Meteorito.x = this.gameObject.transform.position.x;
                Meteorito.y = this.gameObject.transform.position.y;
                Vector3 MeteoritoWorld = Camera.main.ScreenToWorldPoint(new Vector3(Meteorito.x, Meteorito.y, Camera.main.nearClipPlane + 10));
                AnimColisionJugador.transform.position = MeteoritoWorld;
                AnimColisionJugador.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                AnimColisionJugador.SetActive(true);
                this.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
        Invoke("Destruir", 0.5f);
    }
    public void Destruir()
    {
        Destroy(this.gameObject);
    }
}
