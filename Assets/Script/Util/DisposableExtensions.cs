using System;

namespace Game.Utils {
    public static class DisposabeExtensions {
        public static T DisposeAndNull<T>(this T disposable) where T : class, IDisposable {
            disposable?.Dispose();
            return null;
        }
    }
}