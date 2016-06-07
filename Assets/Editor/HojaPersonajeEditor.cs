using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(HojaPersonaje))]
public class HojaPersonajeEditor : Editor {
    HojaPersonaje t;
    //Nuevo parametro
    string nombreNuevoParam;
    Parametro.TipoParametro tipoNuevoParametro;
    int elimParam;

    string nombreNuevaHab;
    int addParamHab;
    int removeParamHab;
    int elimHab;
    
    enum mostrar
    {
        Resumen,
        Completo
    }
    mostrar mParametro;
    mostrar mGestor;

    void Awake()
    {
        t = (HojaPersonaje)target;
    }

    // Update is called once per frame
    public override void OnInspectorGUI() {
        t.nombre = EditorGUILayout.TextField("Nombre", t.nombre);
        t.cambiarTipo((HojaPersonaje.TipoParametros)EditorGUILayout.EnumPopup("Tipo de parámetros", t.tipo));
        if (t.tipo == HojaPersonaje.TipoParametros.unNivelATodosLosParametros)
        {
            parametros();
        }
        else if (t.tipo == HojaPersonaje.TipoParametros.UnNivelAUnaHabilidad)
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
            tipoNuevoParametro = (Parametro.TipoParametro)EditorGUILayout.EnumPopup("Tipo de parámetros", tipoNuevoParametro);
            if (GUILayout.Button("Añadir parametro"))
            {
                Parametro p = new Parametro();
                p.nombre = nombreNuevoParam;
                p.tipoParametro = tipoNuevoParametro;
                t.parametros.Add(p);
            }
            EditorGUILayout.EndVertical();
            //Eliminar parametro
            EditorGUILayout.BeginVertical("Button");
            elimParam = EditorGUILayout.IntField("Numero parametro a eliminar", elimParam);
            if (GUILayout.Button("Eliminar parametro"))
            {
                if (elimParam >= 0 && elimParam < t.parametros.Count) t.parametros.RemoveAt(elimParam);
            }
            EditorGUILayout.EndVertical();
            //Datos a mostrar/cambiar
            for (int i = 0; i < t.parametros.Count; i++)
            {
                EditorGUILayout.LabelField("Parámetro n° " + (i + 1), EditorStyles.boldLabel);
                t.parametros[i].nombre = EditorGUILayout.TextField("Nombre", t.parametros[i].nombre);
                EditorGUILayout.TextField("nivel", (t.parametros[i].nivel + 1) + "");
                EditorGUILayout.TextField("Valor actual", t.parametros[i].getValue());
                EditorGUILayout.Space();

                t.parametros[i].cambiarRequiereValor(EditorGUILayout.Toggle("Requiere puntos", t.parametros[i].requiereCiertoValor));
                t.parametros[i].setCount(EditorGUILayout.IntField("Cantidad niveles", t.parametros[i].valorNiveles.Count));
                for (int j = 0; j < t.parametros[i].valorNiveles.Count; j++)
                {
                    EditorGUILayout.BeginVertical("Button");
                    EditorGUILayout.LabelField("Nivel " + (j + 1), EditorStyles.boldLabel);
                    t.parametros[i].setValueAt(EditorGUILayout.TextField("Valor", t.parametros[i].valorNiveles[j]), j);
                    if (t.parametros[i].requiereCiertoValor && j < t.parametros[i].valorNiveles.Count - 1)
                        t.parametros[i].reqPuntos[j] = EditorGUILayout.IntField("Puntos requeridos", t.parametros[i].reqPuntos[j]);
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
                    Parametro p = t.parametros[addParamHab];
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
}
