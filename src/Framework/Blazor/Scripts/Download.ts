// @ts-ignore
namespace Shipwreck.ViewModelUtils {
    export async function downloadFile(obj, method: string, url: string, headersJson: string, content: string, contentType: string, openFile: boolean) {

        const reqHeaders = JSON.parse(headersJson || '{}');
        if (contentType && content) {
            reqHeaders['Content-Type'] = content;
        }

        const res = await fetch(url, {
            method,
            headers: reqHeaders,
            body: content
        });

        if (res.ok) {
            const resHeaders = {};

            res.headers.forEach((v, k) => resHeaders[k] = v);

            const fn = (await obj.invokeMethodAsync('GetDownloadingFileName', url, res.url, JSON.stringify(resHeaders))) || 'download';

            const b = await res.blob();
            const ou = URL.createObjectURL(b);

            const a = document.createElement('a');
            a.style.display = 'none';
            a.href = ou;
            a.download = fn;
            if (openFile) {
                a.target = '_blank';
            }
            document.body.appendChild(a);
            a.click();
            document.body.removeChild(a);

            URL.revokeObjectURL(ou);
        } else {
            throw `ファイルのダウンロードに失敗しました。${res.status} ${res.statusText}`;
        }
    }
}