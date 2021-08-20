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
var Shipwreck;
(function (Shipwreck) {
    var ViewModelUtils;
    (function (ViewModelUtils) {
        function initDateTimePicker(element, obj, value, format, useCurrent) {
            var jq = $(element);
            jq.val(value);
            if (jq.data('datetimepicker')) {
                jq.datetimepicker('format', format);
                jq.datetimepicker('useCurrent', useCurrent);
            }
            else {
                jq.on('change.datetimepicker', function (e) {
                    obj.invokeMethodAsync('SetValueFromJS', e.currentTarget.value);
                }).datetimepicker({
                    format: format,
                    locale: 'ja',
                    useCurrent: useCurrent,
                    extraFormats: [
                        "YYYY-MM-DD HH:mm:ss",
                        "YYYY-MM-DD HH:mm",
                        "YYYY-MM-DD HH",
                        "YYYY-MM-DD",
                        "YYYY-MM",
                        "YYYY",
                    ].map(function (e) { return [e, e.replace('-', '/')]; })
                        .reduce(function (a, b) { return a.concat(b); })
                        .map(function (e) { return [e, e.replace(' HH', 'THH')]; })
                        .reduce(function (a, b) { return a.concat(b); })
                        .sort(function (a, b) { return (b.length - a.length) || a.localeCompare(b); })
                        .filter(function (e, i, a) { return e !== a[i - 1]; })
                });
            }
            jq.val(value);
        }
        ViewModelUtils.initDateTimePicker = initDateTimePicker;
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
