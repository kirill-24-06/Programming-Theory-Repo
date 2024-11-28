using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveLoad
{
    private static BinaryFormatter _formatter = new BinaryFormatter();

    public static void Save()
    {
        var saveStream = new FileStream(GlobalConstants.BinarySavePath, FileMode.Create);

        var data = new SaveData()
        {
            BestScore = SessionData.BestScore
        };

        _formatter.Serialize(saveStream, data);

        saveStream.Close();
    }

    public static SaveData Load()
    {
        if (File.Exists(GlobalConstants.BinarySavePath))
        {
            var loadSteram = new FileStream(GlobalConstants.BinarySavePath, FileMode.Open);

            var data = _formatter.Deserialize(loadSteram) as SaveData;

            loadSteram.Close();

            return data;
        }

        else
            return null;
    }
}
