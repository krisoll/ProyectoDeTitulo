using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Com.LuisPedroFonseca.ProCamera2D;

public class ParametrosEnPantalla : MonoBehaviour {
    public ProCamera2DPointerInfluence pointerInf;
    public Personaje p;
    public GameObject cameraTarget;
    public GameObject paralax;
    public float paralaxMult;
    public List<ListUI> ListaPorCategorias;
	
	// Update is called once per frame
	void Update () {
        if (GameManager.gManager.mundo == null) return;
        if (p.hoja == null)
        {
            if(GameManager.gManager.mundo == null || GameManager.gManager.mundo.hojasDePersonaje == null || GameManager.gManager.mundo.hojasDePersonaje.Count == 0) return;
            p.hoja = GameManager.gManager.mundo.hojasDePersonaje[0];
            p.Start();
        }
        foreach(ListUI lui in ListaPorCategorias)
        {
            foreach (UIToScreen ui in lui.list)
            {
                ui.UI.transform.position = ui.refPos.transform.position;
                if (ui.texto != null) ui.texto.text = CommonFuncs.evaluateCalc(ui.calculo, p) + "";
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
