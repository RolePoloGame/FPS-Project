using RolePoloGame.Core.Extensions;
using UnityEngine;

namespace Assets.FPSProject.Scripts.Core.Reactors
{
    public class AnimatorReactor : ReactorBase
    {
        [SerializeField]
        private Animator animator;

        [SerializeField]
        private string parameterName;
        private int m_ParameterHashed;
        [SerializeField]
        private ParameterType parameterType;

        [SerializeField, DrawIf(nameof(parameterType), ParameterType.Integer)]
        private int intParam;
        [SerializeField, DrawIf(nameof(parameterType), ParameterType.Float)]
        private float floatParam;
        [SerializeField, DrawIf(nameof(parameterType), ParameterType.Bool)]
        private bool boolParam;

        private void Start()
        {
            m_ParameterHashed = Animator.StringToHash(parameterName);
        }

        public override bool PlayWithResult()
        {
            if (animator == null) return false;
            animator.enabled = true;
            switch (parameterType)
            {
                case ParameterType.Integer:
                    animator.SetInteger(m_ParameterHashed, intParam);
                    break;
                case ParameterType.Float:
                    animator.SetFloat(m_ParameterHashed, floatParam);
                    break;
                case ParameterType.Bool:
                    animator.SetBool(m_ParameterHashed, boolParam);
                    break;
                case ParameterType.Trigger:
                    animator.ResetTrigger(m_ParameterHashed);
                    animator.SetTrigger(m_ParameterHashed);
                    break;
            }
            return true;
        }

        private enum ParameterType
        {
            Integer,
            Float,
            Bool,
            Trigger
        }
    }
}