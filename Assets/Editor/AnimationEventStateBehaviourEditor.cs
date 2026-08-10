using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

[CustomEditor(typeof(AnimationEventStateBehaviour))]
public class AnimationEventStateBehaviourEditor : Editor
{
    private AnimationClip _previewClip;
    private float _previewTime;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var stateBehaviour = (AnimationEventStateBehaviour)target;

        if(Validate(stateBehaviour, out var errorMessage))
        {
            GUILayout.Space(10);

            PreviewAnimationClip(stateBehaviour);

            GUILayout.Label($"Previewing at {_previewTime:F2}s", EditorStyles.helpBox);
        }
        else
        {
            EditorGUILayout.HelpBox(errorMessage, MessageType.Info);
        }
    }

    private void PreviewAnimationClip(AnimationEventStateBehaviour stateBehaviour)
    {
        if (_previewClip == null) return;

        _previewTime = stateBehaviour.TriggerTime * _previewClip.length;

        AnimationMode.StartAnimationMode();
        AnimationMode.SampleAnimationClip(Selection.activeGameObject, _previewClip, _previewTime);
        //AnimationMode.StopAnimationMode();
    }

    private bool Validate(AnimationEventStateBehaviour stateBehaviour, out string errorMessage)
    {
        var controller = GetValidAnimatorController(out errorMessage);
        if (controller == null) return false;

        var matchingState = controller.layers
            .SelectMany(layer => layer.stateMachine.states)
            .FirstOrDefault(state => state.state.behaviours.Contains(stateBehaviour));

        _previewClip = matchingState.state?.motion as AnimationClip;
        if(_previewClip == null)
        {
            errorMessage = "No valid AnimationClip found for the current state";
            return false;
        }

        return true;
    }

    private AnimatorController GetValidAnimatorController(out string errorMessage)
    {
        errorMessage = string.Empty;

        var targetGO = Selection.activeGameObject;
        if(targetGO == null)
        {
            errorMessage = "Select a GO with an Animator to preview";
            return null;
        }

        var animator = targetGO.GetComponent<Animator>();
        if(animator == null)
        {
            errorMessage = "The selected GO doesn't have an Animator component";
            return null;
        }

        var animatorController = animator.runtimeAnimatorController as AnimatorController;
        if(animatorController == null)
        {
            errorMessage = "The selected Animator doesn't have a valid AnimatorController";
            return null;
        }

        return animatorController;
    }
}
