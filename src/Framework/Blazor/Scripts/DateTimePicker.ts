namespace Shipwreck.ViewModelUtils {
    export function initDateTimePicker(element: Element, obj: any, value: string, format: string, useCurrent: boolean) {
        const jq: any = (<any>$)(element);
        jq.val(value);
        if (jq.data('datetimepicker')) {
            jq.datetimepicker('format', format);
            jq.datetimepicker('useCurrent', useCurrent);
        } else {
            jq.on('change.datetimepicker', function (e: any) {
                obj.invokeMethodAsync('SetValueFromJS', (<HTMLInputElement>e.currentTarget).value);
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
                ].map(e => [e, e.replace('-', '/')])
                    .reduce((a, b) => a.concat(b))
                    .map(e => [e, e.replace(' HH', 'THH')])
                    .reduce((a, b) => a.concat(b))
                    .sort((a, b) => (b.length - a.length) || a.localeCompare(b))
                    .filter((e, i, a) => e !== a[i - 1])
            });
        }
        jq.val(value);
    }
}
(function () {
    const locale =
        ((<any>navigator).userLanguage
            || (<any>navigator).browserLanguage
            || navigator.language
            || 'en').substr(0, 2);
    (<any>window).moment.locale(locale);
    const _$: any = $;
    _$.fn.datetimepicker.Constructor.Default = _$.extend({}, _$.fn.datetimepicker.Constructor.Default, {
        icons: {
            time: 'far fa-clock',
            date: 'far fa-calendar',
            up: 'fas fa-arrow-up',
            down: 'fas fa-arrow-down',
            previous: 'fas fa-chevron-left',
            next: 'fas fa-chevron-right',
            today: 'far fa-calendar-check',
            clear: 'fas fa-trash',
            close: 'fas fa-times'
        },

        dayViewHeaderFormat: /^ja$/i.test(locale) ? 'YYYY年 MMM' : 'MMMM YYYY'
    });
}());