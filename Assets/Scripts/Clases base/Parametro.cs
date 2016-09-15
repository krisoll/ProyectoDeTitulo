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
        tabla
    }
    public Upgradeable upgradeable;
    public List<int> valor;
    

}
