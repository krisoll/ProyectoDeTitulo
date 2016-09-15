using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(HojaPersonajeV1))]
public class HojaPersonajeEditorV1 : Editor {
    HojaPersonajeV1 t;
    //Nuevo parametro
    string nombreNuevoParam;
    ParametroV1.TipoParametro tipoNuevoParametro;
    int elimParam;

    string nombreNuevaHab;
    int addParamHab;
    int removeParamHab;
    int elimHab;
    int selectedParam;
    string form1, form2;
    
    [System.Serializable]
    enum mostrar
    {
        Resumen,
        Completo
    }
    mostrar mParametro;
    mostrar mGestor;

    void Awake()
    {
        t = (HojaPersonajeV1)target;
    }

    // Update is called once per frame
    public override void OnInspectorGUI() {
        //Nombre clase/raza de la hoja
        t.nombre = EditorGUILayout.TextField("Nombre", t.nombre);
        //Como se manejarán los parámetros
        t.cambiarTipo((HojaPersonajeV1.TipoParametros)EditorGUILayout.EnumPopup("Tipo de parámetros", t.tipo));
        if (t.tipo == HojaPersonajeV1.TipoParametros.unNivelATodosLosParametros || t.tipo == HojaPersonajeV1.TipoParametros.UnNivelAUnParametro)
        {
            parametros();
        }
        else if (t.tipo == HojaPersonajeV1.TipoParametros.UnNivelAUnaHabilidad)
        {
            parametros();
            gestores();
        }
	}

    void parametros()
    {
        EditorGUILayout.BeginVertical("Button");
        EditorGUILayout.LabelField("Parámetros", EditorStyles.boldLabel);
        mParametro = (mostrar)EditorGUILayout.EnumPopup("Mostrar:", mParametro);
        if (mParametro == mostrar.Completo)
        {
            //Crear nuevo parametro
            EditorGUILayout.BeginVertical("Button");
            nombreNuevoParam = EditorGUILayout.TextField("Nombre parámetro", nombreNuevoParam);
            tipoNuevoParametro = (ParametroV1.TipoParametro)EditorGUILayout.EnumPopup("Tipo de parámetros", tipoNuevoParametro);
            if (GUILayout.Button("Añadir parametro"))
            {
                ParametroV1 p = new ParametroV1();
                p.nombre = nombreNuevoParam;
                p.tipoParametro = tipoNuevoParametro;
                t.parametros.Add(p);
            }
            EditorGUILayout.EndVertical();
            //Eliminar parametro
            if (t.parametros.Count > 0)
            {
                EditorGUILayout.BeginHorizontal("Button");
                elimParam = EditorGUILayout.IntSlider(elimParam, 0, t.parametros.Count-1);
                if (GUILayout.Button("Eliminar parametro"))
                {
                    if (elimParam >= 0 && elimParam < t.parametros.Count) t.parametros.RemoveAt(elimParam);
                }
                EditorGUILayout.EndHorizontal();
            }
            //Seleccionar parámetro
            selectedParam = EditorGUILayout.Popup("Seleccione un parámetro", selectedParam, getListaParams());

            //Datos a mostrar/cambiar
            if (selectedParam >= 0 && selectedParam < t.parametros.Count)
            {
                EditorGUILayout.LabelField("Parámetro n° " + (selectedParam + 1), EditorStyles.boldLabel);
                t.parametros[selectedParam].nombre = EditorGUILayout.TextField("Nombre", t.parametros[selectedParam].nombre);
                EditorGUILayout.IntField("Nivel", t.parametros[selectedParam].nivel);
                EditorGUILayout.TextField("Valor actual", t.parametros[selectedParam].getValue());
                EditorGUILayout.Space();

                //Formulas
                EditorGUILayout.BeginVertical("Button");
                form1 = EditorGUILayout.TextField("Formula valores", form1);
                if(GUILayout.Button("Calcular valores"))
                {
                    AK.ExpressionSolver expS = new AK.ExpressionSolver();
                    for(int i = 0; i < t.parametros[selectedParam].valorNiveles.Count; i++)
                    {
                        t.parametros[selectedParam].setValueAt(expS.EvaluateExpression(form1.Replace("_N", (i + 1) + ""))+"", i);
                    }
                }
                EditorGUILayout.EndVertical();

                t.parametros[selectedParam].cambiarRequiereValor(EditorGUILayout.Toggle("Requiere puntos", t.parametros[selectedParam].requiereCiertoValor));
                t.parametros[selectedParam].setCount(EditorGUILayout.IntField("Cantidad niveles", t.parametros[selectedParam].valorNiveles.Count));
                for (int j = 0; j < t.parametros[selectedParam].valorNiveles.Count; j++)
                {
                    EditorGUILayout.BeginVertical("Button");
                    EditorGUILayout.LabelField("Nivel " + (j + 1), EditorStyles.boldLabel);
                    t.parametros[selectedParam].setValueAt(EditorGUILayout.TextField("Valor", t.parametros[selectedParam].valorNiveles[j]), j);
                    if (t.parametros[selectedParam].requiereCiertoValor && j < t.parametros[selectedParam].valorNiveles.Count - 1)
                        t.parametros[selectedParam].reqPuntos[j] = EditorGUILayout.IntField("Puntos requeridos", t.parametros[selectedParam].reqPuntos[j]);
                    EditorGUILayout.EndVertical();
                }
            }
        }
        else
        {
            EditorGUILayout.BeginVertical("Button");
            for (int i = 0; i < t.parametros.Count; i++)
            {
                EditorGUILayout.LabelField("Parámetro n° " + (i + 1), EditorStyles.boldLabel);
                EditorGUILayout.TextField("Nombre", t.parametros[i].nombre);
                EditorGUILayout.TextField("nivel", (t.parametros[i].nivel + 1) + "");
                EditorGUILayout.TextField("Valor actual", t.parametros[i].getValue());
            }
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndVertical();
    }

    void gestores()
    {
        EditorGUILayout.BeginVertical("Button");
        EditorGUILayout.LabelField("Habilidades", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("Aumentar una habilidad permite aumentar varios parametros a la vez en distintas cantidades", MessageType.Info);
        mGestor = (mostrar)EditorGUILayout.EnumPopup("Mostrar:", mGestor);
        if (mGestor == mostrar.Completo)
        {
            //Crear nueva habilidad
            EditorGUILayout.BeginVertical("Button");
            nombreNuevaHab = EditorGUILayout.TextField("Nombre habilidad", nombreNuevaHab);
            if (GUILayout.Button("Añadir habilidad"))
            {
                GestorParametros g = new GestorParametros();
                g.nombre = nombreNuevaHab;
                t.gestores.Add(g);
            }
            EditorGUILayout.EndVertical();

            //Eliminar habilidad
            EditorGUILayout.BeginVertical("Button");
            elimHab = EditorGUILayout.IntField("Número habilidad", elimHab);
            if (GUILayout.Button("Eliminar habilidad"))
            {
                if (elimHab >= 0 && elimHab < t.gestores.Count) t.gestores.RemoveAt(elimParam);
            }
            EditorGUILayout.EndVertical();

            //Mostrar habilidades
            EditorGUILayout.BeginVertical("Button");
            for (int i = 0; i < t.gestores.Count; i++)
            {
                EditorGUILayout.LabelField("Habilidad n° " + (i + 1), EditorStyles.boldLabel);
                t.gestores[i].nombre = EditorGUILayout.TextField("Nombre", t.gestores[i].nombre);
                EditorGUILayout.TextField("nivel", (t.gestores[i].nivel + 1) + "");
                t.gestores[i].setCount(EditorGUILayout.IntField("Cantidad niveles", t.gestores[i].totalNiveles));
                //Añadir parámetro
                EditorGUILayout.BeginVertical("Button");
                addParamHab = EditorGUILayout.IntField("Numero parametro", addParamHab);
                if (GUILayout.Button("Añadir parámetro") && addParamHab >= 0 && addParamHab < t.parametros.Count)
                {
                    ParametroV1 p = t.parametros[addParamHab];
                    bool encontrado = false;
                    for(int j=0;j< t.gestores[i].parametros.Count; j++)
                    {
                        if (t.gestores[i].parametros[j].param == p)
                        {
                            encontrado = true;
                            break;
                        }
                    }
                    if (!encontrado) t.gestores[i].addParam(ref p);
                }
                EditorGUILayout.EndVertical();

                //Eliminar parámetro
                EditorGUILayout.BeginVertical("Button");
                removeParamHab = EditorGUILayout.IntField("Número parámetro", removeParamHab);
                if (GUILayout.Button("Quitar parámetro") && removeParamHab >= 0 && removeParamHab < t.gestores[i].parametros.Count)
                {
                    t.gestores[i].parametros.RemoveAt(removeParamHab);
                }
                EditorGUILayout.EndVertical();

                //Mostrar parámetros
                EditorGUILayout.BeginVertical("Button");
                for(int j = 0; j < t.gestores[i].parametros.Count; j++)
                {
                    EditorGUILayout.LabelField("Parámetro " + t.gestores[i].parametros[j].param.nombre, EditorStyles.boldLabel);
                    EditorGUILayout.BeginVertical("Button");
                    for(int k=0;k< t.gestores[i].parametros[j].aumento.Count; k++)
                    {
                        t.gestores[i].parametros[j].aumento[k] = EditorGUILayout.IntField("Aumento nivel "+(k+1), 
                                                                    t.gestores[i].parametros[j].aumento[k]);
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();
        }
        else
        {
            EditorGUILayout.BeginVertical("Button");
            for (int i = 0; i < t.gestores.Count; i++)
            {
                EditorGUILayout.LabelField("Habilidad n° " + (i + 1), EditorStyles.boldLabel);
                EditorGUILayout.TextField("Nombre", t.gestores[i].nombre);
                EditorGUILayout.TextField("nivel", (t.gestores[i].nivel + 1) + "");
                EditorGUILayout.IntField("Cantidad niveles", t.gestores[i].totalNiveles);
                EditorGUILayout.BeginVertical("Button");
                for (int j = 0; j < t.gestores[i].parametros.Count; j++)
                {
                    EditorGUILayout.LabelField("Parámetro " + t.gestores[i].parametros[j].param.nombre, EditorStyles.boldLabel);
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndVertical();
    }

    public string[] getListaParams()
    {
        string[] s = new string[t.parametros.Count];
        for(int i=0;i< t.parametros.Count; i++)
        {
            s[i] = t.parametros[i].nombre;
        }
        return s;
    }
}
