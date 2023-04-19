using System;
using Obert.Audio.Runtime;
using UnityEngine;
using UnityEngine.Assertions;

namespace Sample.Basic.Scripts
{
    public sealed class GroundSfxChanger : MonoBehaviour
    {
        [SerializeField] private ContextualSfxPlayerFacade contextualSfxPlayerFacade;
        private string _currentTerrain;

        private void Awake()
        {
            Assert.IsNotNull(contextualSfxPlayerFacade, "contextualSfxPlayer != null");
        }

        private void OnCollisionEnter(Collision other)
        {
            var otherGameObject = other.gameObject.GetComponent<GroundSfxTagProvider>();
            if (otherGameObject == null || string.IsNullOrWhiteSpace(otherGameObject.SfxTag)) return;

            if (!string.IsNullOrWhiteSpace(_currentTerrain))
                contextualSfxPlayerFacade.RemoveRequiredTag(_currentTerrain);
            
            contextualSfxPlayerFacade.AddRequiredTag(otherGameObject.SfxTag);
            _currentTerrain = otherGameObject.SfxTag;
        }
    }
}