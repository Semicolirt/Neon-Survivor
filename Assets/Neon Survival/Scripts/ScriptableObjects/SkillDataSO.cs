using UnityEngine;

[CreateAssetMenu(fileName = "New Skill Data", menuName = "ScriptableObjects/SkillDataSO")]
public class SkillDataSO : ScriptableObject
{
    public string skillName;
    public string description;
    public Sprite icon;
    public float cooldown;
    public float effectDuration;
    public float damageMultiplier = 1f;
    public float speedPlus = 1f;
    public float rangePlus = 1f;
    public float areaOfEffectRadius = 1f;
    public float maxHealthPlus = 1f;
}
