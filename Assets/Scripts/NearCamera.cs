using UnityEngine;

public class NearCamera : MonoBehaviour
{
    [SerializeField]
    private BaseCharacter _ship;

    private void OnEnable()
    {
        transform.position = new Vector3(_ship.transform.position.x, _ship.transform.position.y, transform.position.z);
    }

    private void Update()
    {
        transform.position = new Vector3(_ship.transform.position.x, _ship.transform.position.y, transform.position.z);
    }
}
