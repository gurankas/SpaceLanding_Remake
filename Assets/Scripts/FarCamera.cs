using UnityEngine;

public class FarCamera : MonoBehaviour
{
    [SerializeField]
    private BaseCharacter _ship;

    private void OnEnable()
    {
        transform.position = new Vector3(_ship.transform.position.x, transform.position.y, transform.position.z);
    }

    private void Update()
    {
        transform.position = new Vector3(_ship.transform.position.x, transform.position.y, transform.position.z);
    }
}
