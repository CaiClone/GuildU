using UnityEditor;

namespace Assets.Serializable.Editor
{
    public class OptionsAsset
    {
        [MenuItem("Assets/Create/Options")]
        public static void CreateAsset()
        {
            CustomAssetUtility.CreateAsset<Options>();
        }
    }
}
