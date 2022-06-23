// Amplify Color - Advanced Color Grading for Unity
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>

using UnityEditor;

public class AmplifyColorVolumeEditor : AmplifyColor.VolumeEditorBase
{
    [MenuItem("Window/Amplify Color/Volume Editor", false, 1)]
    public static void Init()
    {
        GetWindow<AmplifyColorVolumeEditor>(false, "Volume Editor", true);
    }
}
