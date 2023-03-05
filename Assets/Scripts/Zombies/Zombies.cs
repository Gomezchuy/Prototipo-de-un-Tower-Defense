using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombies : MonoBehaviour
{
    [SerializeField] private int vida;
    [SerializeField] private float velocidad;

    public LayerMask layerPlanta;

    public float cadencia = 1f;

    float cadAux = 0;

    private Animator camaraGameOver;
    private RectTransform CanvasMazoPlantas;
    
    [Range(1, 4)][SerializeField]private int opZombie; //¿Que tipo de Zombie será?
    /// <summary>
    /// 1.- Zombie normal
    /// 2.- Zombie de nuez
    /// 3.- Zombie periodico
    /// 4.- Zombie caja explosiva
    /// </summary>

    private float tiempoParaExplotar=0;//Tiempo para que el zombie de caja haga su explosión (Este valor se le asigna un valor random automaticamente)
    private float timeToDestroy = 2.5f; //Tiempo para destruir al zombie de caja luego de su animación de explosión
    private bool flagBoxZombie = false; //Al terminar "timeToDestroy", el Zombie se destruye tan rápido que no permite que se aumente el valor de su muerte en el GameManager

    private Animator animZombie;

    private void Awake()
    {
        animZombie = GetComponent<Animator>();
        camaraGameOver = GameObject.FindGameObjectWithTag("CamaraGameOver").GetComponent<Animator>();
        CanvasMazoPlantas = GameObject.FindGameObjectWithTag("CanvasUIManager").GetComponent<RectTransform>() ;
    }

    private void Start()
    {
        if(opZombie==4)// Si el zombie es el de la caja explosiva
        {
            tiempoParaExplotar = NumeroAleatorio(20, 50);
        }
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.left, 0.5f, layerPlanta);

        if (hit.collider != null)
        {
            cadAux += Time.deltaTime;
            if(cadAux>=cadencia)
            {
                cadAux = 0;
                hit.collider.SendMessage("Morder");
            }
        }
        else
        {
            cadAux = 0;
            transform.position -= Vector3.right * velocidad * Time.deltaTime;
        }

        switch(opZombie)
        {
            case 1://Zombie Normal

                break;
            case 2://Zombie WallNut (nues)
                animZombie.SetInteger("Salud", vida);
                break;
            case 3://Zombie Newspaper (periodico)
                animZombie.SetInteger("Salud", vida);

                if(vida<12)
                {
                    velocidad = 1;
                    cadencia = 0.3f;
                }
                break;
            case 4://ZombieBox (el que explota)

                tiempoParaExplotar -= Time.deltaTime;

                if(tiempoParaExplotar<=0)
                {
                    velocidad = 0;
                    animZombie.SetTrigger("ExplotarZombie");
                    
                    //Destruir al zombie
                    timeToDestroy -= Time.deltaTime;
                    if (timeToDestroy <= 0)
                    {
                        GameManager.zombiesDestruidos++;
                        flagBoxZombie = true;
                        if(flagBoxZombie)
                        {
                            Destroy(gameObject);
                        }
                    }
                }
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Guisante"))
        {
            vida -= collision.gameObject.GetComponent<GuisanteProjectile>().danio;
            Destroy(collision.gameObject);

            if(vida<=0)
            {
                GameManager.zombiesDestruidos++;
                Destroy(gameObject);
            }
        }
        if (collision.CompareTag("PapaBombExplosion"))
        {
            vida -= 15;
            if (vida <= 0)
            {
                GameManager.zombiesDestruidos++;
                Destroy(gameObject);
            }
        }
        if (collision.CompareTag("CherryBombExplosion"))
        {
            vida -= 30;
            if (vida <= 0)
            {
                GameManager.zombiesDestruidos++;
                Destroy(gameObject);
            }
        }
        if (collision.CompareTag("FailState"))
        {
            Debug.Log("He perdido!!");
            Time.timeScale = 0;
            camaraGameOver.SetTrigger("Perder");
            CanvasMazoPlantas.position = new Vector2(CanvasMazoPlantas.position.x, CanvasMazoPlantas.position.y + 500);
        }
    }

    private float NumeroAleatorio(int numMin,int numMax)
    {
        return Random.Range(numMin, numMax);
    }
}
