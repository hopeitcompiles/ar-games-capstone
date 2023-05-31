using UnityEditor;

public class Build
{
    static void BuildAndroid()
    {
        // Configura las opciones de compilación para Android
        BuildPlayerOptions buildOptions = new BuildPlayerOptions();
        buildOptions.target = BuildTarget.Android;
        buildOptions.locationPathName = "Builds/Android/MyGame.apk";
        buildOptions.options = BuildOptions.None;

        // Ejecuta la compilación
        BuildPipeline.BuildPlayer(buildOptions);
    }

    static void BuildiOS()
    {
        // Configura las opciones de compilación para iOS
        BuildPlayerOptions buildOptions = new BuildPlayerOptions();
        buildOptions.target = BuildTarget.iOS;
        buildOptions.locationPathName = "Builds/iOS/MyGame.ipa";
        buildOptions.options = BuildOptions.None;

        // Ejecuta la compilación
        BuildPipeline.BuildPlayer(buildOptions);
    }
}
