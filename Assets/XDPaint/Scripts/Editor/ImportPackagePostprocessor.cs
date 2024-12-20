using System.Collections.Generic;
using UnityEditor;

namespace XDPaint.Editor
{
    [InitializeOnLoad]
    public class ImportPackagePostprocessor
    {
        private static readonly KeyValuePair<string, string>[] AssetsToRemove =
        {
            new KeyValuePair<string, string>("TexturesKeeper", "75ae33b80cae64c25be3d931c115a0fe"),
            new KeyValuePair<string, string>("RenderTarget", "43bbae924d5314cd6a3f1c441abe5f50"),
            new KeyValuePair<string, string>("ToolSettingsDrawer", "789ac1649edfa4d62b584b0d904d7ba8"),
            new KeyValuePair<string, string>("InputDataBase", "a361df090e2b94d8baed03dd3c638386"),
            new KeyValuePair<string, string>("RaycastMeshData", "58d87bd4228554184bad93bf8a5ed9b4"),
            new KeyValuePair<string, string>("RaycastMeshDataBase", "ab15234766b09432f84d738b2876776b"),
            new KeyValuePair<string, string>("PaintToolPropertyAttribute", "a08f874aa2db04a11b311b9c8efaff44"),
            new KeyValuePair<string, string>("SkinnedMeshRendererBonesData", "a8264789a4f0d4d88be6be24f92726d8"),
            new KeyValuePair<string, string>("AverageColorCutOff", "76dd38e3438b849b4a28198fb5295dd8"),
            new KeyValuePair<string, string>("TrianglesDataWindow", "a5737f350eb6248e4a8459fe26e7ae9a"),
            new KeyValuePair<string, string>("FrameIntersectionData", "02a9ec0638bcf41828c6e6da88dff130"),
            new KeyValuePair<string, string>("Manual", "bdc2ec32f8f194b6fb52bcd13fad40d4"),
            new KeyValuePair<string, string>("v.3.0_API_Changes", "2a94caabd469940ffb479d18dd44bc29"),
            new KeyValuePair<string, string>("TrianglesContainer", "19fac5b48c1e8406e8bd7b054fb4dd96"),
            new KeyValuePair<string, string>("InputDataDefault", "c8b78c690001b49119d4aa6ba4c97bb7"),
            new KeyValuePair<string, string>("InputDataCanvas", "5f185ee1b76304dcc847aa843aa92ade"),
            new KeyValuePair<string, string>("InputDataResolver", "b4c7dcc75ce5c47d7b33c7ce068957c1"),
            new KeyValuePair<string, string>("InputActionType", "0ff1a85809914bf2896baeada23d8776"),
            new KeyValuePair<string, string>("InputMethods", "2633bc699b874059b0509e604e0b8118"),
            new KeyValuePair<string, string>("RaycastSystemType", "51d81ce6a3044b92a7de0c1fa42527e8"),
            new KeyValuePair<string, string>("BasePaintObjectData", "d743ad6a1bba4430bac21bc08cfbe670"),
            new KeyValuePair<string, string>("LineData", "e1b49e081ed194d4bafd0cffbc978fd8"),
            new KeyValuePair<string, string>("DrawLineExtendedData", "fd095106ba1d48c299c8364b6a5ea2de"),
        };

        static ImportPackagePostprocessor()
        {
            AssetDatabase.importPackageCompleted += OnImportPackageCompleted;
        }

        private static void OnImportPackageCompleted(string packageName)
        {
            if (packageName == "2D3D Paint" || packageName == "2D/3D Paint")
            {
                foreach (var assetData in AssetsToRemove)
                {
                    var assetName = assetData.Key;
                    var guid = assetData.Value;
                    TryToRemoveAsset(assetName, guid);
                }
                AssetDatabase.Refresh();
            }
            AssetDatabase.importPackageCompleted -= OnImportPackageCompleted;
        }

        private static void TryToRemoveAsset(string assetName, string guid)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            if (!string.IsNullOrEmpty(path) && path.Contains(assetName))
            {
                AssetDatabase.DeleteAsset(path);
            }
        }
    }
}