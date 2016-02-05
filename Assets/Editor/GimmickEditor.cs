using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
[System.Serializable]
[CustomEditor(typeof(BasicGimmick))]
public class GimmickEditor : Editor {
    [SerializeField]
    private BasicGimmick g;
    private bool points2 = false;
	// Use this for initialization
	void Awake() {
        g = (BasicGimmick)target;
	}
	
	// Update is called once per frame
    public override void OnInspectorGUI()
    {
        if (g.waveNumber == 0) reset();
        //DrawDefaultInspector();
        //if (GUILayout.Button("Reset")) reset();
        g.gimmickType = (BasicGimmick.GimmickType)EditorGUILayout.EnumPopup("Gimmick Type", g.gimmickType);
        if (g.gimmickType != BasicGimmick.GimmickType.STATIC && g.gimmickType != BasicGimmick.GimmickType.NONE)
        {
            EditorGUILayout.HelpBox("No puede ser una prefab, debe ser un objeto de la escena", MessageType.Info);
            g.terrain = (GameObject)EditorGUILayout.ObjectField("Terrain", g.terrain, typeof(GameObject), true);
            g.velocidad = EditorGUILayout.FloatField("Velocity", g.velocidad);
            g.backgroundQuad = (GameObject)EditorGUILayout.ObjectField("Background", g.backgroundQuad, typeof(GameObject), true);
            g.damping = EditorGUILayout.FloatField("Background Damping", g.damping);
        }
        EditorGUILayout.Space();
        g.waveNumber = EditorGUILayout.IntSlider("Number of Waves",g.waveNumber,0,20);
        if (g.waveNumberD < g.waveNumber) AddWave(g.waveNumber - g.waveNumberD);
        if (g.waveNumberD > g.waveNumber) RemoveWave(g.waveNumber);
        EditorGUILayout.BeginVertical("Button");
        g.spawnPointType = (BasicGimmick.SpawnPointType)EditorGUILayout.EnumPopup("SpawnPoint Type", g.spawnPointType);
        if (g.spawnPointType == BasicGimmick.SpawnPointType.NONE)
        {
            if (g.spawnPoints.Count != 0) RemoveSpawnPoint();
        }
        else if (g.spawnPointType == BasicGimmick.SpawnPointType.RANDOMBETWEENPOINTS)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Point"))
            {
                AddSpawnPoint();
            }
            if (GUILayout.Button("Remove Point") && g.spawnPoints.Count > 0)
            {
                RemoveSpawnPoint();
            }
            EditorGUILayout.EndHorizontal();
        }
        else if (g.spawnPointType == BasicGimmick.SpawnPointType.BETWEEN2POINTS)
        {
            if (g.spawnPoints.Count < 2)
            {
                AddSpawnPoint();
            }
        }
        for (int i = 0; i < g.spawnPoints.Count; i++)
        {
            g.spawnPoints[i] = (GameObject)EditorGUILayout.ObjectField("Point " + (i + 1), g.spawnPoints[i], typeof(GameObject), true);
        }
        for (int i = 0; i < g.waveNumber; i++)
        {
            if (g.numEnemigosD[i] < g.numEnemigos[i]) AddEnemies(i,g.numEnemigos[i] - g.numEnemigosD[i]);
            if (g.numEnemigosD[i] > g.numEnemigos[i]) RemoveEnemies(i,g.numEnemigos[i]);
            EditorGUILayout.LabelField("Wave " + (i + 1), EditorStyles.boldLabel);
            g.numEnemigos[i] = EditorGUILayout.IntSlider("Number Of Enemies", g.numEnemigos[i], 0, 40);
            EditorGUILayout.BeginVertical("Button");
            g.wavetypes[i] =  (BasicGimmick.WaveType)EditorGUILayout.EnumPopup("Wave Type", g.wavetypes[i]);
            if (g.wavetypes[i] == BasicGimmick.WaveType.NONE)
            {

            }
            else if (g.wavetypes[i] == BasicGimmick.WaveType.ACTIVATEOBJECT)
            {
                g.gameObject1[i] = (GameObject)EditorGUILayout.ObjectField("Object To Activate", g.gameObject1[i], typeof(GameObject), true);
            }
            else if (g.wavetypes[i] == BasicGimmick.WaveType.CHANGETERRAIN)
            {
                g.gameObject1[i] = (GameObject)EditorGUILayout.ObjectField("New Terrain", g.gameObject1[i], typeof(GameObject), true);
            }
            else if (g.wavetypes[i] == BasicGimmick.WaveType.CHANGESPAWNPOINT)
            {
                g.gameObject1[i] = (GameObject)EditorGUILayout.ObjectField("New Spawnpoint", g.gameObject1[i], typeof(GameObject), true);
            }
            EditorGUILayout.EndVertical();
            for (int r = 0; r < g.waves[i].enemigos.Count; r++)
            {
                g.waves[i].enemigos[r] = (GameObject)EditorGUILayout.ObjectField("Enemigo " + (r + 1), g.waves[i].enemigos[r], typeof(GameObject), true);
            }
        }
        EditorGUILayout.EndVertical();
        if (GUI.changed) { EditorUtility.SetDirty(target); }
	}
    private void AddSpawnPoint()
    {
        GameObject gr = new GameObject();
        gr.name = "p" + (g.spawnPoints.Count + 1);
        gr.transform.position = g.transform.position;
        gr.transform.parent = g.transform;
        g.spawnPoints.Add(gr);
    }
    private void RemoveSpawnPoint()
    {
        List<GameObject> objS = g.spawnPoints;
        DestroyImmediate(g.spawnPoints[g.spawnPoints.Count - 1]);
        g.spawnPoints = new List<GameObject>();
        for (int r = 0; r < objS.Count - 1; r++)
        {
            g.spawnPoints.Add(objS[r]);
        }
    }
    private void AddEnemies(int i,int numEnemigos)
    {
        for (int r = 0; r < numEnemigos; r++)
        {
            g.waves[i].enemigos.Add(null);
        }
        g.numEnemigosD[i] = g.numEnemigos[i];
    }
    private void RemoveEnemies(int i, int numEnemigos)
    {
        List<GameObject> enemiesS = g.waves[i].enemigos;
        g.waves[i].enemigos = new List<GameObject>();
        for (int r = 0; r < numEnemigos; r++)
        {
            g.waves[i].enemigos.Add(enemiesS[r]);
        }
        g.numEnemigosD[i] = g.numEnemigos[i];
    }
    private void AddWave(int add)
    {
        for (int i = 0; i < add; i++)
        {
            g.numEnemigos.Add(0);
            g.numEnemigosD.Add(0);
            g.gameObject1.Add(null);
            g.gameObject2.Add(null);
            g.wavetypes.Add(BasicGimmick.WaveType.NONE);
            g.waves.Add(new BasicGimmick.GimmickWave());
            g.spawnDelay.Add(0);
        }
        g.waveNumberD = g.waveNumber;
    }
    private void RemoveWave(int rem)
    {
        List<int> numEnemigosS = g.numEnemigos;
        List<int> numEnemigosDS = g.numEnemigosD;
        List<BasicGimmick.WaveType> typeS = g.wavetypes;
        List<BasicGimmick.GimmickWave> waveS = g.waves;
        List<GameObject> gameObject1S = g.gameObject1;
        List<GameObject> gameObject2S = g.gameObject2;
        List<float> spawnDelayS = g.spawnDelay;
        g.numEnemigos = new List<int>();
        g.numEnemigosD = new List<int>();
        g.waves = new List<BasicGimmick.GimmickWave>();
        g.wavetypes = new List<BasicGimmick.WaveType>();
        g.gameObject1 = new List<GameObject>();
        g.gameObject2 = new List<GameObject>();
        g.spawnDelay = new List<float>();
        for (int i = 0; i < rem; i++)
        {
            g.numEnemigos.Add(numEnemigosS[i]);
            g.numEnemigosD.Add(numEnemigosDS[i]);
            g.wavetypes.Add(typeS[i]);
            g.waves.Add(waveS[i]);
            g.gameObject1.Add(gameObject1S[i]);
            g.gameObject2.Add(gameObject2S[i]);
            g.spawnDelay.Add(spawnDelayS[i]);
        }
        g.waveNumberD = g.waveNumber;
    }
    private void reset()
    {
        g.waveNumber = 0;
        g.waveNumberD = 0;
        g.numEnemigos = new List<int>();
        g.numEnemigosD = new List<int>();
        g.waves = new List<BasicGimmick.GimmickWave>();
        g.wavetypes = new List<BasicGimmick.WaveType>();
        g.gameObject1 = new List<GameObject>();
        g.gameObject2 = new List<GameObject>();
        g.spawnDelay = new List<float>();
    }
}
