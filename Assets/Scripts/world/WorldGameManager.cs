using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.InputSystem;

//World scene part of old GameManager class

public class WorldGameManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text energyText;
    public TMP_Text EnergyAddText;
    public TMP_Text Cost1Text;

    public TMP_Text ticketsText;
    public TMP_Text TicketsAddText;
    public TMP_Text Cost2Text;

    public TMP_Text WorldLevelText;
    public GameObject StorePanel;

    //import
    ResourceManager resxmgr;
    MiniMenu menu;


    void Start()
    {
        resxmgr=FindAnyObjectByType<ResourceManager>();
        menu=FindAnyObjectByType<MiniMenu>();
        menu.HideAllMenus();
    }
    void  Update()
    {
        if(Time.timeScale!=0f){
            UiUpdate();
            if(Keyboard.current.tKey.wasPressedThisFrame)
            {
                if(StorePanel.activeSelf)
                {
                    closeStore();
                }
                else
                {
                    openStore();
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape)){
            menu.ShowMenu(0, true);
        }
    }

    public void UiUpdate()
    {
        // Actualizar UI con los recursos
        foreach(ResourceData resource in resxmgr.resources)
        {
            if(resource.resourceName ==
                "Energy")
            {
                energyText.text =
                    "Energy: " +
                    resource.resourceAmount;

                EnergyAddText.text =
                    "+" +
                    resource.resourceAddAmount +
                    " /s";
            }
            else if(resource.resourceName ==
                "Tickets")
            {
                ticketsText.text =
                    "Tickets: " +
                    resource.resourceAmount;

                TicketsAddText.text =
                    "+" +
                    resource.resourceAddAmount +
                    " /s";
            }
        }
        Cost1Text.text =
            "Upgrade Energy: " +
            resxmgr.cost1 +
            " Energy";
        Cost2Text.text =
            "Upgrade Tickets: " +
            resxmgr.cost2 +
            " Tickets";
        WorldLevelText.text =
            "World Level: " +
            resxmgr.worldLevelCost;

    }
    public void closeStore()
    {
        StorePanel.SetActive(false);
    }
    public void openStore()
    {
        StorePanel.SetActive(true);
    }
}