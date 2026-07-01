using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

//v1.0
//persistent/resource part of old GameManager class

public class ResourceManager : MonoBehaviour
{
    [Header("Recursos y costos")]
    public List<ResourceData> resources = new List<ResourceData>();
    public float levelUpCostMultiplier = 1.5f;
    public float cost1 = 1000f;
    public float cost2 = 15000f;
    public float worldLevelCost = 50000f;
    public int worldLevel;
    private string savePath;
    //persistent
    static ResourceManager instance;



    //exe
    void Awake()
    {
        //remove dupes
        if(instance!=null && instance!=this){
            Debug.LogWarning("duped ResourceManager "+gameObject.name+" seppuku'd");
            Destroy(gameObject);
        }else{
            instance=this;
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start(){
        savePath = Application.persistentDataPath +"/save.json";

        // Verificar si existe save
        if(File.Exists(savePath))
        {
            LoadGame();
        }
        else
        {
            CreateNewGame();
            SaveGame();
        }

        StartCoroutine(ResourceGeneration());
    }
    void Update(){
        
    }

    //funx
    #region §saves
    // Crear juego nuevo
    void CreateNewGame()
    {
        resources.Clear();

        CreateResource("Tickets", 0, 5);
        CreateResource("Energy", 0, 10);

        levelUpCostMultiplier = 1.5f;
        cost1 = 1000f;
        cost2 = 15000f;
        worldLevel = 1;

        Debug.Log("Nuevo juego creado");
    }
    // Guardar juego
    public void SaveGame()
    {
        SaveData saveData = new SaveData();

        saveData.resources = resources;

        // Guardar datos nuevos
        saveData.levelUpCostMultiplier = levelUpCostMultiplier;
        saveData.cost1 = cost1;
        saveData.cost2 = cost2;
        saveData.worldLevel = worldLevel;

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(savePath, json);

        Debug.Log("Juego guardado");
    }
    // Cargar juego
    public void LoadGame()
    {
        if(File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);

            SaveData saveData = JsonUtility.FromJson<SaveData>(json);

            resources = saveData.resources;

            levelUpCostMultiplier = saveData.levelUpCostMultiplier;

            cost1 = saveData.cost1;
            cost2 = saveData.cost2;
            worldLevel = saveData.worldLevel;
        }
    }
    #endregion §saves
    #region §resource
    // Crear recurso
    void CreateResource(
        string name,
        float amount,
        float addAmount
    )
    {
        ResourceData newResource = new ResourceData();
        newResource.resourceName = name;
        newResource.resourceAmount = amount;
        newResource.resourceAddAmount = addAmount;
        resources.Add(newResource);
    }
    // Generación automática
    IEnumerator ResourceGeneration()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);

            foreach(ResourceData resource in resources)
            {
                resource.resourceAmount += resource.resourceAddAmount;

                Debug.Log(resource.resourceName + ": " + resource.resourceAmount);
            }

            // Guardar automáticamente
            SaveGame();
        }
    }
    // Añadir recurso (para minijuegos)
    public void AddResource(string resourceName, int amount){
        ResourceData resx=GetResource(resourceName);
        if(resx!=null){
            resx.resourceAmount+=amount;
            Debug.Log("Added "+amount+" to "+resx.resourceName);
        }else{
            Debug.LogError("Couldnt find "+resourceName);
        }
    }
    // Obtener recurso
    public ResourceData GetResource(
        string resourceName
    )
    {
        foreach(ResourceData resource in resources)
        {
            if(resource.resourceName == resourceName)
            {
                return resource;
            }
        }

        return null;
    }
    #endregion §resource
    public void OnApplicationQuit()
    {
        SaveGame();
    }
    public void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            SaveGame();
        }
    }
    public void LevelUpResource1()
    {
        if(GetResource("Energy").resourceAmount >= cost1)
        {
            GetResource("Energy").resourceAmount -= cost1;

            // Aumenta regeneración
            GetResource("Energy").resourceAddAmount *=
                levelUpCostMultiplier;

            // Aumenta cantidad actual
            GetResource("Energy").resourceAmount *=
                levelUpCostMultiplier;

            // Aumenta costo del siguiente upgrade
            cost1 *= levelUpCostMultiplier;
            SaveGame();

        }
        else
        {
            ShowError("No tienes suficiente energia!");
        }
    }
    public void LevelUpResource2()
    {
        if(GetResource("Tickets").resourceAmount >= cost2)
        {
            GetResource("Tickets").resourceAmount -= cost2;

            // Aumenta regeneración
            GetResource("Tickets").resourceAddAmount *=
                levelUpCostMultiplier;

            // Aumenta cantidad actual
            GetResource("Tickets").resourceAmount *=
                levelUpCostMultiplier;

            // Aumenta costo siguiente
            cost2 *= levelUpCostMultiplier;
            SaveGame();
        }
        else
        {
            ShowError("No tienes suficientes tickets!");
        }
    }
    public void LevelUpWorld()
    {
        if(worldLevel!=0)
        {
            worldLevel++;
            worldLevel%=int.MaxValue;
            SaveGame();
        }
        else
        {
            ShowError("Tu Parque no puede subir mas de nivel!");
        }
    }
    //helper
    void ShowError(string message)
    {
        UIMsg uimsg=FindAnyObjectByType<UIMsg>();
        uimsg.AddLog(message, 2f);
    }
}
