using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Personaje : MonoBehaviour {

	public string nombre;
    [System.NonSerialized]
	public HojaPersonaje hoja;
	public List<int> nivelParametros;
	public List<Equipable> equipo;
	public List<Bono> bono;
	public List<ModificadorEstado> modificadores;
    private int prevParam;

    public void Start()
    {
        if (hoja == null) return;
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

    public void setParamRandom(int p)
    {
        List<int> res = new List<int>();
        for(int i = 0; i < 4; i++)
        {
            int r = Random.Range(1, 7);
            res.Add(r);
        }
        res.Sort();
        res.RemoveAt(0);
        nivelParametros[p] = 0;
        foreach (int i in res) nivelParametros[p] += i;
    }

    public void switchParam(int a, int b)
    {
        int t = nivelParametros[a];
        nivelParametros[a] = nivelParametros[b];
        nivelParametros[b] = t;
    }

    public void setNombre(string s)
    {
        nombre = s;
    }

    public void prepareParam(int i)
    {
        prevParam = i;
    }

    public void switchParam(int i)
    {
        int t = nivelParametros[prevParam];
        nivelParametros[prevParam] = nivelParametros[i];
        nivelParametros[i] = t;
    }
}
