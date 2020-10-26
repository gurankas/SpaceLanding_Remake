using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField]
    private float _parallexEffect = 0;

    private float length, startPos;
    private Camera _cam;

    private void OnEnable()
    {
        _cam = Camera.main;
    }

    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float temp = (_cam.transform.position.x * (1 - _parallexEffect));
        float dist = (_cam.transform.position.x * _parallexEffect);

        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

        if (temp > startPos + length)
        {
            startPos += length;
        }
        else if (temp < startPos - length)
        {
            startPos -= length;
        }
    }
}
