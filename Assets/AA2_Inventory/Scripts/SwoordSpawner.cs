using UnityEngine;

public class SwoordSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _sword;

    private void Start()
    {
        Instantiate(_sword, new Vector3(0.5f, 1.5f, 0), Quaternion.identity);
    }
}
