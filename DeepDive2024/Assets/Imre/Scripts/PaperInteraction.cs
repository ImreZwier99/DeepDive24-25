using UnityEngine;

public class PaperInteraction : MonoBehaviour
{
    [Header("Raycast Variables")]
    [SerializeField] private Transform _camera;
    [SerializeField] private float rayDistance = 10f;
    [SerializeField] private LayerMask interactableLayerMask;
    [SerializeField] private GameObject paperPrefab;
    [SerializeField] private float paperDistance;
    [SerializeField] private float paperRotationX;
    [SerializeField] private float paperXOffset;
    [SerializeField] private float paperYoffset;

    public GameObject heldPaper;
    private PaperStack paperStack;
    private bool canGrab = false;

    private void Start()
    {
        paperStack = FindObjectOfType<PaperStack>();
    }

    void Update()
    {
        CheckRaycast();
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (heldPaper == null && canGrab && paperStack != null && paperStack.counter > 0)
            {
                GrabPaper();
            }
        }

        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    if (heldPaper != null)
        //    {
        //        DeletePaper();
        //    }
        //}
    }

    private void CheckRaycast()
    {
        Ray ray = new Ray(_camera.position, _camera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, interactableLayerMask))
        {
            canGrab = hit.collider.CompareTag("PaperStack");
        }
        else
        {
            canGrab = false;
        }
    }

    private void GrabPaper()
    {
        if (paperPrefab == null) return;

        Vector3 paperPosition = _camera.position + _camera.forward * paperDistance;
        Quaternion paperRotation = _camera.rotation * Quaternion.Euler(paperRotationX, 0, 0);
        paperPosition.x += paperXOffset;
        paperPosition.y -= paperYoffset;

        heldPaper = Instantiate(paperPrefab, paperPosition, paperRotation);
        heldPaper.transform.parent = _camera;

        if (paperStack != null)
        {
            paperStack.DecreaseCounter();
        }
    }

    public void DeletePaper()
    {
        if (heldPaper != null)
        {
            Destroy(heldPaper);
            heldPaper = null;
        }
    }
}
