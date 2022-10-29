using Assets.Scripts.Projectiles.PoisionDrop;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Makardwaj.Projectiles
{
    public class PoisonPool : MonoBehaviour
    {
        [SerializeField] private Poison m_poisonPrefab;
        [SerializeField] private PoisonSpillVfx m_poisonSpillPrefab;
        [SerializeField] private int m_initialPoolCount = 10;
        [SerializeField] private Transform m_poisonParent;
        [SerializeField] private Transform m_poisonSpillParent;

        private List<Poison> _poisonPool;
        private List<PoisonSpillVfx> _poisonSpillPool;
        private Poison _workspacePoison;
        private PoisonSpillVfx _workspacePoisonSpill;

        private void Awake()
        {
            InitializePool();
        }

        private void InitializePool()
        {
            _poisonPool = new List<Poison>();
            _poisonSpillPool = new List<PoisonSpillVfx>();

            for (int i = 0; i < m_initialPoolCount; i++)
            {
                _workspacePoison = Instantiate(m_poisonPrefab, m_poisonParent);
                _workspacePoison.gameObject.SetActive(false);

                _poisonPool.Add(_workspacePoison);

                _workspacePoisonSpill = Instantiate(m_poisonSpillPrefab, m_poisonSpillParent);
                _workspacePoisonSpill.gameObject.SetActive(false);

                _poisonSpillPool.Add(_workspacePoisonSpill);
            }
        }

        public Poison InstantiatePoison()
        {
            _workspacePoison = _poisonPool.FirstOrDefault(p => !p.gameObject.activeInHierarchy);

            if (!_workspacePoison)
            {
                _workspacePoison = Instantiate(m_poisonPrefab, m_poisonParent);
                _workspacePoison.gameObject.SetActive(false);
                _poisonPool.Add(_workspacePoison);
            }

            _workspacePoison.gameObject.SetActive(true);

            return _workspacePoison;
        }

        public Poison ShootPoison(Vector2 position, float speed, int dir)
        {
            _workspacePoison = InstantiatePoison();
            _workspacePoison.Shoot(position, speed, dir);
            return _workspacePoison;
        }

        public Poison ShootPoison(Vector2 position, float speed, float angle)
        {
            _workspacePoison = InstantiatePoison();
            _workspacePoison.Shoot(position, angle, speed);
            return _workspacePoison;
        }

        public Poison DropPoison(Vector2 position, float dropSpeed)
        {
            _workspacePoison = InstantiatePoison();
            _workspacePoison.Drop(position, dropSpeed);

            return _workspacePoison;
        }

        public PoisonSpillVfx InstantiatePoisonSpill(Vector2 position, float rotation)
        {
            _workspacePoisonSpill = _poisonSpillPool.FirstOrDefault(p => !p.gameObject.activeInHierarchy);

            if (!_workspacePoisonSpill)
            {
                _workspacePoisonSpill = Instantiate(m_poisonSpillPrefab, m_poisonSpillParent);
                _workspacePoisonSpill.gameObject.SetActive(false);
                _poisonSpillPool.Add(_workspacePoisonSpill);
            }

            _workspacePoisonSpill.Activate(position);
            _workspacePoisonSpill.gameObject.SetActive(true);
            _workspacePoisonSpill.transform.rotation = Quaternion.Euler(Vector3.forward * rotation);

            return _workspacePoisonSpill;
        }

        public void RemoveAllPoison()
        {
            for(int i = 0; i < _poisonPool.Count; i++)
            {
                _poisonPool[i].gameObject.SetActive(false);
                _poisonSpillPool[i].Deactivate();
            }
        }
    }
}