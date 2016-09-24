using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class HojaPersonaje
{
    public string nombre;
    public List<Parametro> parametros = new List<Parametro>();
    public List<int> espaciosEquipo = new List<int>();
    public List<Bono> bonos = new List<Bono>();
}
