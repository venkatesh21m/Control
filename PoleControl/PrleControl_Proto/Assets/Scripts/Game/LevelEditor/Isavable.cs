namespace Rudrac.Control
{
    public interface ISavable
    {
        public void PopulateSaveData(LevelData levelData);
        public void LoadSaveData(LevelData levelData);
    }
}
