using UnityEngine;

public class Bow : MonoBehaviour
{
    [SerializeField] private Transform firePos; // Vị trí bắn
    [SerializeField] private float damage = 10f; // Sát thương
    [SerializeField] private float projectileSpeed = 10f; // Tốc độ đạn
    [SerializeField] private GameObject arrowPrefab; // Prefab của arrow
    [SerializeField] private int poolSize = 10; // Kích thước pool
    [SerializeField] private float baseAttackSpeed = 1f; // Tốc độ đánh cơ bản (giây)
    [SerializeField] private float rotationOffset = 90f; // Offset góc quay để điều chỉnh hướng (mặc định 90 cho sprite xuống dưới)

    private Animator animator;
    private Vector3 mouseDirection; // Hướng chuột để truyền cho arrow
    private bool isShooting = false; // Kiểm soát để tránh lặp animation

    private void Start()
    {
        if (firePos == null)
        {
            Debug.LogError("FirePos is not assigned on Bow!");
        }
        if (arrowPrefab == null)
        {
            Debug.LogError("Arrow Prefab is not assigned on Bow!");
        }
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator is missing on Bow!");
        }
        // Khởi tạo pool
        Arrow.InitializePool(arrowPrefab, poolSize);
    }

    // Xoay cung theo hướng chuột
    void Update()
    {
        RotateBow();

        // Kích hoạt kéo cung và bắn khi nhấn chuột trái
        if (Input.GetMouseButtonDown(0) && !isShooting)
        {
            ShootSequence();
        }
    }

    private void RotateBow()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // Đảm bảo 2D
        mouseDirection = (mousePosition - transform.position).normalized; // Lưu hướng chuột
        float angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + rotationOffset)); // Xoay toàn bộ Bow
        firePos.localRotation = Quaternion.identity; // Đặt firePos về hướng mặc định
    }

    // Bắt đầu sequence kéo cung và bắn
    private void ShootSequence()
    {
        isShooting = true;
        animator.SetBool("IsDrawing", true); // Kích hoạt animation kéo cung
        animator.speed = 1f / baseAttackSpeed; // Điều chỉnh tốc độ animation dựa trên attack speed
    }

    // Phương thức bắn (gọi từ Animation Event)
    public void Shoot()
    {
        Debug.Log("Attempting to shoot"); // Debug để kiểm tra
        if (arrowPrefab != null && firePos != null)
        {
            Arrow arrow = Arrow.GetArrow(arrowPrefab);
            if (arrow != null)
            {
                arrow.transform.position = firePos.position;
                // Đặt rotation theo hướng chuột và thêm 90 độ cho sprite
                float angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg + 90f;
                arrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + rotationOffset));
                arrow.Setup(damage, projectileSpeed, mouseDirection);
                Debug.Log("Arrow shot successfully");
            }
            else
            {
                Debug.LogWarning("Failed to get arrow from pool");
            }
        }
        else
        {
            Debug.LogWarning("ArrowPrefab or firePos is null");
        }
        animator.SetBool("IsDrawing", false); // Tắt animation sau khi bắn
        animator.speed = 1f; // Đặt lại tốc độ animation về mặc định
        isShooting = false; // Đặt lại trạng thái để cho phép bắn lần tiếp theo
    }

    // Getter cho firePos (nếu cần)
    public Transform GetFirePos()
    {
        return firePos;
    }
}