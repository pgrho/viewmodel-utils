// @ts-ignore
namespace Shipwreck.ViewModelUtils {
    export function sendFiles(method: string, url: string, headersJson: string, files: ((HTMLInputElement | File)[] | HTMLInputElement | File)): Promise<string> {
        return new Promise<string>(function (resolve, reject) {
            const xhr = new XMLHttpRequest();
            xhr.onreadystatechange = function () {
                switch (xhr.readyState) {
                    case XMLHttpRequest.DONE:
                        xhr.onreadystatechange = null;
                        try {
                            resolve(JSON.stringify({
                                Status: xhr.status,
                                StatusText: xhr.statusText,
                                ResponseText: xhr.responseText
                            }));
                        } catch (ex) {
                            reject(ex);
                        }
                        break;
                }
            };

            xhr.open(method, url);
            xhr.setRequestHeader("Accept", "application/json, text/javascript, */*; q = 0.01");

            const reqHeaders = JSON.parse(headersJson || '{}');
            for (let k of Object.getOwnPropertyNames(reqHeaders)) {
                xhr.setRequestHeader(k, reqHeaders[k]);
            }

            const fd: FormData = new FormData();
            if (Array.isArray(files)) {
                for (const f of (files as (HTMLInputElement | File)[])) {
                    if (f instanceof File) {
                        fd.append("file", f);
                    } else if (f instanceof HTMLInputElement) {
                        for (let i = 0; i < f.files.length; i++) {
                            fd.append("file", f.files[i]);
                        }
                    }
                }
            } else if (files instanceof File) {
                fd.append("file", files);
            } else if (files instanceof HTMLInputElement) {
                for (let i = 0; i < files.files.length; i++) {
                    fd.append("file", files.files[i]);
                }
            }

            xhr.send(fd);
        });
    }
}