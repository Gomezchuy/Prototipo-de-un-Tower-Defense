using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plantas : MonoBehaviour
{
    public Sprite cartaAsiganada;

    public int precioSoles;
    public int vida;

    void Morder()
    {
        vida--;
        if(vida<=0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("ExplosionZombie"))
        {
            Destroy(gameObject);
        }
    }
}
