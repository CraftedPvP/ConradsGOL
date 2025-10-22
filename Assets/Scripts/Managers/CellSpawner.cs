using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace Michael
{
    public class CellSpawner : Singleton<CellSpawner>
    {
        [SerializeField] Transform cellContainer;
        // The pool holds plain GameObjects (you can swap this for any component type).
        IObjectPool<Cell> pool;
        public int PoolCount => pool.CountInactive;
        public Cell Get() => pool.Get();
        public void Return(Cell cell) => pool.Release(cell);
        public override void Awake()
        {
            base.Awake();

            int initialCellCount = GameManager.Instance.GameSettings.MapSize.x * GameManager.Instance.GameSettings.MapSize.y;
            initialCellCount += 100; // buffer

            // Create a pool with the four core callbacks.
            pool = new ObjectPool<Cell>(
                createFunc: CreateItem,
                actionOnGet: OnGet,
                actionOnRelease: OnRelease,
                actionOnDestroy: OnDestroyItem,
                collectionCheck: true,   // helps catch double-release mistakes
                defaultCapacity: initialCellCount,
                maxSize: GameManager.Instance.GameSettings.MaxCells
            );
        }

        void OnDestroyItem(Cell cell)
        {
            Destroy(cell.gameObject);
        }

        void OnRelease(Cell cell)
        {
            cell.gameObject.SetActive(false);
        }

        void OnGet(Cell cell)
        {
            cell.gameObject.SetActive(true);
        }

        Cell CreateItem()
        {
            Cell newCell = Instantiate(GameManager.Instance.GameSettings.CellPrefab, cellContainer);
            newCell.gameObject.SetActive(false);
            return newCell;
        }

    }
}
