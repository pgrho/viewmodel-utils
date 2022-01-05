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

            const b = await res.blob();

            if (openFile && b.size < (1 << 24)) {
                const fr = new FileReader();
                fr.onload = () => {
                    window.open(fr.result as string);
                };
                fr.readAsDataURL(b);
            } else {

                const fn = (await obj.invokeMethodAsync('GetDownloadingFileName', url, res.url, JSON.stringify(resHeaders))) || 'download';

                const ou = URL.createObjectURL(b);
                const a = document.createElement('a');
                a.style.display = 'none';
                a.href = ou;
                a.download = fn;
                document.body.appendChild(a);
                a.click();
                document.body.removeChild(a);

                URL.revokeObjectURL(ou);
            }
        } else {
            throw `ファイルのダウンロードに失敗しました。${res.status} ${res.statusText}`;
        }
    }
}