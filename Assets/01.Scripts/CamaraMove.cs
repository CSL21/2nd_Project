using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] Transform target;

    Vector3 velocity;

    Vector3 targetPos;

    [SerializeField] float minX = -20f;
    [SerializeField] float maxX = 20f;
    [SerializeField] float minY = -20f;
    [SerializeField] float maxY = 20f;
    [SerializeField] float offsetX = 7f;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        velocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        targetPos = new Vector3(target.position.x + offsetX, target.position.y, transform.position.z);

        targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX);
        targetPos.y = Mathf.Clamp(targetPos.y, minY, maxY);

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, 0.2f);
    }
}
