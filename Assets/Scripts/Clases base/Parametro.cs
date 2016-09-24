using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Parametro {
    public int idParametro;
    public enum Upgradeable
    {
        no,
        unoEnUno,
        lista
    }
    public Upgradeable upgradeable;
    public List<int> valor = new List<int>();
    

}
