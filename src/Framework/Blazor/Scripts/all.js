var Shipwreck;
(function (Shipwreck) {
    var ViewModelUtils;
    (function (ViewModelUtils) {
        function setIndeterminate(id, value) {
            var e = document.getElementById(id);
            if (e) {
                e.indeterminate = value;
                return;
            }
            $(id).prop('indeterminate', value);
        }
        ViewModelUtils.setIndeterminate = setIndeterminate;
    })(ViewModelUtils = Shipwreck.ViewModelUtils || (Shipwreck.ViewModelUtils = {}));
})(Shipwreck || (Shipwreck = {}));
// @ts-ignore
var Shipwreck;
(function (Shipwreck) {
    var ViewModelUtils;
    (function (ViewModelUtils) {
        function toggleModal(element, isOpen, obj) {
            $(element).one('hidden.bs.modal', function () {
                obj.invokeMethodAsync('OnClosed');
            }).one('click', function (e) {
                if (e.target === e.currentTarget) {
                    $(e.currentTarget).modal('hide');
                }
            }).modal({
                show: !!isOpen,
                backdrop: false
            }).modal(isOpen ? 'show' : 'hide');
        }
        ViewModelUtils.toggleModal = toggleModal;
    })(ViewModelUtils = Shipwreck.ViewModelUtils || (Shipwreck.ViewModelUtils = {}));
})(Shipwreck || (Shipwreck = {}));
