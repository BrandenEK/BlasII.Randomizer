using Il2CppSystem.Threading.Tasks;
using Il2CppTGK.Persistence;
using System;

namespace BlasII.Randomizer.Persistence
{
    public class PersistenceHandler : PersistentInterface
    {
        // Not working ...
        public PersistenceHandler(IntPtr pointer) : base(pointer) { }

        public override string GetPersistentUID()
        {
            return "ID_RANDOMIZER";
        }

        public override int GetPersistentID()
        {
            return 0xFF77FF;
        }

        public override PersistentData BuildCurrentPersistentState()
        {
            Main.Randomizer.Log("Save data");
            return base.BuildCurrentPersistentState();
        }

        public override int BuildCurrentPersistentState(PersistentData persistentData)
        {
            return base.BuildCurrentPersistentState(persistentData);
        }

        public override Task SetCurrentPersistentState(PersistentData data)
        {
            Main.Randomizer.Log("Load data");
            return base.SetCurrentPersistentState(data);
        }

        public override void UpdatePersistentData(int version, ref PersistentData data)
        {
            base.UpdatePersistentData(version, ref data);
        }

        public override void ResetPersistence()
        {
            Main.Randomizer.Log("Reset data");
            base.ResetPersistence();
        }

        public override int GetPersistenceOrder()
        {
            return 0;
        }

        public override int GetCurrentVersion()
        {
            return 0;
        }
    }
}
