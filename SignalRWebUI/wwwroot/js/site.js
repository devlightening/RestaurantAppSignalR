document.addEventListener("DOMContentLoaded", function () {
    const body = document.body;
    const themeToggle = document.getElementById("themeToggle");
    const themeIcon = document.getElementById("themeIcon");

    if (!themeToggle || !themeIcon) return; // buton yoksa çık

    let darkMode = localStorage.getItem("theme") === "dark";

    function applyTheme() {
        if (darkMode) {
            body.classList.add("dark-mode");
            themeIcon.classList.remove("bi-moon-fill");
            themeIcon.classList.add("bi-sun-fill");
        } else {
            body.classList.remove("dark-mode");
            themeIcon.classList.remove("bi-sun-fill");
            themeIcon.classList.add("bi-moon-fill");
        }
    }

    applyTheme();

    themeToggle.addEventListener("click", function () {
        darkMode = !darkMode;
        localStorage.setItem("theme", darkMode ? "dark" : "light");
        applyTheme();
    });
});
