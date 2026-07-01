using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class SaveSystem : MonoBehaviour
{
    private string path;

    public SaveData saveData = new SaveData();

    void Start()
    {
        path = Application.persistentDataPath + "/resources.json";

        // Crear recursos
        R_Energy energy = new R_Energy();
        R_Tickets tickets = new R_Tickets();
        // Agregar recursos a la lista
        saveData.resources.Add(energy);
        saveData.resources.Add(tickets);

        SaveGame();
        LoadGame();
    }

    public void SaveGame()
    {
        string json = JsonUtility.ToJson(saveData, true);

        File.WriteAllText(path, json);

        Debug.Log("Juego Guardado");
        Debug.Log(json);
    }

    public void LoadGame()
    {
        if(File.Exists(path))
        {
            string json = File.ReadAllText(path);

            saveData = JsonUtility.FromJson<SaveData>(json);

            Debug.Log("Juego Cargado");

            foreach(ResourceData resource in saveData.resources)
            {
                Debug.Log(resource.resourceName);
                Debug.Log(resource.resourceAmount);
                Debug.Log(resource.resourceAddAmount);
            }
        }
    }
}