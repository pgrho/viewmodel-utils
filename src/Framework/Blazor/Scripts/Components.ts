namespace Shipwreck.ViewModelUtils {
    export function setIndeterminate(id: string, value: boolean) {
        const e = document.getElementById(id) as HTMLInputElement;
        if (e) {
            e.indeterminate = value;
            return;
        }
        $(id).prop('indeterminate', value);
    }
}