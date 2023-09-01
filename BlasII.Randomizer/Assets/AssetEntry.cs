using System;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BlasII.Randomizer.Assets
{
    public class AssetEntry<T> : AbstractAssetEntry
    {
        private readonly AsyncOperationHandle<T> handle;

        private readonly Action<T> callback;

        public AssetEntry(AsyncOperationHandle<T> handle, Action<T> callback)
        {
            this.handle = handle;
            this.callback = callback;
        }

        public override bool IsLoaded
        {
            get
            {
                if (handle.Status == AsyncOperationStatus.None)
                {
                    return false;
                }
                else
                {
                    if (handle.Status == AsyncOperationStatus.Succeeded)
                        callback(handle.Result);
                    handle.Release();
                    return true;
                }
            }
        }
    }

    public abstract class AbstractAssetEntry
    {
        public abstract bool IsLoaded { get; }
    }
}
