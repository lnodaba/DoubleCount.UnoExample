using Tizen.Applications;
using Uno.UI.Runtime.Skia;

namespace Dcx.App.Skia.Tizen
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new TizenHost(() => new Dcx.App.App(), args);
            host.Run();
        }
    }
}
