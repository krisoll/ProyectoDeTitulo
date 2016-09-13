using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Personaje : MonoBehaviour {

	public string Nombre;
	public HojaPersonaje hoja;
	public List<int> nivel;
	public List<equipable> equipo;
	public List<Bono> bono;
	public List<ModificadorEstado> modificadores;

}
