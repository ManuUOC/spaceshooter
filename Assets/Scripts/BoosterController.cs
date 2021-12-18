using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterController : MonoBehaviour
{
    public int tipoBooster;
    private float speed = 1000;
    private void Update()
    {
        this.gameObject.transform.Translate(Vector2.left * speed * Time.deltaTime);
    }
}
