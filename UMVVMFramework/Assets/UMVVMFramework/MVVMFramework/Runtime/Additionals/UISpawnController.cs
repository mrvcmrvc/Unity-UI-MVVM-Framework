using System.Collections.Generic;
using UnityEngine;

public class UISpawnController : MonoBehaviour
{
    [SerializeField] private GameObject _placeholder;

    private List<GameObject> _cachedSpawnedColl = new List<GameObject>();

    private void Awake()
    {
        _placeholder.SetActive(false);
    }

    public List<T> LoadSpawnables<T>(int amount, bool activateAll = false)
    {
        if (_cachedSpawnedColl.Count > 0)
            UnloadAllSpawned();

        int addSpawnAmount = UpdateAmountWithCachedSpawnables(amount);

        IncreasePool(addSpawnAmount);

        return GetCachedSpawnablesOf<T>(amount, activateAll);
    }

    private int UpdateAmountWithCachedSpawnables(int amount)
    {
        if (_cachedSpawnedColl.Count < amount)
            amount -= _cachedSpawnedColl.Count;
        else
            amount = 0;

        return amount;
    }

    private void IncreasePool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject container = Instantiate(_placeholder, Vector3.zero, Quaternion.identity, _placeholder.transform.parent);
            container.transform.localPosition = Vector3.zero;
            container.transform.localEulerAngles = Vector3.zero;

            _cachedSpawnedColl.Add(container);

            container.name = container.name.Replace("Placeholder", "");
        }
    }

    private List<T> GetCachedSpawnablesOf<T>(int amount, bool activateAll)
    {
        List<T> resultList = new List<T>();

        for (int i = 0; i < amount; i++)
        {
            resultList.Add(_cachedSpawnedColl[i].GetComponent<T>());

            if (activateAll)
                _cachedSpawnedColl[i].SetActive(true);
            else
                _cachedSpawnedColl[i].SetActive(false);
        }

        return resultList;
    }

    private void UnloadAllSpawned()
    {
        UnloadSpawned(_cachedSpawnedColl.Count);
    }

    private void UnloadSpawned(int amount)
    {
        if (amount > _cachedSpawnedColl.Count)
            amount = _cachedSpawnedColl.Count;

        for (int i = amount; i > 0; i--)
            _cachedSpawnedColl[_cachedSpawnedColl.Count - i].SetActive(false);
    }
}
