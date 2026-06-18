using UnityEngine;
using DG.Tweening;

public class ExpOrb : MonoBehaviour
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

    [Header("Exp Data")]
    [SerializeField] private float experience = 10f;

    private Animator animator;
    private Transform visual;
    private Transform playerTransform;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        visual = GetComponentInChildren<SpriteRenderer>().transform;
    }

    public void OnSpawn(Vector3 spawnPosition)
    {
        transform.position = spawnPosition;
        isMagneting = false;

        // Chơi hiệu ứng nảy
        PlayBounceEffect();

        // Reset scale nếu cần
        if (visual != null) visual.localScale = Vector3.one;
    }

    private void PlayBounceEffect()
    {
        // DOTween Bounce (mượt + linh hoạt)
        if (visual != null)
        {
            visual.DOKill(); // Clear tween cũ
            visual.localPosition = Vector3.zero;

            Sequence seq = DOTween.Sequence();
            var position = new Vector2(0, bounceHeight);
            var randomOffset = new Vector2(Random.Range(-1f, 1f), 0);
            position += randomOffset;
            var positionDown = new Vector2(position.x, 0);

            seq.Append(visual.DOLocalMove(position, bounceDuration * 0.5f).SetEase(bounceEaseUp));
            seq.Append(visual.DOLocalMove(positionDown, bounceDuration * 0.5f).SetEase(bounceEaseDown));
        }
    }

    void Update()
    {
        DeSpawnExpOrb();
    }

    void DeSpawnExpOrb()
    {
        if (isMagneting && playerTransform != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, magnetSpeed * Time.deltaTime);

            ExpSpawner expSpawner = FindAnyObjectByType<ExpSpawner>().GetComponent<ExpSpawner>();
            GameObject prefabToDespawn = expSpawner.expPrefab;


            if (Vector2.Distance(transform.position, playerTransform.position) < pickupRange)
            {
                LevelUpManager.Instance.AddExperience(experience);

                ObjectPoolManager.Instance.Despawn(prefabToDespawn, gameObject);
            }
        }
    }

    public void SetExperience(float expValue)
    {
        experience = expValue;
    }

    // Gọi khi Player ở gần (thường qua trigger hoặc OverlapCircle)
    public void StartMagnet(Transform player)
    {
        isMagneting = true;
        playerTransform = player;

        // Hiệu ứng hút (scale down)
        if (visual != null)
            visual.DOScale(0.3f, 0.3f).SetEase(Ease.Linear);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}
