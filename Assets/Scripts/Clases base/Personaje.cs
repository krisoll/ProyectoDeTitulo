using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Personaje : MonoBehaviour {

	public string nombre;
	public HojaPersonaje hoja;
	public List<int> nivelParametros;
	public List<Equipable> equipo;
	public List<Bono> bono;
	public List<ModificadorEstado> modificadores;

    void Start()
    {
        nivelParametros = new List<int>();
        for (int i = 0; i < hoja.parametros.Count; i++)
        {
            nivelParametros.Add(0);
        }
        equipo = new List<Equipable>();
        for (int i = 0; i < hoja.espaciosEquipo.Count; i++)
        {
            equipo.Add(null);
        }
    }

    public void setNombre(string s)
    {
        nombre = s;
    }
}
