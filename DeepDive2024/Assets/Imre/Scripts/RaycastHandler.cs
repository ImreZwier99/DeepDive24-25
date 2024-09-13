using UnityEngine;

public class RaycastHandler : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private LayerMask interactableLayerMask;
    [SerializeField] private float rayDistance = 10f;

    public bool isLookingAtBin { get; private set; }
    public bool isLookingAtPlant { get; private set; }

    void Update()
    {
        SendRaycast();
    }

    private void SendRaycast()
    {
        Ray ray = new Ray(_camera.position, _camera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, interactableLayerMask))
        {
            isLookingAtBin = hit.collider.CompareTag("Bin") || hit.collider.CompareTag("Boss");
            //isLookingAtPlant = hit.collider.CompareTag("Plant");
        }
        else
        {
            isLookingAtBin = false;
            isLookingAtPlant = false;
        }
    }
}
