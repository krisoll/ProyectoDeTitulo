using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Permite subir de nivel a varios parametros a la vez 
[System.Serializable]
public class GestorParametros {
    public string nombre;
    public List<Gestor> parametros;
    public int nivel;
    public int totalNiveles=10;
    
    public GestorParametros()
    {
        parametros = new List<Gestor>();
    }

    public void addParam(ref Parametro par)
    {
        Gestor g = new Gestor(totalNiveles);
        g.param = par;
        parametros.Add(g);
    }

    public void setCount(int i)
    {
        totalNiveles = i;
        foreach (Gestor g in parametros) g.setCount(totalNiveles);
    }

    public void addNivel()
    {
        foreach (Gestor g in parametros)
        {
            g.addNivel(nivel);
        }
        nivel++;
    }

    [System.Serializable]
    public class Gestor
    {
        public Parametro param;
        public List<int> aumento;

        public Gestor(int i)
        {
            aumento = new List<int>();
            setCount(i);
        }

        public void setStatic(int val, int limit)
        {
            aumento = new List<int>();
            for (int i = 0; i < limit; i++)
            {
                aumento.Add(val);
            }
        }

        public void setCount(int i)
        {
            if (i < 0) return;
            while (i > aumento.Count) aumento.Add(1);
            while (i < aumento.Count) aumento.RemoveAt(aumento.Count-1);
        }

        public void setLineal(int init, int inc, int limit)
        {
            aumento = new List<int>();
            for (int i = 0; i < limit; i++)
            {
                aumento.Add(init + i * inc);
            }
        }

        public bool setFunction(string calculo, int limit)
        {
            AK.ExpressionSolver ex = new AK.ExpressionSolver();
            bool error = false;
            aumento = new List<int>();
            for (int i = 0; i < limit; i++)
            {
                try
                {
                    string s = calculo + "";
                    s.Replace("nivel", (i + 1) + "");
                    s.Replace("Nivel", (i + 1) + "");
                    aumento.Add((int)ex.EvaluateExpression(s));
                    
                }
                catch
                {
                    error = true;
                    break;
                }
            }
            return error;
        }

        public void addNivel(int nivel)
        {
            param.addNivel(aumento[nivel]);
        }
    }
}
