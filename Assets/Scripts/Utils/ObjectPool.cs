using System.Collections.Generic;
using UnityEngine;
using System;

namespace FridayAfternoons
{
    [Serializable]
    public class ObjectPool<T> where T:Component
    {
        public T prefab;
        public int amount;
        public Transform container;
        
        public int TotalInstanceCount { get; private set; }
        public int ActiveInstanceCount => TotalInstanceCount - availableObjects.Count;

        private List<T> availableObjects;
        private List<T> activeObjects;
        private int nextId;

        public List<T> ActiveObjects => activeObjects;

        public event Action<T> OnInstantiate;
        
        public void Init(bool trackActiveObjects)
        {
            availableObjects = new List<T>();
            activeObjects = trackActiveObjects ? new List<T>() : null;
            amount = Mathf.Max(1, amount);
            InstantiatePrefab(amount);
        }

        private int GetId()
        {
            int id = nextId;
            nextId++;
            return id;
        }

        private void InstantiatePrefab(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                T instance = UnityEngine.Object.Instantiate(prefab, container);
                instance.gameObject.SetActive(false);
                instance.gameObject.name = prefab.name + " " + GetId();
                availableObjects.Add(instance);
                OnInstantiate?.Invoke(instance);
            }
            TotalInstanceCount += amount;
        }

        public T GetAvailableInstance()
        {
            if(availableObjects.Count == 0)
            {
                InstantiatePrefab(amount);
            }

            T instance = availableObjects[0];
            availableObjects.RemoveAt(0);
            if(activeObjects != null) activeObjects.Add(instance);
            return instance;
        }

        public void ReleaseInstance(T instance, bool removeFromActive = true)
        {
            if (activeObjects != null && removeFromActive) activeObjects.Remove(instance);
            instance.gameObject.SetActive(false);
            availableObjects.Add(instance);
        }

        public void ReleaseAll()
        {
            foreach(T instance in activeObjects)
            {
                ReleaseInstance(instance, false);
            }
            activeObjects.Clear();
        }
    }
}