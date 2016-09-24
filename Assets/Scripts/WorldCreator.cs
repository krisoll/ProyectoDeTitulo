using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldCreator : MonoBehaviour {
    public bool ReemplazarMundo;
    public Mundo mundo;
    public bool añadirHojas;
    public List<HojaPersonaje> hojasPersonajes;
    public enum Visualizacion
    {
        NoMostrar,
        Resumen,
        Completa
    }
    public Visualizacion verMundo;
    public List<Visualizacion> visualizacionHojas;
    public List<bool> mostrarParametro;
    public List<string> calculoParametro;

    // Use this for initialization
    void Start () {
        if (ReemplazarMundo) GameManager.gManager.mundo = mundo;
        if (añadirHojas)
        {
            if (GameManager.gManager.mundo.hojasDePersonaje == null) GameManager.gManager.mundo.hojasDePersonaje = new List<HojaPersonaje>();
            foreach (HojaPersonaje hp in hojasPersonajes)
            {
                GameManager.gManager.mundo.hojasDePersonaje.Add(hp);
            }
        }
	}
}
