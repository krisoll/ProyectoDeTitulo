using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class fuerza : MonoBehaviour {
	public Text Fuerza;
	public Text Modificador;
	public HojaPersonaje neeee;
	void Start () {
		neeee.parametros [0].nivel = 11;
		Fuerza.text = neeee.parametros [0].getValue ();
		int a = (neeee.parametros [0].getIntValue ())/2;
		neeee.parametros [6].nivel = a;
		Modificador.text=neeee.parametros [6].getValue ();
	

	}
	


}
