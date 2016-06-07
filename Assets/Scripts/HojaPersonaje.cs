﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class HojaPersonaje : MonoBehaviour {
    public string nombre;

    public List<Parametro> parametros = new List<Parametro>();
    public List<GestorParametros> gestores;
    public GestorParametros gestorGeneral;

    public bool hasExp = false;
    public List<int> expReq;

    public enum TipoParametros
    {
        unNivelATodosLosParametros, //+1 a todos los paramertos, no gestores, parametros manejan su aumento con nivel (Exp clásica)
        UnNivelAUnGestorGeneral, //Forma alternativa de la opción anterior, permite un mejor orden pero es más complicado
        UnNivelAUnaHabilidad, //+1 a gestor, él determina a quien sube de nivel (Dark souls)
        UnNivelAUnParametro //+1 a parámetro, su aumento depende solo de él 
    }
    public TipoParametros tipo = TipoParametros.UnNivelAUnParametro;

    public void cambiarTipo(TipoParametros t)
    {
        tipo = t;
        if (tipo == TipoParametros.UnNivelAUnaHabilidad && gestores==null) gestores = new List<GestorParametros>();
    }

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    
}
