using UnityEngine;

[CreateAssetMenu(fileName = "New Skill Data", menuName = "ScriptableObjects/SkillDataSO")]
public class SkillDataSO : ScriptableObject
{
    public string skillName;
    public string description;
    public Sprite icon;
    public float cooldown;
    public float effectDuration;
    public float damageMultiplier;
}
