using UnityEngine;

namespace Sample.Basic.Scripts
{
    public class DummyPlayerController : MonoBehaviour
    {
        [SerializeField] private string walkSpeedParameterName = "WalkSpeed";

        [SerializeField] private float rotationSpeedMultiplier = 30;

        [SerializeField] private Animator animator;

        private int _walkSpeedParameter;

        private void Awake()
        {
            _walkSpeedParameter = Animator.StringToHash(walkSpeedParameterName);
        }


        // Update is called once per frame
        private void Update()
        {
            var horizontalAxis = Input.GetAxis("Horizontal");
            var verticalAxis = Input.GetAxis("Vertical");

            var transform1 = transform;
            var deltaTime = Time.deltaTime;

            transform1.Rotate(transform1.up, rotationSpeedMultiplier * deltaTime * horizontalAxis);

            animator.SetFloat(_walkSpeedParameter, verticalAxis * (Input.GetKey(KeyCode.LeftShift) ? 2 : 1));
            if(Input.GetKeyDown(KeyCode.Space))
                animator.SetTrigger("Die");
        }
    }
}