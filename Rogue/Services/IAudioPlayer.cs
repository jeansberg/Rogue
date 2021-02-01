using System.Threading.Tasks;

namespace Rogue.Services {
    public interface IAudioPlayer {
        Task PlaySound(string soundPattern);
    }
}
