using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    Image img;

    [SerializeField] Transform target;
    [SerializeField] Vector3 offset = Vector3.up;

    float gauge;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        img = GetComponent<Image>();
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            transform.position = target.position + offset;
        }

        if (PlayerStatus.instance != null && img != null)
        {
            img.fillAmount = PlayerStatus.instance.GetHPPercent();
        }
    }

    public void SetGauge(float gauge)
    {
        img.fillAmount = gauge;
    }

    public void SetTarget(Transform t)
    {
        target = t;
    }


}
