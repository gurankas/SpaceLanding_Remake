using UnityEngine;

public class NearCamera : MonoBehaviour
{
    [SerializeField]
    private BaseCharacter _ship;

    private void OnEnable()
    {
        if (_ship != null)
        {
            transform.position = new Vector3(_ship.transform.position.x, _ship.transform.position.y, transform.position.z);
        }
    }

    private void Update()
    {
        if (_ship != null)
        {
            transform.position = new Vector3(_ship.transform.position.x, _ship.transform.position.y, transform.position.z);
        }
    }
}
