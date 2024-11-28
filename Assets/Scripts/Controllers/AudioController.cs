using UnityEngine;

namespace Match3
{
    public class AudioController
    {
        private AudioSource _musicPlayer;
        private AudioSource _soundFX;
        private AudioClips _clips;

        public AudioController(AudioSource musicPlayer,AudioSource soundFX, AudioClips clips)
        {
            _musicPlayer = musicPlayer;
            _soundFX = soundFX;
            _clips = clips;
            EntryPoint.Instance.Events.Stop += OnStop;
        }

        private void OnStop() => _musicPlayer.Stop();

        public void PlaySelect() => _soundFX.PlayOneShot(_clips.SelectSound);

        public void PlayDeselect() => _soundFX.PlayOneShot(_clips.DeselectSound);

        public void PlayDestroy()
        {
            _soundFX.pitch = Random.Range(0.9f, 1.1f);
            _soundFX.PlayOneShot(_clips.DestroySound);
            _soundFX.pitch = 1.0f;
        }

        public void PlaySpawnSound() => _soundFX.PlayOneShot(_clips.SpawnSound);
    }
}