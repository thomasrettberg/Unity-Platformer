using UnityEngine;

public class SwitchLevel : MonoBehaviour {

    [SerializeField] int levelIndex = 1;

    private void OnDrawGizmos()
    {
        Utils.DrawBoxCollider(this, Color.blue);
    }

    private void OnTriggerEnter(Collider other)
    {
        LevelManager.SwitchToScene(levelIndex);
    }
}
