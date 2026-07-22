using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

[RequireComponent(typeof(Animator))]
public class ReversePlayable : MonoBehaviour
{
    public AnimationClip clip;
    private PlayableGraph playableGraph;

    void Start()
    {
        if (clip == null) return;

        // 1. Create a graph to manage the animation
        playableGraph = PlayableGraph.Create("ReverseAnimationGraph");

        // 2. Create a playable node for your specific clip
        var clipPlayable = AnimationClipPlayable.Create(playableGraph, clip);

        // 3. REVERSE IT: Set speed to -1
        clipPlayable.SetSpeed(-1.0);
        
        // 4. Start the clip at its end time so it doesn't wait a cycle to appear
        clipPlayable.SetTime(clip.length);

        // 5. Output the graph directly to the Animator component
        var playableOutput = AnimationPlayableOutput.Create(playableGraph, "Animation", GetComponent<Animator>());
        playableOutput.SetSourcePlayable(clipPlayable);

        // 6. Play the graph
        playableGraph.Play();
    }

    void OnDestroy()
    {
        // Always clean up PlayableGraphs to prevent memory leaks
        if (playableGraph.IsValid())
        {
            playableGraph.Destroy();
        }
    }
}