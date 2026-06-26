const BuildDocument = (() => {

    async function build(url, options = {}) {

        const {
            onBeforeSend,
            onSuccess,
            onError
        } = options;

        try {

            // ===== 1. Gom toàn bộ dữ liệu =====
            const data = {};

            document
                .querySelectorAll("input, textarea, select")
                .forEach(el => {

                    if (!el.id) return;

                    // bỏ file input
                    if (el.type === "file") return;

                    data[el.id] = el.value ?? "";
                });

            // hook trước khi gửi
            if (onBeforeSend) onBeforeSend(data);

            // ===== 2. Call API =====
            const response = await fetch(url, {

                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(data)

            });

            // ===== 3. Handle response =====
            if (!response.ok) {

                const errText = await response.text();

                if (onError) onError(errText);

                console.error("Build failed:", errText);

                return;
            }

            // Nếu backend trả file / json
            let result;

            const contentType = response.headers.get("content-type") || "";

            if (contentType.includes("application/json")) {
                result = await response.json();
            } else {
                result = await response.text();
            }

            // ===== 4. Success =====
            if (onSuccess) {
                onSuccess(result);
            } else {
                alert("Build thành công!");
            }

        }
        catch (ex) {

            console.error(ex);

            if (onError) onError(ex.message);

            else alert("Có lỗi xảy ra khi build document");
        }
    }

    return {
        build
    };

})();