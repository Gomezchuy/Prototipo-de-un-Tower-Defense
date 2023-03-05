using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public List<Plantas> plantasAUsar;

    public GameObject Mazo;
    public GameObject PrefabCarta;

    private TextMeshProUGUI txtSoles;

    int soles = 1000;
    public int PlantaAUsar = 0;

    /// Activa la camara Winner ******************************************************************
    private Animator camaraWinner;
    static public int zombiesDestruidos;
    private bool victoria = false;
    private bool flagWin = true;
    private RectTransform CanvasMazoPlantas;
    /// ******************************************************************************************

    ///Cuenta el total de zombies a generar en todo el nivel -------------------------------------
    public GameObject[] respawnsZombies;

    [SerializeField] private int zombiesTotales;
    /// ------------------------------------------------------------------------------------------

    private void Awake()
    {
        zombiesDestruidos = 0;
        victoria = false;
        flagWin = true;

        Time.timeScale = 1;

        camaraWinner = GameObject.FindGameObjectWithTag("CamaraWinner").GetComponent<Animator>();
        CanvasMazoPlantas = GetComponent<RectTransform>();

        txtSoles = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        ///Cuenta el total de zombies a generar -------------------------------------
        respawnsZombies = GameObject.FindGameObjectsWithTag("generadoresZombies");

        foreach (GameObject respawn in respawnsZombies)
        {
            zombiesTotales += respawn.GetComponent<InstanciadorDeZombies>().tiempos.Length;
        }
        /// -------------------------------------------------------------------------

        ActualizarSoles(0);

        /*
         * Por cada carta asignada se le instanciárá un botón y se irá encolando en el mazo
         */
        for (int i = 0; i < plantasAUsar.Count; i++)
        {
            GameObject go = Instantiate(PrefabCarta) as GameObject;
            go.transform.SetParent(Mazo.transform);
            go.transform.position = Vector3.zero;
            go.transform.localScale = Vector3.one; //Arregla un error haciendo que la carta no se vea pequeña

            Image img = go.GetComponent<Image>();
            img.sprite = plantasAUsar[i].cartaAsiganada;

            Button bot = go.GetComponent<Button>();
            bot.onClick.RemoveAllListeners();
            int u = i; //Si se le asigna la variable "i" directamente se genera un error
            bot.onClick.AddListener(() => { PlantaAUsar = u; });
        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(r.origin,r.direction);

            if (hit.collider != null)
            {
                if(hit.collider.CompareTag("Cuadricula"))
                {
                    Transform t = hit.collider.transform;
                    CrearPlanta(PlantaAUsar, t);
                }
                else if (hit.collider.CompareTag("Sol"))
                {
                    ActualizarSoles(50);
                    Destroy(hit.collider.gameObject);
                }
            }
        }

        ///Logistica de la victoria ---------------------------------------------------------------------------------------
        if(zombiesDestruidos>=zombiesTotales && flagWin) 
        {
            victoria = true;
        }

        if(victoria)
        {
            camaraWinner.SetTrigger("Ganar");
            flagWin = false; //Nota: en vez de la bandera, solo pude disminuir uno el valor de "zombiesDestruidos", pero decidí hacerlo así
            victoria = false;
            CanvasMazoPlantas.position = new Vector2(CanvasMazoPlantas.position.x, CanvasMazoPlantas.position.y + 500);
            Time.timeScale = 0;
        }
        ///-----------------------------------------------------------------------------------------------------------------
    }

    void CrearPlanta(int numero, Transform t)
    {
        if (plantasAUsar[numero].precioSoles > soles)
            return;
        if (t.childCount != 0)
            return;

        Vector2 fixPosition = new Vector2(t.position.x, t.position.y - 0.5f);

        GameObject g = Instantiate(plantasAUsar[PlantaAUsar].gameObject, fixPosition, gameObject.transform.rotation) as GameObject;
        g.transform.SetParent(t);

        ActualizarSoles(-plantasAUsar[numero].precioSoles);
    }

    public void ActualizarSoles(int Add)
    {
        soles += Add;
        txtSoles.text = soles.ToString();
    }
}
