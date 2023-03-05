using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanzaGuisantes : MonoBehaviour
{
    [SerializeField]private float frecuenciaDeDisparo = 5;
    [SerializeField]private GameObject guisante;
    public Transform canion;
    public LayerMask layerZombie;

    IEnumerator Start()
    {
        while (true)
        {
            //------------------------------------------ CUANDO INICIE EL JUEGO
            yield return new WaitForSeconds(frecuenciaDeDisparo);
            //------------------------------------------ LANZA EL PROYECTIL LUEGO DE LA FRECUENCIA
            
            RaycastHit2D hit = Physics2D.Raycast(canion.position, Vector3.right,12, layerZombie);
            //Debug.DrawRay(canion.position, Vector3.right * 12);

            if (hit.collider != null)
            {
                GameObject go = Instantiate(guisante, canion.position, guisante.transform.rotation);
                Destroy(go, 10);
            }
        }
    }

}
