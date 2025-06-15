using UnityEngine;

public class Arrow : MonoBehaviour
{
    private float damage;
    private float speed;
    private float lifeTime = 5f; // Thời gian sống của arrow
    private float spawnTime; // Thời điểm spawn
    private Vector3 direction; // Hướng di chuyển

    private static ObjectPool<Arrow> pool; // Pool tĩnh để quản lý arrow

    void Awake()
    {
        // Loại bỏ Rigidbody2D, chỉ giữ transform
    }

    // Khởi tạo pool khi cần
    public static void InitializePool(GameObject arrowPrefab, int initialSize)
    {
        if (pool == null)
        {
            pool = new ObjectPool<Arrow>(arrowPrefab.GetComponent<Arrow>(), initialSize);
        }
    }

    // Lấy arrow từ pool
    public static Arrow GetArrow(GameObject arrowPrefab)
    {
        if (pool == null)
        {
            InitializePool(arrowPrefab, 10); // Mặc định kích thước pool là 10 nếu chưa khởi tạo
        }
        Arrow arrow = pool.Get();
        if (arrow != null)
        {
            arrow.spawnTime = Time.time;
        }
        return arrow;
    }

    // Trả arrow về pool
    public static void ReturnArrow(Arrow arrow)
    {
        if (pool != null && arrow != null)
        {
            pool.Return(arrow);
        }
    }

    // Cài đặt thông số từ Bow
    public void Setup(float damage, float speed, Vector3 direction)
    {
        this.damage = damage;
        this.speed = speed;
        this.direction = direction.normalized;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f); // Xoay 90 độ
    }

    void Update()
    {
        // Di chuyển thủ công
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        // Hủy arrow nếu hết thời gian sống
        if (Time.time - spawnTime > lifeTime)
        {
            ReturnArrow(this);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Chỉ xử lý va chạm với các object không phải Player
        if (other.CompareTag("Player"))
        {
            return; // Bỏ qua va chạm với Player
        }
        // Logic va chạm (VD: gây sát thương cho enemy)
        ReturnArrow(this); // Trả về pool khi va chạm với object khác
    }

    // Phương thức truy cập damage
    public float GetDamage()
    {
        return damage;
    }
}