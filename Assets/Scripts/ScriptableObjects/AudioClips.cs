using UnityEngine;

namespace Match3
{
    [CreateAssetMenu(fileName ="new AudioClips",menuName = "Match3/AudioClips", order = 53)]
    public class AudioClips : ScriptableObject
    {
        [SerializeField] private AudioClip _selectSound;
        [SerializeField] private AudioClip _deselectSound;

        [SerializeField] private AudioClip _destroySound;

        [SerializeField] private AudioClip _spawnSound;


        public AudioClip SelectSound => _selectSound;
        public AudioClip DeselectSound => _deselectSound;
        public AudioClip DestroySound => _destroySound;
        public AudioClip SpawnSound => _spawnSound;

    }
}
