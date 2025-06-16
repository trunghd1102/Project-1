using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float smoothSpeed = 0.125f; // Tốc độ di chuyển mượt của camera
    [SerializeField] private Vector3 offset; // Khoảng cách giữa camera và nhân vật
    private Transform target; // Đối tượng để theo dõi
    private Vector3 velocity = Vector3.zero; // Vận tốc để SmoothDamp

    void Start()
    {
        // Tìm tất cả các object có script kế thừa từ Player
        Player[] players = FindObjectsByType<Player>(FindObjectsSortMode.None);
        if (players.Length > 0)
        {
            target = players[0].transform; // Lấy nhân vật đầu tiên làm mục tiêu
        }
        else
        {
            Debug.LogWarning("No Player object found in scene!");
        }

        // Đặt offset mặc định (có thể điều chỉnh trong Inspector)
        if (offset == Vector3.zero)
        {
            offset = new Vector3(0f, 0f, -10f); // Camera đứng sau nhân vật theo Z
        }
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            // Tính vị trí mục tiêu mới
            Vector3 desiredPosition = target.position + offset;
            // Di chuyển camera mượt mà với SmoothDamp
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
            transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
        }
    }
}