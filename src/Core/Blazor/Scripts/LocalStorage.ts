// @ts-ignore
namespace Shipwreck.ViewModelUtils {
    export function readLocalStorage(name: string) {
        return localStorage[name];
    }
    export function writeLocalStorage(name: string, value: string) {
        localStorage[name] = value;
    }
    export function readSessionStorage(name: string) {
        return sessionStorage[name];
    }
    export function writeSessionStorage(name: string, value: string) {
        sessionStorage[name] = value;
    }
    export function userAgent() {
        return navigator.userAgent;
    }
}