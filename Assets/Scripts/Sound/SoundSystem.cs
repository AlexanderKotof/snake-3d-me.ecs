using Game.Events;
using ME.ECS;
using UnityEngine;

namespace Game.Sound
{
    public class SoundSystem : MonoBehaviour
    {
        public AudioSource source;

        public GameOverGlobalEvent gameOverEvent;
        public GlobalEvent gameStartedEvent;
        public AddPointsGlobalEvent addPointsEvent;

        public AudioClip gameOveClip;
        public AudioClip gameStartedClip;
        public AudioClip collectAppleClip;

        private void Start()
        {
            gameStartedEvent.Subscribe(PlayGameStartedSound);
            gameOverEvent.Subscribe(PlayGameOverSound);
            addPointsEvent.Subscribe(PlayAppleCollectedSound);
        }

        private void PlayAppleCollectedSound(in Entity entity)
        {
            source.PlayOneShot(collectAppleClip);
        }

        private void PlayGameStartedSound(in Entity entity)
        {
            source.PlayOneShot(gameStartedClip);
        }

        private void PlayGameOverSound(in Entity entity)
        {
            source.PlayOneShot(gameOveClip);
        }


        private void OnDestroy()
        {
            gameStartedEvent.Unsubscribe(PlayGameStartedSound);
            gameOverEvent.Unsubscribe(PlayGameOverSound);
            addPointsEvent.Unsubscribe(PlayAppleCollectedSound);
        }
    }
}