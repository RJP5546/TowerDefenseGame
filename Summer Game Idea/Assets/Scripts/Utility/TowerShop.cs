using UnityEngine;

public class TowerShop : MonoBehaviour
{
    BuildManager buildManager;

    private void Start()
    {
        buildManager = BuildManager.Instance;
    }
    public void PurchaseStandardTower()
    {
        Debug.Log("Standard Tower Selected");
        buildManager.SetTurretToBuild(buildManager.standardTowerPrefab);
    }

    public void PurchaseDifferentTowerType()
    {
        //just copy and paste these methods for each kind of tower
        Debug.Log("Different Type Of Tower Selected");
        buildManager.SetTurretToBuild(buildManager.differentTypeTowerPrefab);
    }
}
