using UnityEngine;
using System.Collections;
using System;

public class CommonFuncs{
    static public int evaluateCalc(string s, Personaje p)
    {
        AK.ExpressionSolver expS = new AK.ExpressionSolver();
        return expS.EvaluateExpression(replaceString(s, p));
    }

    static public int evaluateCalc(string s)
    {
        AK.ExpressionSolver expS = new AK.ExpressionSolver();
        return expS.EvaluateExpression(s);
    }

    static public int evaluateCalc(string s, int nivel)
    {
        AK.ExpressionSolver expS = new AK.ExpressionSolver();
        return expS.EvaluateExpression(s.Replace("_N", (nivel + 1) + ""));
    }

    static string replaceString(string s, Personaje p)
    {
        Mundo mundo = GameManager.gManager.mundo;
        for (int i = 0; i < p.nivelParametros.Count; i++)
        {
            s = s.Replace("v_" + mundo.parametrosFijos[p.hoja.parametros[i].idParametro], (p.hoja.parametros[i].valor[p.nivelParametros[i]]) + "");
            s = s.Replace("V_" + mundo.parametrosFijos[p.hoja.parametros[i].idParametro], (p.hoja.parametros[i].valor[p.nivelParametros[i]]) + "");
            s = s.Replace("n_" + mundo.parametrosFijos[p.hoja.parametros[i].idParametro], (p.nivelParametros[i] + 1) + "");
            s = s.Replace("N_" + mundo.parametrosFijos[p.hoja.parametros[i].idParametro], (p.nivelParametros[i] + 1) + "");
        }

        return s;
    }

    static public void setListSize(IList list, int size)
    {
        while (list.Count > size)
        {
            list.RemoveAt(list.Count - 1);
        }
        while (list.Count < size)
        {
            list.Add(null);
        }
    }

    static public void setListSize(IList list, int size, object newObject)
    {
        while (list.Count > size)
        {
            list.RemoveAt(list.Count - 1);
        }
        while (list.Count < size)
        {
            list.Add(newObject);
        }
    }

    static public string getListToString(IList l, int maxCharInLine)
    {
        if (l.Count == 0) return "";
        string res = "";
        string temp = "";
        foreach (object o in l)
        {
            string s = o.ToString();
            if (maxCharInLine > 0 && temp.Length + s.Length > maxCharInLine)
            {
                res += "\n" + s + ", ";
                temp = s + ", ";
            }
            else
            {
                res += s + ", ";
                temp += s + ", ";
            }
        }
        res = res.Remove(res.Length - 2, 2) + ".";
        return res;
    }
}
