#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

public class ReverseAnimationTool
{
    [MenuItem("Assets/Create/Reversed Animation Clip", false, 14)]
    public static void ReverseClip()
    {
        AnimationClip originalClip = Selection.activeObject as AnimationClip;
        if (originalClip == null) return;

        // Determine where to save the new file
        string assetPath = AssetDatabase.GetAssetPath(originalClip);
        string directory = Path.GetDirectoryName(assetPath);
        string newPath = directory + "/" + originalClip.name + "_Reversed.anim";

        // Create the new clip
        AnimationClip reversedClip = new AnimationClip();
        reversedClip.name = originalClip.name + "_Reversed";
        reversedClip.frameRate = originalClip.frameRate;

        // Copy loop settings
        AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(originalClip);
        AnimationUtility.SetAnimationClipSettings(reversedClip, settings);

        // Copy and reverse all animation curves (bones, muscles, etc.)
        EditorCurveBinding[] bindings = AnimationUtility.GetCurveBindings(originalClip);
        foreach (EditorCurveBinding binding in bindings)
        {
            AnimationCurve curve = AnimationUtility.GetEditorCurve(originalClip, binding);
            AnimationCurve reversedCurve = new AnimationCurve();

            foreach (Keyframe key in curve.keys)
            {
                // To reverse a keyframe, subtract its time from the total length, and invert its tangents
                reversedCurve.AddKey(new Keyframe(originalClip.length - key.time, key.value, -key.outTangent, -key.inTangent));
            }
            AnimationUtility.SetEditorCurve(reversedClip, binding, reversedCurve);
        }

        // Save it to the project
        AssetDatabase.CreateAsset(reversedClip, newPath);
        AssetDatabase.SaveAssets();
        Debug.Log("Reversed clip created successfully at: " + newPath);
    }

    // This ensures the menu option only appears when you select an animation clip
    [MenuItem("Assets/Create/Reversed Animation Clip", true)]
    public static bool ValidateReverseClip()
    {
        return Selection.activeObject is AnimationClip;
    }
}
#endif