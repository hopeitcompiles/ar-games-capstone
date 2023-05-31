using UnityEditor;

public class Build
{
    static void BuildAndroid()
    {
        // Configura las opciones de compilaci�n para Android
        BuildPlayerOptions buildOptions = new BuildPlayerOptions();
        buildOptions.target = BuildTarget.Android;
        buildOptions.locationPathName = "Builds/Android/MyGame.apk";
        buildOptions.options = BuildOptions.None;

        // Ejecuta la compilaci�n
        BuildPipeline.BuildPlayer(buildOptions);
    }

    static void BuildiOS()
    {
        // Configura las opciones de compilaci�n para iOS
        BuildPlayerOptions buildOptions = new BuildPlayerOptions();
        buildOptions.target = BuildTarget.iOS;
        buildOptions.locationPathName = "Builds/iOS/MyGame.ipa";
        buildOptions.options = BuildOptions.None;

        // Ejecuta la compilaci�n
        BuildPipeline.BuildPlayer(buildOptions);
    }
}
