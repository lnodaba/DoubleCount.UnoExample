using Tizen.Applications;
using Uno.UI.Runtime.Skia;

namespace Dcx.Uno.Skia.Tizen
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new TizenHost(() => new Dcx.Uno.App(), args);
            host.Run();
        }
    }
}
