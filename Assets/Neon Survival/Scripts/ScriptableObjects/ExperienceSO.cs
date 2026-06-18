using UnityEngine;

[CreateAssetMenu(fileName = "ExperienceData", menuName = "ScriptableObjects/ExperienceData")]
public class ExperienceSO : ScriptableObject
{
    [Header("Experience Settings")]
    public string expName;
    public int expValue;
}
