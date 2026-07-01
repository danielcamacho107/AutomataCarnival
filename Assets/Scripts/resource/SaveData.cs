using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public List<ResourceData> resources = new List<ResourceData>();

      public float levelUpCostMultiplier;
    public float cost1;
    public float cost2;
    public int worldLevel;
}