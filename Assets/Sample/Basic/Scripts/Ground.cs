using UnityEngine;

namespace Sample.Basic.Scripts
{
    public class Ground : MonoBehaviour
    {
        [SerializeField]
        private GroundType groundType;

        public GroundType Type => groundType;
    }
}