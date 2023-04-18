using System.Collections;
using Obert.Audio.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace Sample.Basic.Scripts
{
    public sealed class PlayAndDestroy : MonoBehaviour
    {
        [SerializeField] private Button prefab;

        [SerializeField] private AudioClip sfx;

        [SerializeField] private float respawnSeconds = 3;
        
        private void Start()
        {
            InstantiateButton();
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        private void WaitAndRecreate()
        {
            StartCoroutine(WaitCoroutine());
        }

        private IEnumerator WaitCoroutine()
        {
            yield return new WaitForSeconds(respawnSeconds);
            InstantiateButton();
        }

        private void InstantiateButton()
        {
            var instance = Instantiate(prefab, transform);
            instance.onClick.AddListener(() =>
            {
                GlobalSfxPlayer.Instance.PlaySfx(sfx);
                Destroy(instance.gameObject);
                WaitAndRecreate();
            });
        }
    }
}