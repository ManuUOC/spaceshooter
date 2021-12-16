using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private Vector2 screenBounds;
    private float objectWidth;
    private float objectHeight;


    // Start is called before the first frame update
    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        objectWidth = transform.GetComponent<BoxCollider>().bounds.size.x / 2;
        objectHeight = transform.GetComponent<BoxCollider>().bounds.size.y / 2;
        
    }

    private void LateUpdate()
    {
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x + objectWidth, screenBounds.x * -1 - objectWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y + objectHeight, screenBounds.y * -1 - objectHeight);
        transform.position = viewPos;
    }
    // Update is called once per frame
    void Update()
    {

        if(Input.GetKey(KeyCode.RightArrow))
        {
            this.gameObject.transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.gameObject.transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
    }
}
