using UnityEngine;

public class SwitchLevel : MonoBehaviour {

    [SerializeField] int levelIndex = 1;
    private LevelManager levelManager;

    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    private void OnDrawGizmos()
    {
        Utils.DrawBoxCollider(this, Color.blue);
    }

    private void OnTriggerEnter(Collider other)
    {
        levelManager.SwitchToScene(levelIndex);
        SaveGameData.current = new SaveGameData();
    }
}
