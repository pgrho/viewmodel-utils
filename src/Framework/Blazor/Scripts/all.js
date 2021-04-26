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
