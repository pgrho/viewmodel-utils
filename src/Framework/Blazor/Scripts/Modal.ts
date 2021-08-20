// @ts-ignore
namespace Shipwreck.ViewModelUtils {
    export function toggleModal(element: Element, isOpen: boolean, obj) {
        (<any>$)(element).one('hidden.bs.modal', function () {
            obj.invokeMethodAsync('OnClosed');
        }).one('click', function (e: JQueryEventObject) {
            if (e.target === e.currentTarget) {
                (<any>$)(e.currentTarget).modal('hide');
            }
        }).modal({
            show: !!isOpen,
            backdrop: false,
        }).modal(isOpen ? 'show' : 'hide');
    }
}