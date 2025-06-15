using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 5f; // Tốc độ di chuyển cơ bản
    [SerializeField] protected float rollSpeed = 10f; // Tốc độ né (dùng cho dash)
    [SerializeField] protected float rollDuration = 0.3f; // Thời gian né
    [SerializeField] protected float rollDistance = 2f; // Khoảng cách dash khi né
    protected Rigidbody2D rb; // Thành phần Rigidbody2D để xử lý vật lý
    protected Vector2 movement; // Vector lưu hướng di chuyển
    protected Vector2 lastMovement; // Lưu hướng di chuyển cuối cùng trước khi né
    protected SpriteRenderer spriteRenderer; // Thành phần SpriteRenderer để lật sprite
    protected Animator animator; // Thành phần Animator để điều khiển animation
    protected bool isRolling = false; // Trạng thái né

    // Khởi tạo
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component is missing on Player!");
        }
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component is missing on Player!");
        }
        if (animator == null)
        {
            Debug.LogError("Animator component is missing on Player!");
        }
    }

    // Cập nhật input di chuyển
    protected virtual void Update()
    {
        if (!isRolling)
        {
            // Lấy input từ người chơi (WASD hoặc phím mũi tên)
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            // Lưu hướng di chuyển cuối cùng
            if (movement.magnitude > 0)
            {
                lastMovement = movement.normalized;
            }

            // Chuẩn hóa vector để đảm bảo tốc độ đồng đều khi di chuyển chéo
            movement = movement.normalized;

            // Điều khiển animation
            UpdateAnimation();

            // Lật sprite theo hướng di chuyển
            FlipX();
        }

        // Kích hoạt né khi nhấn Space
        if (Input.GetKeyDown(KeyCode.Space) && !isRolling)
        {
            StartCoroutine(Roll());
        }
    }

    // Xử lý di chuyển
    protected virtual void FixedUpdate()
    {
        if (!isRolling)
        {
            // Di chuyển nhân vật bằng Rigidbody2D
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    // Cập nhật animation dựa trên hướng di chuyển
    protected virtual void UpdateAnimation()
    {
        // Đặt tất cả boolean về false trước
        animator.SetBool("RunUp", false);
        animator.SetBool("RunDown", false);
        animator.SetBool("RunX", false);
        animator.SetBool("Roll", false);

        if (movement.y > 0) // Di chuyển lên
        {
            animator.SetBool("RunUp", true);
        }
        else if (movement.y < 0) // Di chuyển xuống
        {
            animator.SetBool("RunDown", true);
        }
        else if (movement.x != 0) // Di chuyển trái/phải
        {
            animator.SetBool("RunX", true);
        }
    }

    // Phương thức lật sprite theo hướng di chuyển
    protected virtual void FlipX()
    {
        if (movement.x < 0)
        {
            spriteRenderer.flipX = false; // Hướng sang phải (mặc định)
        }
        else if (movement.x > 0)
        {
            spriteRenderer.flipX = true; // Hướng sang trái
        }
    }

    // Phương thức né
    protected virtual System.Collections.IEnumerator Roll()
    {
        isRolling = true;
        // Ngắt tất cả animation khác và kích hoạt Roll
        animator.SetBool("RunUp", false);
        animator.SetBool("RunDown", false);
        animator.SetBool("RunX", false);
        animator.SetBool("Roll", true);

        // Thực hiện dash theo hướng cuối cùng
        Vector2 dashDirection = lastMovement;
        float dashTime = 0f;
        Vector2 startPosition = rb.position;

        while (dashTime < rollDuration)
        {
            dashTime += Time.deltaTime;
            float progress = dashTime / rollDuration;
            rb.MovePosition(Vector2.Lerp(startPosition, startPosition + dashDirection * rollDistance, progress));
            yield return null;
        }

        // Tắt animation Roll và kết thúc né
        animator.SetBool("Roll", false);
        isRolling = false;
    }

    // Phương thức ảo để lấy tốc độ di chuyển (cho phép lớp con ghi đè)
    public virtual float GetMoveSpeed()
    {
        return moveSpeed;
    }

    // Phương thức ảo để thiết lập tốc độ di chuyển
    public virtual void SetMoveSpeed(float newSpeed)
    {
        moveSpeed = Mathf.Max(0, newSpeed); // Đảm bảo tốc độ không âm
    }
}