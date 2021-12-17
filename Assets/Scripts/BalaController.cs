using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalaController : MonoBehaviour
{
    float speed = 2600;
    private void Start()
    {

    }
    private void Update()
    {
        this.gameObject.transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Enemigo(Clone)")
        {
            GameController._controller.MatarEnemigo();
            collision.gameObject.GetComponent<EnemyController>().EliminarEnemigo(0);
            Destroy(this.gameObject);
        }
        if (collision.gameObject.name == "Destruir")
        {
            Destroy(this.gameObject);
        }
    }
}
