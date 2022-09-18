using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Makardwaj.Collectibles
{
    public class CollectibleFactory : ObjectPool<Collectible>
    {

    }
}

public class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private GameObject m_prefab;
    [SerializeField] private Transform m_poolParent;
    [SerializeField] private int m_initialPoolCount = 2;

    protected List<T> _pool;

    virtual protected void Awake()
    {
        GeneratePool();
    }

    private void GeneratePool()
    {
        if (_pool == null)
        {
            _pool = new List<T>();
        }
        else
        {
            _pool.Clear();
        }

        for (int i =0; i < m_initialPoolCount; i++)
        {
            var poolElement = Instantiate(m_prefab, m_poolParent);
            poolElement.SetActive(false);
            _pool.Add(poolElement.GetComponent<T>());
        }
    }

    virtual public T Instantiate(Vector3 position, Quaternion rotation)
    {
        var element = _pool.FirstOrDefault(e => !e.gameObject.activeInHierarchy);
        if(element == null)
        {
            element = Instantiate(m_prefab, m_poolParent).GetComponent<T>();
            _pool.Add(element);
        }

        element.transform.position = position;
        element.transform.rotation = rotation;
        element.gameObject.SetActive(true);

        return element;
    }
}