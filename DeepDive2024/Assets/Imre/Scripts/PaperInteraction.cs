using UnityEngine;
using UnityEngine.UI;

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

    public AudioSource paperSFX;
    public GameObject heldPaper;
    public Image kruisje1;
    public Image kruisje2;
    private PaperStack paperStack;
    private bool canGrab = false;
    private bool canInteractWithBinder = false;
    private int fout = 0;
    public static bool isHoldingPaper = false;

    private void Start()
    {
        paperStack = FindObjectOfType<PaperStack>();
    }

    void Update()
    {
        if (heldPaper != null) isHoldingPaper = true;
        else isHoldingPaper = false;

        CheckRaycast();
        HandleInput();

        if (fout > 2)
        {
            WateringSystem.fired = true;
            print("je hebt te veel fouten gemaakt.");
        }
        if (fout == 0)
        {
            kruisje1.color = Color.black;
            kruisje2.color = Color.black;
        }
        if (fout == 1)
        {
            kruisje1.color = Color.red;
        }
        if (fout == 2)
        {
            kruisje2.color = Color.red;
        }
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0) && !WateringSystem.fired)
        {
            if (heldPaper == null && canGrab && paperStack != null && PaperStack.counter > 0)
            {
                GrabPaper();
            }
            else if (heldPaper != null && canInteractWithBinder)
            {
                InteractWithBinder();
            }
        }
    }

    private void CheckRaycast()
    {
        Ray ray = new Ray(_camera.position, _camera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, interactableLayerMask))
        {
            canGrab = hit.collider.CompareTag("PaperStack");

            if (canInteractWithBinder = hit.collider.CompareTag("Binder") && heldPaper != null && Input.GetKeyDown(KeyCode.Mouse0) && !RandomizedPapers.departementaalOnversleuteld)
            {
                Destroy(heldPaper);
                fout++;
                print(fout);
            }
            else if (canInteractWithBinder = hit.collider.CompareTag("Binder")&& Input.GetKeyDown(KeyCode.Mouse0) && RandomizedPapers.departementaalOnversleuteld)
            {
                Destroy(heldPaper);
                print(fout);
            }
            
            if (canInteractWithBinder = hit.collider.CompareTag("Binder2") && heldPaper != null && Input.GetKeyDown(KeyCode.Mouse0) && !RandomizedPapers.departementaalVersleuteld)
            {
                Destroy(heldPaper);
                fout++;
                print(fout);
            }
            else if (canInteractWithBinder = hit.collider.CompareTag("Binder2") && Input.GetKeyDown(KeyCode.Mouse0) && RandomizedPapers.departementaalVersleuteld)
            {
                Destroy(heldPaper);
                print(fout);
            }

            if (canInteractWithBinder = hit.collider.CompareTag("Binder3") && heldPaper != null && Input.GetKeyDown(KeyCode.Mouse0) && !RandomizedPapers.openbaar)
            {
                Destroy(heldPaper);
                fout++;
                print(fout);
            }
            else if (canInteractWithBinder = hit.collider.CompareTag("Binder3") && Input.GetKeyDown(KeyCode.Mouse0) && RandomizedPapers.openbaar)
            {
                Destroy(heldPaper);
                print(fout);
            }

            if (canInteractWithBinder = hit.collider.CompareTag("Binder4") && heldPaper != null && Input.GetKeyDown(KeyCode.Mouse0) && !RandomizedPapers.vertrouwelijk)
            {
                Destroy(heldPaper);
                fout++;
                print(fout);
            }
            else if (canInteractWithBinder = hit.collider.CompareTag("Binder4") && Input.GetKeyDown(KeyCode.Mouse0) && RandomizedPapers.vertrouwelijk)
            {
                Destroy(heldPaper);
                print(fout);
            }

            if (canInteractWithBinder = hit.collider.CompareTag("Binder5") && heldPaper != null && Input.GetKeyDown(KeyCode.Mouse0) && !RandomizedPapers.intern)
            {
                Destroy(heldPaper);
                fout++;
                print(fout);
            }
            else if (canInteractWithBinder = hit.collider.CompareTag("Binder5") && Input.GetKeyDown(KeyCode.Mouse0) && RandomizedPapers.intern)
            {
                Destroy(heldPaper);
                print(fout);
            }
        }
        else
        {
            canGrab = false;
            canInteractWithBinder = false;
        }
    }

    private void GrabPaper()
    {
        //paperSFX.Play();
        RandomizedPapers.OnNewSpawn();
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

    private void InteractWithBinder()
    {
        DeletePaper();
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
