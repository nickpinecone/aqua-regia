using System.IO;
using TypedPath;

namespace AquaRegia.Library;

[TypedPath("Assets", originalFilename: true)]
public partial class Assets : ITypedPath
{
    public static string Wrap(string path)
    {
        return "AquaRegia/" + path.Replace(Path.GetFileName(path), Path.GetFileNameWithoutExtension(path));
    }
}