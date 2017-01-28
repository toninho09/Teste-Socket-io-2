using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour {

    public GameObject objetoBolinha;
    private Vector3 posicaoInicial;
	// Use this for initialization
	void Start () {
        posicaoInicial = this.transform.position - objetoBolinha.transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = objetoBolinha.GetComponent<Rigidbody>().transform.position + posicaoInicial ;
	}
}
