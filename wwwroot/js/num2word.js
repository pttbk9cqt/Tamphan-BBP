function formatTienVietNam(number) {
    return number.toString().replace(/\D/g, "")
        .replace(/\B(?=(\d{3})+(?!\d))/g, ".");
}
function vietHoaChuDau(str) {
    if (!str) return "";
    return str.charAt(0).toUpperCase() + str.slice(1);
}
function num2word(so) {
    so = parseInt(so.toString().replace(/\D/g, "")) || 0;
    if (so === 0) return "không đồng.";

    let chuSo = ["không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín"];
    let hang = ["", " nghìn,", " triệu,", " tỷ,"];

    function doc3So(n) {
        let tram = Math.floor(n / 100);
        let chuc = Math.floor((n % 100) / 10);
        let donvi = n % 10;

        let kq = "";

        if (tram > 0) {
            kq += chuSo[tram] + " trăm";
            if (chuc === 0 && donvi !== 0) kq += " linh";
        }

        if (chuc > 1) {
            kq += " " + chuSo[chuc] + " mươi";
        } else if (chuc === 1) {
            kq += " mười";
        }

        if (donvi > 0) {
            if (donvi === 1 && chuc > 1) kq += " mốt";
            else if (donvi === 5 && chuc > 0) kq += " lăm";
            else kq += " " + chuSo[donvi];
        }

        return kq.trim();
    }

    let i = 0;
    let result = "";

    while (so > 0) {
        let part = so % 1000;

        if (part !== 0) {
            result = doc3So(part) + hang[i] + " " + result;
        }

        so = Math.floor(so / 1000);
        i++;
    }

    return result.trim() + " đồng.";
}

// GẮN EVENT NGAY TẠI ĐÂY
document.addEventListener("DOMContentLoaded", function () {
    let input = document.getElementById("Giatri");
    let output = document.getElementById("GiatriChu");

    input.addEventListener("input", function () {
        let raw = this.value.replace(/\D/g, "");

        // format hiển thị: 1.500.000
        this.value = formatTienVietNam(raw);

        // đổi sang chữ
        output.value = vietHoaChuDau(num2word(raw));
    });
});