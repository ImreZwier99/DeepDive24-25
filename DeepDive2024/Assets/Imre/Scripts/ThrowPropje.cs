using UnityEngine;

public class ThrowPropje : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private GameObject Papierpropje;
    [SerializeField] private GameObject propThrow;

    private PaperInteraction paperInteraction;
    private RaycastHandler raycastHandler;

    void Start()
    {
        paperInteraction = FindObjectOfType<PaperInteraction>();
        raycastHandler = FindObjectOfType<RaycastHandler>();
    }

    void Update()
    {
        if (raycastHandler.isLookingAtBin && Input.GetMouseButtonDown(0) && paperInteraction.heldPaper != null)
        {
            Instantiate(Papierpropje, propThrow.transform.position, propThrow.transform.rotation);
            paperInteraction.DeletePaper();
        }
    }
}
