// @ts-ignore
namespace Shipwreck.ViewModelUtils {
    export function openWindow(url: string, name: string, features: string) {
        // TODO: vmu (core)
        const w = window.open(url, name, features);
        if (w) {
            w.focus();
            return true;
        } else {
            return false;
        }
    }
}