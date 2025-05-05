let toggleButton = document.getElementById("toggleKeyboard");
let keyboard = document.getElementById("keyboard");

// Проверяем состояние из localStorage при загрузке
if (localStorage.getItem("keyboardVisible") === "false") {
    keyboard.style.display = "none";
    toggleButton.innerText = "Показать клавиатуру";
} else {
    keyboard.style.display = "grid";
    toggleButton.innerText = "Скрыть клавиатуру";
}

toggleButton.addEventListener("click", function () {
    if (keyboard.style.display === "none") {
        keyboard.style.display = "grid";
        toggleButton.innerText = "Скрыть клавиатуру";
        localStorage.setItem("keyboardVisible", "true");
    } else {
        keyboard.style.display = "none";
        toggleButton.innerText = "Показать клавиатуру";
        localStorage.setItem("keyboardVisible", "false");
    }
});

// Логика ввода символов с клавиатуры
document.querySelectorAll(".key").forEach(button => {
    button.addEventListener("click", function () {
        document.getElementById("inputField").value += this.innerText;
    });
});

// Подсказка
document.getElementById("helpButton").addEventListener("click", function () {
    let supportText = document.getElementById("supportText");
    if (supportText.style.display === "none") {
        supportText.style.display = "block";
    } else {
        supportText.style.display = "none";
    }
});
