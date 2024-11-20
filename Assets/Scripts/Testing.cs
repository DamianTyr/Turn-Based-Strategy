using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private Colonist colonist;
    
    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            colonist.Test(new GridPosition(1,1), OnActionComplete);
        }
    }

    private void OnActionComplete()
    {
        Debug.Log("On Action Complete");
    }
}
