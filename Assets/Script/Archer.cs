using UnityEngine;

public class Archer : Player
{
    [SerializeField] private float speedMultiplier = 1.2f; // Cung thủ di chuyển nhanh hơn 20%
    [SerializeField] private Bow bow; // Tham chiếu đến Bow

    protected override void Awake()
    {
        base.Awake();
        // Tăng tốc độ di chuyển cho Archer
        moveSpeed *= speedMultiplier;
        if (bow == null)
        {
            Debug.LogError("Bow is not assigned on Archer!");
        }
    }

    protected override void Update()
    {
        base.Update();
        // Logic di chuyển và né đã được xử lý trong Player
    }
}