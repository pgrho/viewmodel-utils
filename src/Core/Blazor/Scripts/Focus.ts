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
    export function containsActiveElement(p: Element) {
        for (let e = document.activeElement; e; e = e.parentElement) {
            if (e === p) {
                return true;
            }
        }
        return false;
    }
    export function showPopover(e: HTMLElement) {
        (e as any).showPopover();
    }
    export function hidePopover(e: HTMLElement) {
        (e as any).hidePopover();
    }
}