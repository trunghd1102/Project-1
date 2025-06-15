using UnityEngine;

public class Archer : Player
{
    [SerializeField] private float speedMultiplier = 1.2f; // Cung thủ di chuyển nhanh hơn 20%

    protected override void Awake()
    {
        base.Awake();
        // Tăng tốc độ di chuyển cho Archer
        moveSpeed *= speedMultiplier;
    }

    // Ghi đè để thêm hành vi riêng cho Archer nếu cần
    protected override void Update()
    {
        base.Update();
        // Có thể thêm logic riêng cho Archer, ví dụ: né tránh nhanh khi nhấn phím đặc biệt
    }

    // Ghi đè để thêm hành vi riêng cho Archer nếu cần
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        // Có thể thêm logic riêng cho di chuyển của Archer
    }

    // Ghi đè phương thức lấy tốc độ để phản ánh đặc điểm của Archer
    public override float GetMoveSpeed()
    {
        return base.GetMoveSpeed() * speedMultiplier;
    }
}