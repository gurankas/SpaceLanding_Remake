using UnityEngine;

public class CameraTransition : MonoBehaviour
{
    [SerializeField]
    private Camera _farCamera;

    [SerializeField]
    private Camera _nearCamera;

    private void OnEnable()
    {
        _nearCamera.enabled = false;
        _farCamera.enabled = true;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<BaseCharacter>() != null)
        {
            _nearCamera.enabled = true;
            _farCamera.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<BaseCharacter>() != null)
        {
            _nearCamera.enabled = false;
            _farCamera.enabled = true;
        }
    }
}
