using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Com.LuisPedroFonseca.ProCamera2D;

public class ParametrosEnPantalla : MonoBehaviour {
    public ProCamera2DPointerInfluence pointerInf;
    public Mundo mundo;
    public Personaje p;
    public GameObject cameraTarget;
    public GameObject paralax;
    public float paralaxMult;
    public List<ListUI> ListaPorCategorias;
	
	// Update is called once per frame
	void Update () {
        foreach(ListUI lui in ListaPorCategorias)
        {
            foreach (UIToScreen ui in lui.list)
            {
                ui.UI.transform.position = ui.refPos.transform.position;
                if (ui.texto != null) ui.texto.text = getResultado(ui.calculo) + "";
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (pointerInf.enabled)
            {
                cameraTarget.transform.position = pointerInf.transform.position;
                pointerInf.enabled = false;
            }
            else
            {
                cameraTarget.transform.position = Vector3.zero;
                pointerInf.enabled = true;
            }
        }
        if (paralax != null) paralax.transform.position = pointerInf.transform.position * paralaxMult;
	}

    public int getResultado(string calc)
    {
        AK.ExpressionSolver expS = new AK.ExpressionSolver();
        return (int) expS.EvaluateExpression(reemplazarVariables(calc));
    }

    public string reemplazarVariables(string calc)
    {
        string res = calc;

        for(int i = 0; i < p.nivelParametros.Count; i++)
        {
            res = res.Replace("v_" + mundo.parametrosFijos[p.hoja.parametros[i].idParametro], (p.hoja.parametros[i].valor[p.nivelParametros[i]]) + "");
            res = res.Replace("n_" + mundo.parametrosFijos[p.hoja.parametros[i].idParametro], (p.nivelParametros[i] + 1) + "");
            res = res.Replace("V_" + mundo.parametrosFijos[p.hoja.parametros[i].idParametro], (p.hoja.parametros[i].valor[p.nivelParametros[i]]) + "");
            res = res.Replace("N_" + mundo.parametrosFijos[p.hoja.parametros[i].idParametro], (p.nivelParametros[i] + 1) + "");
        }

        return res;
    }

    [System.Serializable]
    public class ListUI
    {
        public string nombre;
        public List<UIToScreen> list;
    }
    [System.Serializable]
    public class UIToScreen
    {
        public string nombre;
        public GameObject UI;
        public GameObject refPos;
        public Text texto;
        public string calculo;
    }
}
