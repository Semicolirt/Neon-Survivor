using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Shooting))]
[RequireComponent(typeof(HealthComponent))]
public class PlayerController : MonoBehaviour, IObserver<HealthData>
{
    public PlayerStatsSO playerStats;
    private Vector2 movementInput;
    private Rigidbody2D rb;
    private Transform spriteTransform;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private HealthComponent healthComponent;
    private float magenetRadius = 2f; // Bán kính hút ExpOrb
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteTransform = transform;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Khởi tạo HealthComponent và đăng ký observer
        healthComponent = GetComponent<HealthComponent>();
        healthComponent.AddObserver(this);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    void Update()
    {
        MagnetExpOrbs();
    }

    void FixedUpdate()
    {
        // Di chuyển player bằng vật lý (velocity)
        rb.linearVelocity = movementInput * playerStats.moveSpeed;

        PlayerUtility.FlipPlayerTowardMouse(spriteTransform);

        // Cập nhật thông số cho Animator
        animator.SetFloat("Speed", rb.linearVelocity.magnitude);
    }

    public void OnNotify(HealthData data)
    {
        //UIManager.Instance.UpdateHealthBar(data.CurrentHealth, data.MaxHealth);
        if (data.IsDead)
        {
            animator.SetTrigger("Dead");
            PlayerUtility.HandlePlayerDeath(animator, rb);
            this.enabled = false; // Vô hiệu hóa controller
        }
    }

    private void MagnetExpOrbs()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, magenetRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("ExpOrb"))
            {
                ExpOrb expOrb = hitCollider.GetComponent<ExpOrb>();
                if (expOrb != null)
                {
                    expOrb.StartMagnet(transform); // Bắt đầu hút ExpOrb về phía player
                }
            }
        }
    }

    void ResetStats()
    {
        playerStats.maxHealth = 100f;

    }

    public void OnSelfHit()
    {
        animator.SetTrigger("Hit");
        spriteRenderer.color = Color.red;
        spriteRenderer.color = Color.white; // Reset màu sắc sau khi bị hit
    }

}