using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ParametroV1 {
    public string nombre;
    //Que es el parámetro
    public enum TipoParametro
    {
        entero,
        flotante,
        dadoSuma,
        dadoPromedio
    }
    public TipoParametro tipoParametro;
    public bool dadoConCero;

    //Valores y nivel actual
    public int nivel;
    public List<string> valorNiveles;

    //Requerimiento de puntaje antes de subir
    public bool requiereCiertoValor;
    public List<int> reqPuntos;
    public int puntosTemporal;

    //public bool puedeBajar; //Se verá más adelante

    public ParametroV1()
    {
        tipoParametro = TipoParametro.entero;
        valorNiveles = new List<string>();
        setCount(10);
    }
    
    public void setCount(int c)
    {
        if (c <= 0) return;
        while (valorNiveles.Count < c) valorNiveles.Add("");
        while (valorNiveles.Count > c) valorNiveles.RemoveAt(valorNiveles.Count - 1);

        if (requiereCiertoValor)
        {
            while (reqPuntos.Count < c-1) reqPuntos.Add(1);
            while (reqPuntos.Count > c-1) reqPuntos.RemoveAt(reqPuntos.Count - 1);
        }
    }

    public void cambiarRequiereValor(bool b)
    {
        requiereCiertoValor = b;
        if (requiereCiertoValor)
        {
            if(reqPuntos==null) reqPuntos = new List<int>();
            while (reqPuntos.Count < valorNiveles.Count-1) reqPuntos.Add(1);
        }
    }

    public void addNivel(int puntos)
    {
        if (nivel == valorNiveles.Count - 1) return;
        if (requiereCiertoValor)
        {
            puntosTemporal += puntos;
            while (puntosTemporal >= reqPuntos[nivel])
            {
                puntosTemporal -= reqPuntos[nivel];
                nivel++;
                if (nivel == valorNiveles.Count - 1)
                {
                    puntosTemporal = 0;
                    return;
                }
            }
        }
        else
        {
            nivel += puntos;
        }
    }

    public int cuantosPuntosFaltan()
    {
        if (nivel == valorNiveles.Count - 1) return -1;
        if (!requiereCiertoValor) return 1;
        return reqPuntos[nivel];
    }

    public string getValue()
    {
        return valorNiveles[nivel];
    }

    public int getIntValue()
    {
        return int.Parse(valorNiveles[nivel]);
    }
    public float getFloatValue()
    {
        return float.Parse(valorNiveles[nivel]);
    }
    public int getDiceValue()
    {
        string dice = valorNiveles[nivel];
        if (tipoParametro==TipoParametro.dadoSuma)
        {
            int a = int.Parse(dice.Split('D')[0]);
            int b = int.Parse(dice.Split('D')[1]);
            int r = 0;
            for (int i = 0; i < a; i++) r += Random.Range(0, b) + (dadoConCero ? 0 : 1);
            return r;
        }
        if (tipoParametro == TipoParametro.dadoPromedio)
        {
            int a = int.Parse(dice.Split('o')[0]);
            int b = int.Parse(dice.Split('o')[1]);
            int r = 0;
            for (int i = 0; i < a; i++) r += Random.Range(0, b) + (dadoConCero ? 0 : 1);
            r /= a;
            return r;
        }
        return -1;
    }

    public int getCount()
    {
        return valorNiveles.Count;
    }

    public int verificarValores()
    {
        if (tipoParametro == TipoParametro.entero)
        {
            int o;
            for (int i = 0; i < valorNiveles.Count; i++)
            {
                if (!int.TryParse(valorNiveles[i], out o)) return i;
            }
        }
        else if (tipoParametro == TipoParametro.flotante)
        {
            float o;
            for (int i = 0; i < valorNiveles.Count; i++)
            {
                if (!float.TryParse(valorNiveles[i], out o)) return i;
            }
        }
        else if (tipoParametro == TipoParametro.dadoSuma)
        {
            int o;
            for (int i = 0; i < valorNiveles.Count; i++)
            {
                if (valorNiveles[i].Split('D').Length != 2) return i;
                if (!int.TryParse(valorNiveles[i].Split('D')[0], out o)) return i;
                if (!int.TryParse(valorNiveles[i].Split('D')[1], out o)) return i;
            }
        }
        else if (tipoParametro == TipoParametro.dadoPromedio)
        {
            int o;
            for (int i = 0; i < valorNiveles.Count; i++)
            {
                if (valorNiveles[i].Split('o').Length != 2) return i;
                if (!int.TryParse(valorNiveles[i].Split('o')[0], out o)) return i;
                if (!int.TryParse(valorNiveles[i].Split('o')[1], out o)) return i;
            }
        }

        return -1;
    }

    #region Escalas
    public void setEscalaUniformeInt(int valIni, int aumento)
    {
        for (int i = 0; i < valorNiveles.Count; i++)
        {
            valorNiveles[i] = (valIni + i * aumento) + "";
        }
    }

    public void setEscalaUniformeFloat(float valIni, float aumento)
    {
        for (int i = 0; i < valorNiveles.Count; i++)
        {
            valorNiveles[i] = (valIni + i * aumento) + "";
        }
    }
    
    public bool setEscalaCalculo(string calculo)
    {
        AK.ExpressionSolver ex = new AK.ExpressionSolver();
        List<string> res = new List<string>(valorNiveles.Count);
        bool error=false;
        for (int i = 0; i < valorNiveles.Count; i++)
        {
            string s = calculo + "";
            s.Replace("nivel", (i + 1) + "");
            s.Replace("Nivel", (i + 1) + "");
            try
            {
                if(tipoParametro==TipoParametro.entero)  res[i]=((int)ex.EvaluateExpression(s))+"";
                else if(tipoParametro == TipoParametro.flotante) res[i] = ((float)ex.EvaluateExpression(s)) + "";
            }
            catch
            {
                error = true;
                break;
            }
        }
        if (!error) valorNiveles = res;
        return error;
    }

    public bool setValueAt(string val, int pos)
    {
        if (pos >= valorNiveles.Count) return false;
        if (tipoParametro == TipoParametro.entero)
        {
            int o;
            if (!int.TryParse(val, out o)) return false;
        }
        else if (tipoParametro == TipoParametro.flotante)
        {
            float o;
            if (!float.TryParse(val, out o)) return false;
        }
        else if (tipoParametro == TipoParametro.dadoSuma)
        {
            int o;
            if (val.Split('D').Length != 2) return false;
            if (!int.TryParse(val.Split('D')[0], out o)) return false;
            if (!int.TryParse(val.Split('D')[1], out o)) return false;
        }
        else if (tipoParametro == TipoParametro.dadoPromedio)
        {
            int o;
            if (val.Split('o').Length != 2) return false;
            if (!int.TryParse(val.Split('o')[0], out o)) return false;
            if (!int.TryParse(val.Split('o')[1], out o)) return false;
        }
        valorNiveles[pos] = val;
        return true;
    }

    public void setEscaraUniformeRequisito(int valIni, int valAum)
    {
        for(int i = 0; i < reqPuntos.Count; i++)
        {
            reqPuntos[i] = valIni + i * valAum;
        }
    }

    public bool setEscalaCalculoReq(string calculo)
    {
        AK.ExpressionSolver ex = new AK.ExpressionSolver();
        List<int> res = new List<int>(reqPuntos.Count);
        bool error = false;
        for (int i = 0; i < reqPuntos.Count; i++)
        {
            string s = calculo + "";
            s.Replace("nivel", (i + 1) + "");
            s.Replace("Nivel", (i + 1) + "");
            try
            {
                res[i] = (int)ex.EvaluateExpression(s);
            }
            catch
            {
                error = true;
                break;
            }
        }
        if (!error) reqPuntos = res;
        return error;
    }
    #endregion

    public void guardar()
    {
        if (!requiereCiertoValor) reqPuntos = null;
    }
}
