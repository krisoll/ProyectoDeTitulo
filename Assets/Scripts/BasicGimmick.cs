using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class BasicGimmick : MonoBehaviour {
    public int waveNumber = 0;
    public int waveNumberD = 0;
    public GameObject terrain= null;
    public GameObject backgroundQuad = null;
    public float damping = 0.01f;
    public float velocidad = 0;
    public GimmickType gimmickType = new GimmickType();
    [SerializeField]
    public List<GimmickWave> waves = new List<GimmickWave>();
    public List<int> numEnemigos = new List<int>();
    public List<int> numEnemigosD = new List<int>();
    public List<GameObject> gameObject1 = new List<GameObject>();
    public List<GameObject> gameObject2 = new List<GameObject>();
    public List<float> spawnDelay = new List<float>();
    public List<GameObject> spawnPoints = new List<GameObject>();
    private GameObject terrain1, terrain2;
    private Vector2 posLado;
    public enum WaveType
    {
        NONE,
        ACTIVATEOBJECT,
        CHANGETERRAIN,
        CHANGESPAWNPOINT
    }
    public enum GimmickType
    {
        NONE,
        STATIC,
        MOVING
    }
    public enum SpawnPointType
    {
        NONE,
        BETWEEN2POINTS,
        RANDOMBETWEENPOINTS,
    }
    public SpawnPointType spawnPointType = new SpawnPointType();
    public List<WaveType> wavetypes = new List<WaveType>();
    private int currentWave = 0;
    private int destroyCount = 0;
    private bool finalizado = false;
    private bool comenzo = false;
    private float velX;
    private float velY;
    private Vector2 tamanhoTerrain;
    private float contVelX;
    private float contVelY;
    private GameObject puntoAMover;
	// Use this for initialization
	void Start () {
        //if (Manager.gManager.player.transform.parent != null) Manager.gManager.player.transform.parent = null;
        for (int i = 0; i < waves[currentWave].enemigos.Count; i++)
        {
            spawnEnemies(i);
        }
        ImpulsoAngular(transform.eulerAngles.z);
	}
	
	// Update is called once per frame
    void FixedUpdate()
    {
        if (comenzo)
        {
            backgroundQuad.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(-contVelX*damping, -contVelY*damping);
            contVelX += velX;
            contVelY += velY;
            terrain1.transform.position = new Vector2(terrain1.transform.position.x+velX, terrain1.transform.position.y+velY);
            terrain2.transform.position = new Vector2(terrain2.transform.position.x + velX, terrain2.transform.position.y + velY);
            if (velocidad < 0)
            {
                if (terrain1.transform.localPosition.x <= -tamanhoTerrain.x)
                {
                    terrain1.transform.localPosition = new Vector2(terrain2.transform.localPosition.x + tamanhoTerrain.x, terrain2.transform.localPosition.y);
                }
                if (terrain2.transform.localPosition.x <= -tamanhoTerrain.x)
                {
                    terrain2.transform.localPosition = new Vector2(terrain1.transform.localPosition.x + tamanhoTerrain.x, terrain1.transform.localPosition.y);
                }
            }
            else
            {
                if (terrain1.transform.localPosition.x >= tamanhoTerrain.x)
                {
                    terrain1.transform.localPosition = new Vector2(terrain2.transform.localPosition.x + tamanhoTerrain.x, terrain2.transform.localPosition.y);
                }
                if (terrain2.transform.localPosition.x >= tamanhoTerrain.x)
                {
                    terrain2.transform.localPosition = new Vector2(terrain1.transform.localPosition.x + tamanhoTerrain.x, terrain1.transform.localPosition.y);
                }
            }
        }
    }
	void Update () {
        GimmickTypeSelect();
	}
    public void GimmickTypeSelect()
    {
        if (gimmickType == GimmickType.NONE)
        {
        }
        else if (gimmickType == GimmickType.STATIC)
        {
        }
        else if (gimmickType == GimmickType.MOVING)
        {
            if (!comenzo)
            {
                tamanhoTerrain = terrain.GetComponent<BoxCollider2D>().size;
                terrain1 = terrain;
                //float posX = (tamanhoTerrain.x) * Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad);
                //float posY = (tamanhoTerrain.y) * Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad);
                //posLado = new Vector2(terrain.transform.position.x+posX,terrain.transform.position.y+posY);
                terrain2 = (GameObject)Instantiate(terrain, posLado, Quaternion.identity);
                terrain2.transform.parent = transform;
                if(velocidad<0)terrain2.transform.localPosition = new Vector2(terrain1.transform.localPosition.x+tamanhoTerrain.x, terrain1.transform.localPosition.y);
                else terrain2.transform.localPosition = new Vector2(terrain1.transform.localPosition.x + tamanhoTerrain.x, terrain1.transform.localPosition.y);
                terrain2.transform.localEulerAngles = new Vector3(0, 0, 0);
                comenzo = true;
            }
        }
        waveClearCheck();
    }
    void waveClearCheck()
    {
        if (currentWave < waves.Count)
        {
            if (waves[currentWave].enemigos.Count > destroyCount)
            {
                for (int i = 0; i < waves[currentWave].enemigos.Count; i++)
                {
                    if (waves[currentWave].enemigos[i] == null)
                    {
                        destroyCount += 1;
                    };
                }
                if (waves[currentWave].enemigos.Count > destroyCount) destroyCount = 0;
            }
            else
            {
                currentWave += 1;
                if (currentWave <= waves.Count - 1)
                {
                    for (int i = 0; i < waves[currentWave].enemigos.Count; i++)
                    {
                        spawnEnemies(i);
                    }
                }
                destroyCount = 0;
            }
        }
        if (currentWave >= waves.Count && !finalizado)
        {
            finalizado = true;
        }
    }
    public void ImpulsoAngular(float angulo)
    {
        float angle = angulo * Mathf.Deg2Rad;
        velY = velocidad * Mathf.Sin(angle);
        velX = velocidad * Mathf.Cos(angle);
    }
    private void spawnEnemies(int i)
    {
        if (spawnPointType == SpawnPointType.BETWEEN2POINTS)
        {
            float randomY = 0;
            float randomX = 0;
            if (spawnPoints[0].transform.position.x <= spawnPoints[1].transform.position.x) randomX = Random.Range(spawnPoints[0].transform.position.x, spawnPoints[1].transform.position.x);
            else randomX = Random.Range(spawnPoints[1].transform.position.x, spawnPoints[0].transform.position.x);
            if (spawnPoints[0].transform.position.y <= spawnPoints[1].transform.position.y) randomY = Random.Range(spawnPoints[0].transform.position.y, spawnPoints[1].transform.position.y);
            else randomY = Random.Range(spawnPoints[1].transform.position.y, spawnPoints[0].transform.position.y);
            waves[currentWave].enemigos[i] = (GameObject)Instantiate(waves[currentWave].enemigos[i], new Vector2(randomX, randomY), Quaternion.identity);
        }
        else if (spawnPointType == SpawnPointType.RANDOMBETWEENPOINTS)
        {
            int random = Random.Range(0, spawnPoints.Count);
            waves[currentWave].enemigos[i] = (GameObject)Instantiate(waves[currentWave].enemigos[i], spawnPoints[random].transform.position, Quaternion.identity);
        }
    }
    [System.Serializable]
    public class GimmickWave
    {
        public List<GameObject> enemigos = new List<GameObject>();
        public List<GameObject> spawnPoints = new List<GameObject>();
    }
}
