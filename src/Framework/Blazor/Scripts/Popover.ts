/// <reference path="../node_modules/popper.js/index.d.ts" />
var Popper;
namespace Shipwreck.ViewModelUtils {
    export function initializePopper(obj, reference: Element, popper: Element, boundaries: Element, arrow: Element) {
        let placement;
        const handleData = function (data, isCreation) {
            if (data.placement !== placement) {
                placement = data.placement;
                obj.invokeMethodAsync('OnPlacementChanged', placement);
            }
        }
        const p = new Popper(reference, popper, {
            placement: 'right-start',
            modifiers: {
                preventOverflow: {
                    boundariesElement: boundaries
                },
                flip: {
                    behavior: 'flip',
                    boundariesElement: boundaries,
                }, arrow: {
                    element: arrow
                }
            },
            onCreate: function (data) {
                handleData(data, true);
            },
            onUpdate: function (data) {
                handleData(data, false);
            }
        });

        const h = function (ev) {
            for (let p = ev.relatedTarget; p; p = p.parentElement) {
                if (p === reference || p === popper) {
                    return;
                }
            }

            p.destroy();
            obj.invokeMethodAsync('OnDestroy');

            $([reference, popper]).off('focusout', h);
        };

        $([reference, popper]).focusout(h);
    }

    // @ts-ignore
    export function initializeToast(element, obj) {
        (<any>$)(element).one('hidden.bs.toast', function () {
            obj.invokeMethodAsync('OnHidden');
        }).toast('show');
    }
}