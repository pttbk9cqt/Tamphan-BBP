const ToaDoImporter = (() => {

    let data = [];

    const ids = ["L11", "L12", "L21", "L22", "L31", "L32", "L41", "L42"];

    function showMsg(msg, isError = true) {
        const el = document.getElementById("msg");
        if (!el) return;

        el.innerText = msg;
        el.className = isError ? "text-danger" : "text-success";
    }

    function clearMsg() {
        const el = document.getElementById("msg");
        if (el) el.innerText = "";
    }

    function bindEvents() {

        const dropZone = document.getElementById("dropZone");
        const fileInput = document.getElementById("fileExcel");

        if (!dropZone || !fileInput) return;

        dropZone.addEventListener("click", () => fileInput.click());

        fileInput.addEventListener("change", handleFile);

        dropZone.addEventListener("dragover", e => {
            e.preventDefault();
            dropZone.classList.add("bg-light");
        });

        dropZone.addEventListener("dragleave", () => {
            dropZone.classList.remove("bg-light");
        });

        dropZone.addEventListener("drop", e => {
            e.preventDefault();
            dropZone.classList.remove("bg-light");

            const file = e.dataTransfer.files[0];
            readExcel(file);
        });
    }

    function handleFile(e) {
        const file = e.target.files[0];
        readExcel(file);
    }

    function readExcel(file) {

        if (!file) return;

        const fileNameEl = document.getElementById("fileName");
        if (fileNameEl) fileNameEl.innerText = file.name;

        const reader = new FileReader();

        reader.onload = function (e) {

            const dataExcel = new Uint8Array(e.target.result);
            const workbook = XLSX.read(dataExcel, { type: "array" });

            const sheetName = workbook.SheetNames[0];
            const sheet = workbook.Sheets[sheetName];

            data = XLSX.utils.sheet_to_json(sheet, { header: 1, raw: false });

            if (!data || data.length < 4) {
                showMsg("File phải có ít nhất 4 dòng dữ liệu");
                return;
            }

            fillToCoordinates();
            clearMsg();
        };

        reader.readAsArrayBuffer(file);
    }

    function fillToCoordinates() {

        if (!data || data.length < 4 || (data[0] || []).length < 2) {
            showMsg("Excel phải có ít nhất 4 dòng và 2 cột");
            return;
        }

        const values = [
            data[0][0], data[0][1],
            data[1][0], data[1][1],
            data[2][0], data[2][1],
            data[3][0], data[3][1]
        ];

        ids.forEach((id, i) => {

            const el = document.getElementById(id);
            if (!el) return;

            if (typeof formatNumstyleVietNam === "function") {
                let raw = values[i] !== undefined && values[i] !== null
                    ? values[i].toString()
                    : "";

                if (raw === null || raw === undefined) {
                    el.value = "";
                    return;
                }

                el.value = formatNumstyleVietNam(String(raw));
            } else {
                el.value = values[i] ?? "";
            }
        });
    }

    function clearAll() {

        data = [];

        const fileInput = document.getElementById("fileExcel");
        const fileNameEl = document.getElementById("fileName");

        if (fileInput) fileInput.value = "";
        if (fileNameEl) fileNameEl.innerText = "";

        ids.forEach(id => {
            const el = document.getElementById(id);
            if (el) el.value = "";
        });

        clearMsg();
    }

    return {
        init: function () {
            bindEvents();
        },
        clear: clearAll
    };

})();