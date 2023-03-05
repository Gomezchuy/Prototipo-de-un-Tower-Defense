using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuisanteProjectile : MonoBehaviour
{
    [SerializeField] private int velocidad = 10;
    public int danio = 1;

    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right * velocidad * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Plantorcha"))
        {
            sprite.color = new Color(255, 0, 0);
            danio = 3;
        }
    }
}
