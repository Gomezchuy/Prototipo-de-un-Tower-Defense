using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nuez : MonoBehaviour
{
    private Animator anim;
    private Plantas planta;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        planta = GetComponent<Plantas>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetInteger("Vida", planta.vida);
    }
}
