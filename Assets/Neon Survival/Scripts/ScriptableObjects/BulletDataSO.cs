using UnityEngine;

[CreateAssetMenu(fileName = "BulletData", menuName = "ScriptableObjects/BulletData")]
public class BulletDataSO : ScriptableObject
{
    [Header("Bullet Properties")]
    public string bulletName = "Default Bullet";
    public float speed = 20f;
    public float hitRadius = 0.5f;
    public float damage = 1f;
    public float lifetime = 5f;
}
