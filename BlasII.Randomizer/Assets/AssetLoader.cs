using System;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BlasII.Randomizer.Assets
{
    public class AssetLoader
    {
        private readonly List<AbstractAssetEntry> loaders = new();

        public void UpdateLoaders()
        {
            for (int i = 0; i < loaders.Count; i++)
            {
                if (loaders[i].IsLoaded)
                {
                    loaders.RemoveAt(i);
                    i--;
                }
            }
        }

        public void AddLoader<T>(AsyncOperationHandle<T> handle, Action<T> callback)
        {
            loaders.Add(new AssetEntry<T>(handle, callback));
        }
    }
}
