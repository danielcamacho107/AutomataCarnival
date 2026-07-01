using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CreditsData", menuName = "Scriptable Objects/CreditsData")]
public class CreditsData : ScriptableObject
{
    public List<Collaborator> m_CreditsList = new List<Collaborator>();
}
