using UnityEngine;
using DG.Tweening;

public class DropItem : MonoBehaviour
{
    [Header("Bounce Settings")]
    [SerializeField] private float bounceHeight = 0.55f;
    [SerializeField] private float bounceDuration = 0.45f;
    [SerializeField] private Ease bounceEaseUp = Ease.OutQuad;
    [SerializeField] private Ease bounceEaseDown = Ease.InQuad;

    [Header("Magnet Settings")]
    [SerializeField] private float magnetSpeed = 8f;
    [SerializeField] private float pickupRange = 1.5f;
    private bool isMagneting = false;

    private Transform visual;
    private Transform playerTransform;
    private DropItemDataSO itemData;
    private GameObject originalPrefab;

    private void Awake()
    {
        var sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            visual = sr.transform;
        }
    }

    public void OnSpawn(DropItemDataSO data, GameObject prefab, Vector3 spawnPosition)
    {
        itemData = data;
        originalPrefab = prefab;
        
        // Randomize spawn position slightly around death location for visual variance
        Vector3 offset = new Vector3(Random.Range(-0.4f, 0.4f), Random.Range(-0.4f, 0.4f), 0f);
        transform.position = spawnPosition + offset;
        
        isMagneting = false;
        playerTransform = null;

        // Play bounce animation
        PlayBounceEffect();

        if (visual != null) visual.localScale = Vector3.one;
    }

    private void PlayBounceEffect()
    {
        if (visual != null && visual != transform)
        {
            visual.DOKill();
            visual.localPosition = Vector3.zero;

            Sequence seq = DOTween.Sequence();
            var position = new Vector2(0, bounceHeight);
            var positionDown = new Vector2(0, 0);

            seq.Append(visual.DOLocalMove(position, bounceDuration * 0.5f).SetEase(bounceEaseUp));
            seq.Append(visual.DOLocalMove(positionDown, bounceDuration * 0.5f).SetEase(bounceEaseDown));
        }
        else
        {
            // If SpriteRenderer is on the root, animate world position to avoid snapping to pool's origin (0,0)
            transform.DOKill();
            transform.DOJump(transform.position, bounceHeight, 1, bounceDuration);
        }
    }

    void Update()
    {
        HandleMagnetAndPickup();
    }

    void HandleMagnetAndPickup()
    {
        if (isMagneting && playerTransform != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, magnetSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, playerTransform.position) < pickupRange)
            {
                ApplyEffect(playerTransform.gameObject);
                DespawnItem();
            }
        }
    }

    public void StartMagnet(Transform player)
    {
        if (isMagneting) return;
        isMagneting = true;
        playerTransform = player;

        if (visual != null)
            visual.DOScale(0.3f, 0.3f).SetEase(Ease.Linear);
    }

    private void ApplyEffect(GameObject player)
    {
        if (itemData == null) return;

        switch (itemData.itemType)
        {
            case DropItemType.Health:
                HealthComponent hc = player.GetComponent<HealthComponent>();
                if (hc != null)
                {
                    hc.Heal(itemData.value);
                }
                break;

            case DropItemType.Magnet:
                // Pull all experience orbs and other drop items
                TriggerGlobalMagnet(player.transform);
                break;

            case DropItemType.WeaponBoost:
                // Apply temporary attack boost to the player
                PlayerController pc = player.GetComponent<PlayerController>();
                if (pc != null)
                {
                    pc.ApplyWeaponBoost(itemData.value, 0.5f); // 0.5f means twice as fast fire rate
                }
                break;
        }
    }

    private void TriggerGlobalMagnet(Transform player)
    {
        // Find all ExpOrbs and trigger magnet
        ExpOrb[] expOrbs = FindObjectsByType<ExpOrb>(FindObjectsSortMode.None);
        foreach (var orb in expOrbs)
        {
            orb.StartMagnet(player);
        }

        // Find all DropItems and trigger magnet
        DropItem[] dropItems = FindObjectsByType<DropItem>(FindObjectsSortMode.None);
        foreach (var item in dropItems)
        {
            item.StartMagnet(player);
        }
    }

    private void DespawnItem()
    {
        if (originalPrefab != null && ObjectPoolManager.Instance != null)
        {
            ObjectPoolManager.Instance.Despawn(originalPrefab, gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}
