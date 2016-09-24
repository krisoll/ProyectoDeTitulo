using System.Collections.Generic;

[System.Serializable]
public class Mundo {

    public List<string> parametrosFijos = new List<string>();
    public List<Variable> parametrosVariables = new List<Variable>();
    public List<string> lugaresequip = new List<string>();
	public List<string> bonos = new List<string>();
    public List<Calculo> calculos = new List<Calculo>();
    public List<HojaPersonaje> hojasDePersonaje = new List<HojaPersonaje>();
    
    [System.Serializable]
    public class Variable
    {
        public string nombre;
        public bool tieneLimSup;
        public bool tieneLimInf;
        public override string ToString()
        {
            return nombre;
        }
    }
    [System.Serializable]
    public class Calculo
    {
        public string nombre;
        public string textoDeCalculo;
    }
}
