using System.Threading.Tasks;

namespace Unity.Services.LevelPlay.Editor
{
    interface ILevelPlaySdkInstaller
    {
        Task OnLoad();
    }
}
