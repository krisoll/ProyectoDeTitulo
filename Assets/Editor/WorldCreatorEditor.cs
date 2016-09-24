using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(WorldCreator))]
public class WorldCreatorEditor : Editor
{
    WorldCreator t;

	// Use this for initialization
	void Awake () {
        t = (WorldCreator)target;
	}

    // Update is called once per frame
    public override void OnInspectorGUI()
    {
        drawWorld();
    }

    private void drawWorld()
    {
        //Mundo
        t.ReemplazarMundo = EditorGUILayout.Toggle("Crear Mundo", t.ReemplazarMundo);
        if (!t.ReemplazarMundo) return;
        t.verMundo = (WorldCreator.Visualizacion)EditorGUILayout.EnumPopup("Mostrar mundo", t.verMundo);
        if (t.verMundo == WorldCreator.Visualizacion.Completa) mundoCompleto();
        else if(t.verMundo == WorldCreator.Visualizacion.Resumen) mundoResumen();
        
        //Hojas de personaje
        t.añadirHojas = EditorGUILayout.Toggle("Crear hojas de personaje", t.añadirHojas);
        CommonFuncs.setListSize(t.hojasPersonajes, EditorGUILayout.IntField("Numero de hojas", t.hojasPersonajes.Count));
        CommonFuncs.setListSize(t.visualizacionHojas, t.hojasPersonajes.Count, WorldCreator.Visualizacion.NoMostrar);
        for(int i = 0; i < t.visualizacionHojas.Count; i++)
        {
            EditorGUILayout.BeginVertical("Button");
            if (t.hojasPersonajes[i] == null) t.hojasPersonajes[i] = new HojaPersonaje();
            t.visualizacionHojas[i] = (WorldCreator.Visualizacion)EditorGUILayout.EnumPopup("Mostrar hoja " + (i + 1), t.visualizacionHojas[i]);
            if(t.visualizacionHojas[i] == WorldCreator.Visualizacion.Completa)
            {
                HojaCompleta(i);
            }
            else if (t.visualizacionHojas[i] == WorldCreator.Visualizacion.Resumen)
            {

            }
            EditorGUILayout.EndVertical();
        }
    }

    void mundoCompleto()
    {
        EditorGUILayout.BeginVertical("Button");
        if (t.mundo == null) t.mundo = new Mundo();

        EditorGUILayout.LabelField("Parámetros fijos", EditorStyles.boldLabel);
        CommonFuncs.setListSize(t.mundo.parametrosFijos, EditorGUILayout.IntField("Cantidad", t.mundo.parametrosFijos.Count));
        for (int i = 0; i < t.mundo.parametrosFijos.Count; i++)
        {
            t.mundo.parametrosFijos[i] = EditorGUILayout.TextField((i + 1) + ".", t.mundo.parametrosFijos[i]);
        }
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Parametros variables", EditorStyles.boldLabel);
        CommonFuncs.setListSize(t.mundo.parametrosVariables, EditorGUILayout.IntField("Cantidad", t.mundo.parametrosVariables.Count));
        for (int i = 0; i < t.mundo.parametrosVariables.Count; i++)
        {
            EditorGUILayout.BeginVertical("Button");
            EditorGUILayout.LabelField("Parametro " + (i + 1));
            if (t.mundo.parametrosVariables[i] == null) t.mundo.parametrosVariables[i] = new Mundo.Variable();
            t.mundo.parametrosVariables[i].nombre = EditorGUILayout.TextField("Nombre", t.mundo.parametrosVariables[i].nombre);
            t.mundo.parametrosVariables[i].tieneLimInf = EditorGUILayout.Toggle("Tiene lim. inferior", t.mundo.parametrosVariables[i].tieneLimInf);
            t.mundo.parametrosVariables[i].tieneLimSup = EditorGUILayout.Toggle("Tiene lim. superior", t.mundo.parametrosVariables[i].tieneLimSup);
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Lugares para equipar", EditorStyles.boldLabel);
        CommonFuncs.setListSize(t.mundo.lugaresequip, EditorGUILayout.IntField("Cantidad", t.mundo.lugaresequip.Count));
        for (int i = 0; i < t.mundo.lugaresequip.Count; i++)
        {
            t.mundo.lugaresequip[i] = EditorGUILayout.TextField((i + 1) + ".", t.mundo.lugaresequip[i]);
        }
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Tipos de bonos", EditorStyles.boldLabel);
        CommonFuncs.setListSize(t.mundo.bonos, EditorGUILayout.IntField("Cantidad", t.mundo.bonos.Count));
        for (int i = 0; i < t.mundo.bonos.Count; i++)
        {
            t.mundo.bonos[i] = EditorGUILayout.TextField((i + 1) + ".", t.mundo.bonos[i]);
        }
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Cálculos", EditorStyles.boldLabel);
        CommonFuncs.setListSize(t.mundo.calculos, EditorGUILayout.IntField("Cantidad", t.mundo.calculos.Count));
        for (int i = 0; i < t.mundo.calculos.Count; i++)
        {
            EditorGUILayout.BeginVertical("Button");
            EditorGUILayout.LabelField("Calculo " + (i + 1));
            if (t.mundo.calculos[i] == null) t.mundo.calculos[i] = new Mundo.Calculo();
            t.mundo.calculos[i].nombre = EditorGUILayout.TextField("Nombre", t.mundo.calculos[i].nombre);
            t.mundo.calculos[i].textoDeCalculo = EditorGUILayout.TextField("Texto cálculo", t.mundo.calculos[i].textoDeCalculo);
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.Space();


        EditorGUILayout.EndVertical();
    }

    void mundoResumen()
    {
        EditorGUILayout.BeginVertical("Button");
        EditorGUILayout.LabelField("Parametros fijos (" + t.mundo.parametrosFijos.Count + ")", EditorStyles.boldLabel);
        EditorGUILayout.TextArea(CommonFuncs.getListToString(t.mundo.parametrosFijos, 55));
        EditorGUILayout.LabelField("Parametros Variables (" + t.mundo.parametrosVariables.Count + ")", EditorStyles.boldLabel);
        EditorGUILayout.TextArea(CommonFuncs.getListToString(t.mundo.parametrosVariables, 55));
        EditorGUILayout.LabelField("Lugares para equipar (" + t.mundo.lugaresequip.Count + ")", EditorStyles.boldLabel);
        EditorGUILayout.TextArea(CommonFuncs.getListToString(t.mundo.lugaresequip, 55));
        EditorGUILayout.LabelField("Tipos de bonos (" + t.mundo.bonos.Count + ")", EditorStyles.boldLabel);
        EditorGUILayout.TextArea(CommonFuncs.getListToString(t.mundo.bonos, 55));
        EditorGUILayout.LabelField("Tipos de bonos (" + t.mundo.calculos.Count + ")", EditorStyles.boldLabel);
        EditorGUILayout.TextArea(CommonFuncs.getListToString(t.mundo.calculos, 55));
        EditorGUILayout.EndVertical();
    }

    void HojaCompleta(int h)
    {
        HojaPersonaje hoja = t.hojasPersonajes[h];
        hoja.nombre = EditorGUILayout.TextField("Nombre", hoja.nombre);
        CommonFuncs.setListSize(hoja.parametros, EditorGUILayout.IntField("Cantidad", hoja.parametros.Count));
        for (int i = 0; i < hoja.parametros.Count; i++) {
            EditorGUILayout.BeginVertical("Button");
            if (hoja.parametros[i] == null) hoja.parametros[i] = new Parametro();
            CommonFuncs.setListSize(t.mostrarParametro, hoja.parametros.Count, false);
            CommonFuncs.setListSize(t.calculoParametro, hoja.parametros.Count);

            EditorGUILayout.BeginHorizontal();
            t.mostrarParametro[i] = EditorGUILayout.Toggle(t.mostrarParametro[i], GUILayout.Width(20));
            hoja.parametros[i].idParametro = EditorGUILayout.Popup("Parámetro", hoja.parametros[i].idParametro, t.mundo.parametrosFijos.ToArray());
            EditorGUILayout.EndHorizontal();

            if (t.mostrarParametro[i])
            {
                hoja.parametros[i].upgradeable = (Parametro.Upgradeable)EditorGUILayout.EnumPopup("Mejorable", hoja.parametros[i].upgradeable);
                if (hoja.parametros[i].upgradeable == Parametro.Upgradeable.unoEnUno)
                {
                    if (hoja.parametros[i].valor.Count != 1)
                    {
                        hoja.parametros[i].valor = new List<int>();
                        hoja.parametros[i].valor.Add(0);
                    }
                    hoja.parametros[i].valor[0] = EditorGUILayout.IntField("Valor inicial", hoja.parametros[i].valor[0]);
                }
                else if (hoja.parametros[i].upgradeable == Parametro.Upgradeable.lista)
                {
                    EditorGUILayout.BeginHorizontal();
                    t.calculoParametro[i] = EditorGUILayout.TextField(t.calculoParametro[i]);
                    if (GUILayout.Button("Calcular parámetros"))
                    {
                        for (int j = 0; j < hoja.parametros[i].valor.Count; j++)
                        {
                            hoja.parametros[i].valor[j] = CommonFuncs.evaluateCalc(t.calculoParametro[i], j);
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    CommonFuncs.setListSize(hoja.parametros[i].valor, EditorGUILayout.IntField("Niveles", hoja.parametros[i].valor.Count), 0);
                    for (int j = 0; j < hoja.parametros[i].valor.Count; j++)
                    {
                        EditorGUILayout.BeginVertical("Button");
                        hoja.parametros[i].valor[j] = EditorGUILayout.IntField("Nivel " + (j + 1), hoja.parametros[i].valor[j]);
                        EditorGUILayout.EndVertical();
                    }
                }
            }
            EditorGUILayout.EndVertical();
        }
    }
}
