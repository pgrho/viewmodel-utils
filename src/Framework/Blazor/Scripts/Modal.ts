// @ts-ignore
namespace Shipwreck.ViewModelUtils {
    let _timeout: any;
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


        const h = () => {
            if (document.querySelector('.modal.show')) {
                document.body.classList.add("modal-open");
            } else {
                document.body.classList.remove("modal-open");
                if (_timeout) {
                    clearInterval(_timeout);
                    _timeout = 0;
                }
            }
        };
        h();
        if (!_timeout) {
            _timeout = setInterval(h, 1000);
        }
    }
}