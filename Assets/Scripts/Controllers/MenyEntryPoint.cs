using UnityEngine;

namespace Match3
{
    public class MenyEntryPoint : MonoBehaviour
    {
        private void Awake()
        {
            var data = SaveLoad.Load();
            if (data != null)
                SessionData.LoadData(data.BestScore);
        }
    }
}