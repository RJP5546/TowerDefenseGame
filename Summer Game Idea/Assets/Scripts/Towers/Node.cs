using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Color hoverColor;
    [SerializeField] private Color startColor;
    [SerializeField] private Vector3 towerOffset;
    private Renderer rend;

    private GameObject currentTower;

    BuildManager buildManager;

    private void Start()
    {
        buildManager = BuildManager.Instance;
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
    }

    private void OnMouseDown()
    {
        //if there is already a tower in place
        if (currentTower != null)
        {
            Debug.Log("Already a tower in place");
            return;
        }

        if (buildManager.GetTurretToBuild() == null) { return; }

        //if no tower in place
        GameObject _towerToBuild = buildManager.GetTurretToBuild();
        currentTower = (GameObject) Instantiate(_towerToBuild, transform.position + towerOffset, transform.rotation);

    }

    private void OnMouseEnter()
    {
        //if the mouse is over UI, ignore. Failsave if UI is over UI to not interact with the node
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        rend.material.color = hoverColor;
    }

    private void OnMouseExit()
    {
        rend.material.color = startColor;
    }
}
