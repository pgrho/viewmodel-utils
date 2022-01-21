// @ts-ignore
namespace Shipwreck.ViewModelUtils {
    export function focus(element: HTMLInputElement | HTMLTextAreaElement, selectAll: boolean) {
        if (element) {
            element.focus();
            if (selectAll) {
                element.select();
            }
        }
    }
}