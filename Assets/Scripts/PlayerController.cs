using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private Vector2 screenBounds;
    private float objectWidth;
    private float objectHeight;
    public GameObject Bala;
    public Transform NullParent;
    public int GolpesRecibidos;
    public bool mover;

    // Start is called before the first frame update
    void Start()
    {
        mover = true;
    }

    private void LateUpdate()
    {
        //Vector3 viewPos = transform.position;
        //viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x + objectWidth, screenBounds.x * -1 - objectWidth);
        //viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y + objectHeight, screenBounds.y * -1 - objectHeight);
        //transform.position = viewPos;
    }
    // Update is called once per frame
    void Update()
    {
        if (mover)
        {
            if (ControlFreak2.CF2Input.GetKey(KeyCode.RightArrow))
            {
                this.gameObject.transform.Translate(Vector3.right * speed * Time.deltaTime);
            }

            if (ControlFreak2.CF2Input.GetKey(KeyCode.LeftArrow))
            {
                this.gameObject.transform.Translate(Vector3.left * speed * Time.deltaTime);
            }

            if (ControlFreak2.CF2Input.GetKey(KeyCode.UpArrow))
            {
                this.gameObject.transform.Translate(Vector3.up * speed * Time.deltaTime);
            }

            if (ControlFreak2.CF2Input.GetKey(KeyCode.DownArrow))
            {
                this.gameObject.transform.Translate(Vector3.down * speed * Time.deltaTime);
            }

            if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.Space))
            {
                Disparar();
            }
        }
    }
    public void Disparar()
    {
        if (Bala != null)
        {
            GameObject BalaActual = Instantiate(Bala, this.gameObject.transform);
            BalaActual.transform.localPosition = new Vector3(0, 0, 0);
            BalaActual.transform.SetParent(NullParent);
        }
    }

    public void RecibirGolpe()
    {
        GolpesRecibidos++;
        if (GolpesRecibidos >= 3)
        {
            GameController._controller.Fin();
        }
    }
}
