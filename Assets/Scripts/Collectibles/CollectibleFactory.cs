using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Makardwaj.Collectibles
{
    public class CollectibleFactory : MonoBehaviour
    {
        [SerializeField] private List<CollectibleData> m_collectibles;
        [SerializeField] private int m_initialCollectibleCount = 2;

        [SerializeField] private Transform m_collectiblesParent;

        private List<Collectible> _collectibles;


        private List<CollectibleType> _collectibleNames;
        private int _collectibleCount;

        private void Awake()
        {
            _collectibleNames = System.Enum.GetValues(typeof(CollectibleType)).Cast<CollectibleType>().ToList();
            _collectibleCount = _collectibleNames.Count;

            InitializePool();
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


        public Collectible Instantiate(Vector3 position, Quaternion rotation)
        {
            int collectibleIndex = Random.Range(0, _collectibleCount);
            var collectibleType = _collectibleNames[collectibleIndex];

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
        
    }
}