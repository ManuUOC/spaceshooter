using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private Vector2 screenBounds;
    private float objectWidth;
    private float objectHeight;
    public GameObject Bala;
    public GameObject Escudo;
    public Transform NullParent;
    public int GolpesRecibidos;
    public bool mover;
    public bool moverArriba;
    public bool moverAbajo;
    public bool moverDerecha;
    public bool moverIzquierda;
    public bool shoot;
    public bool damage;
    public bool infinito;
    public int shootInt;
    public Image NaveActual;
    public Slider SliderRecarga;
    public Slider SliderVida;
    public TMP_Text recargandoTxt;
    // Start is called before the first frame update
    void Start()
    {
        mover = true;
        shoot = true;
        moverArriba = true;
        moverAbajo = true;
        moverDerecha = true;
        moverIzquierda = true;
        damage = true;
        infinito = false;
        SliderRecarga.maxValue = 3;
        SliderVida.maxValue = 3;
    }
    void Update()
    {
        if (mover)
        {
            if (this.gameObject.transform.localPosition.y >= 292)
            {
                moverArriba = false;
            }
            else
            {
                moverArriba = true;
            }

            if (this.gameObject.transform.localPosition.y <= -276)
            {
                moverAbajo = false;
            }
            else
            {
                moverAbajo = true;
            }

            if (this.gameObject.transform.localPosition.x <= -776)
            {
                moverIzquierda = false;
            }
            else
            {
                moverIzquierda = true;
            }

            if (this.gameObject.transform.localPosition.x >= -153)
            {
                moverDerecha = false;
            }
            else
            {
                moverDerecha = true;
            }

            if (ControlFreak2.CF2Input.GetKey(KeyCode.RightArrow) && moverDerecha)
            {
                this.gameObject.transform.Translate(Vector3.right * speed * Time.deltaTime);
            }

            if (ControlFreak2.CF2Input.GetKey(KeyCode.LeftArrow) && moverIzquierda)
            {
                this.gameObject.transform.Translate(Vector3.left * speed * Time.deltaTime);
            }

            if (ControlFreak2.CF2Input.GetKey(KeyCode.UpArrow) && moverArriba)
            {
                this.gameObject.transform.Translate(Vector3.up * speed * Time.deltaTime);
            }

            if (ControlFreak2.CF2Input.GetKey(KeyCode.DownArrow) && moverAbajo)
            {
                this.gameObject.transform.Translate(Vector3.down * speed * Time.deltaTime);
            }

            if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.Space))
            {
                shootInt++;

                if (shoot)
                {
                    Disparar();
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "EscudoObj(Clone)")
        {
            StartCoroutine(PonerEscudo());
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.name == "DisparoObj(Clone)")
        {
            StartCoroutine(DisparoInfinito());
            Destroy(collision.gameObject);
        }
    }

    public IEnumerator PonerEscudo()
    {
        if (Escudo != null)
        {
            Escudo.SetActive(true);
            damage = false;
            yield return new WaitForSeconds(7f);
            damage = true;
            Escudo.SetActive(false);
        }
    }

    public IEnumerator DisparoInfinito()
    {
        if (Escudo != null)
        {
            infinito = true;
            if (SliderRecarga != null)
            {
                SliderRecarga.value = 0;
            }
            recargandoTxt.gameObject.SetActive(true);
            recargandoTxt.text = "DISPARA!";
            yield return new WaitForSeconds(7f);
            recargandoTxt.gameObject.SetActive(false);
            recargandoTxt.text = "RECARGANDO";
            infinito = false;
        }
    }

    public IEnumerator CadenciaDisparo()
    {
        if (recargandoTxt != null)
        {
            recargandoTxt.gameObject.SetActive(true);

            shoot = false;

            yield return new WaitForSeconds(1f);

            if (SliderRecarga != null)
            {
                SliderRecarga.value = 0;
            }

            recargandoTxt.gameObject.SetActive(false);
            shootInt = 0;
            shoot = true;
        }
    }

    public void Disparar()
    {
        if (Bala != null)
        {
            this.gameObject.GetComponent<AudioSource>().Play();
            GameObject BalaActual = Instantiate(Bala, this.gameObject.transform);
            BalaActual.transform.localPosition = new Vector3(0, 0, 0);
            BalaActual.transform.SetParent(NullParent);
            SliderRecarga.value = shootInt;
            if (shootInt >= GameController._controller.cadenciaDisparo && !infinito)
            {
                StartCoroutine(CadenciaDisparo());
            }
        }
    }

    public void RecibirGolpe()
    {
        if (damage)
        {
            GolpesRecibidos++;
            if (SliderVida != null)
            {
                SliderVida.value = GolpesRecibidos;
            }
            if (GolpesRecibidos >= 3)
            {
                GameController._controller.Fin();
            }
        }
    }
}
