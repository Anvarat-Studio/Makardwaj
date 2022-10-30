using System.Collections.Generic;
using System.Linq;
using Makardwaj.Managers;
using UnityEngine;

namespace Makardwaj.Collectibles
{
    public class CollectibleFactory : MonoBehaviour
    {
        [SerializeField] private List<CollectibleData> m_collectibles;
        [SerializeField] private int m_initialCollectibleCount = 2;

        [SerializeField] private Transform m_collectiblesParent;

        private List<Collectible> _collectibles;

        private void OnEnable()
        {
            EventHandler.LevelChangeStarted += DisableAllCollectibles;
            EventHandler.heavenActivated += DisableAllCollectibles;
        }

        private void Awake()
        {
            InitializePool();
        }

        private void OnDisable()
        {
            EventHandler.LevelChangeStarted -= DisableAllCollectibles;
            EventHandler.heavenActivated -= DisableAllCollectibles;
        }

        public void InitializePool()
        {
            _collectibles = new List<Collectible>();

            for(int i = 0; i < m_collectibles.Count; i++)
            {
                for(int j = 0; j < m_initialCollectibleCount; j++)
                {
                    var collectibleData = m_collectibles[i] ;
                    SpawnCollectible(collectibleData);
                }
            }
        }

        private Collectible SpawnCollectible(CollectibleData collectibleData)
        {
            var collectible = Instantiate(collectibleData.collectiblePrefab, m_collectiblesParent);
            collectible.Initialize(collectibleData.points, collectibleData.type);
            collectible.gameObject.SetActive(false);
            _collectibles.Add(collectible);

            return collectible;
        }


        public Collectible Instantiate(Vector3 position, Quaternion rotation, CollectibleColor colorNotAllowed)
        {
            var collectiblesCollection = m_collectibles.Where(c => c.color != colorNotAllowed).ToList();
            var collectibleType = collectiblesCollection[Random.Range(0, collectiblesCollection.Count)].type;

            var collectible = _collectibles.FirstOrDefault(c => c.Type == collectibleType && !c.gameObject.activeInHierarchy);

            if(collectible == null)
            {
                var collectibleData = m_collectibles.FirstOrDefault(cd => cd.type == collectibleType);
                collectible = SpawnCollectible(collectibleData);
            }

            collectible.transform.position = position;
            collectible.transform.rotation = rotation;
            collectible.gameObject.SetActive(true);

            return collectible;
        }

        private void DisableAllCollectibles()
        {
            for(int i = 0; i < _collectibles.Count; i++)
            {
                _collectibles[i].gameObject.SetActive(false);
            }
        }
    }
}