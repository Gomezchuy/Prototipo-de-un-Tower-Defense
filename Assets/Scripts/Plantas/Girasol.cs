using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Girasol : MonoBehaviour
{
    private float frecuenciaDeSoles = 10;
    public GameObject Sol;

    IEnumerator Start()
    {
        while (true)
        {
            //------------------------------------------ CUANDO INICIE EL JUEGO
            yield return new WaitForSeconds(frecuenciaDeSoles);
            //------------------------------------------ LANZA EL SOL LUEGO DE LA FRECUENCIA
            GameObject go = Instantiate(Sol, transform.position + Vector3.up * Random.Range(0f, 1f) + Vector3.left * Random.Range(-1f, 1f), Sol.transform.rotation);
            Destroy(go, 10);
        }
    }
    
}
