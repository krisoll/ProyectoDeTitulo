﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mundo : MonoBehaviour {

    public List<string> parametrosFijos;
    public List<Variable> parametrosVariables;
    public List<string> lugaresequip;
	public List<string> bonos;
    public List<Calculo> calculos;
    public List<HojaPersonaje> hojasDePersonaje;

    [System.Serializable]
    public class Variable
    {
        public string nombre;
        public bool tieneLimSup;
        public bool tieneLimInf;
    }
    [System.Serializable]
    public class Calculo
    {
        public string nombre;
        public string textoDeCalculo;
    }
}
