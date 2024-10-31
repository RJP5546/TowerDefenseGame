using UnityEngine;

public class BuildManager : Singleton<BuildManager>
{
    private GameObject towerToBuild;

    public GameObject standardTowerPrefab;
    public GameObject differentTypeTowerPrefab;

    public GameObject GetTurretToBuild()
    {
        return towerToBuild;
    }

    public void SetTurretToBuild(GameObject tower)
    {
        towerToBuild = tower;
    }
}
