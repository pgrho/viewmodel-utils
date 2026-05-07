/// <reference path="../node_modules/popper.js/index.d.ts" />
var Popper;
namespace Shipwreck.ViewModelUtils {
    // @ts-ignore
    export function initializeToast(element, obj) {
        (<any>$)(element).one('hidden.bs.toast', function () {
            obj.invokeMethodAsync('OnHidden');
        }).toast('show');
    }
}