using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PapaBomb : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private float tiempoCarga = 5f;
    private bool canExplotar=false;

    private bool canDestroy = false;
    [SerializeField]private float timeToDestroy = 1f;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(tiempoCarga>0)
        {
            tiempoCarga -= Time.deltaTime;
            anim.SetFloat("Cargando", tiempoCarga);
            canExplotar = true;
        }

        if(canDestroy)
        {
            timeToDestroy -= Time.deltaTime;
            if(timeToDestroy<=0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Zombie") && canExplotar==true)
        {
            anim.SetTrigger("Explosion");
            canDestroy = true;
        }
    }
}
