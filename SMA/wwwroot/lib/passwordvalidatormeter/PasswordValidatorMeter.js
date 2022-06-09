const userInput = document.getElementById("password"),
  meter = document.getElementById("password-meter"),
  letter = document.getElementById("letter"),
  capital = document.getElementById("capital"),
  number = document.getElementById("number"),
  character = document.getElementById("character"),
  minLength = document.getElementById("min-length"),
  maxLength = document.getElementById("max-length");

const btn = document.getElementById("btnCambiarClave");
btn.disabled = true;

const confirm = document.getElementById("confirmPassword");

// when the user clicks on the password field,
// show the message box and password strength meter
userInput.onfocus = function () {
  //document.getElementById("message").style.display = "block";
  document.getElementById("password-meter").style.display = "block";
  document.getElementById("meter-strength").style.display = "block";
};

// When the user clicks outside of the password field, hide the message box
// password strength meter
userInput.onblur = function () {
  //document.getElementById("message").style.display = "none";
  document.getElementById("password-meter").style.display = "none";
  document.getElementById("meter-strength").style.display = "none";
};

// When the user starts to type something inside the password field
userInput.onkeyup = function () {
  let lowerCase = 0,
    upperCase = 0,
    numberValue = 0,
    specialChar = 0;

  // Validate lowercase letters
  const lowerCaseLetters = /[a-z]/g;
  if (userInput.value.match(lowerCaseLetters)) {
    letter.classList.remove("invalid");
    letter.classList.add("valid");
  } else {
    letter.classList.remove("valid");
    letter.classList.add("invalid");
  }

  // Validate capital letters
  const upperCaseLetters = /[A-Z]/g;
  if (userInput.value.match(upperCaseLetters)) {
    capital.classList.remove("invalid");
    capital.classList.add("valid");
  } else {
    capital.classList.remove("valid");
    capital.classList.add("invalid");
  }

  // Validate numbers
  const numbers = /[0-9]/g;
  if (userInput.value.match(numbers)) {
    number.classList.remove("invalid");
    number.classList.add("valid");
  } else {
    number.classList.remove("valid");
    number.classList.add("invalid");
  }

  // Validate special characters
    const specialCharacters = /[!#$%&'()*+,-./:;<=>?[\]^_{|}~]/g;
  if (userInput.value.match(specialCharacters)) {
    character.classList.remove("invalid");
    character.classList.add("valid");
  } else {
    character.classList.remove("valid");
    character.classList.add("invalid");
  }

  // Validate length
  if (userInput.value.length >= 8) {
    minLength.classList.remove("invalid");
    minLength.classList.add("valid");
  } else {
    minLength.classList.remove("valid");
    minLength.classList.add("invalid");
  }
  if (userInput.value.length <= 25) {
    maxLength.classList.remove("invalid");
    maxLength.classList.add("valid");
  } else {
    maxLength.classList.remove("valid");
    maxLength.classList.add("invalid");
  }

  // meter logic
  if (userInput.value.match(lowerCaseLetters)) {
    lowerCase = 20;
  }
  if (userInput.value.match(upperCaseLetters)) {
    upperCase = 20;
  }
  if (userInput.value.match(numbers)) {
    numberValue = 20;
  }

  var minMax = 0;
  if (userInput.value.match(specialCharacters)) {
    specialChar = 20;
  }

  if (userInput.value.length <= 25 && userInput.value.length >= 8) {
    minMax = 20;
  }

  meter.value = upperCase + lowerCase + numberValue + specialChar + minMax;
  document.getElementById("password-pecentage").innerHTML = meter.value + "%";

  if (meter.value == 100 && confirm.value == userInput.value) {
    btn.disabled = false;
    document.getElementById("errorConfirm").innerHTML = "";
  } else {
    document.getElementById("errorConfirm").innerHTML =
      "Las contraseñas no coinciden.";
    btn.disabled = true;
  }

  return meter.value;
};

confirm.onkeyup = function () {
  if (meter.value == 100 && confirm.value == userInput.value) {
    btn.disabled = false;
    document.getElementById("errorConfirm").innerHTML = "";
    return false;
  } else {
    document.getElementById("errorConfirm").innerHTML =
      "Las contraseñas no coinciden.";
    btn.disabled = true;
    return true;
  }
};

//Ver contrasena Clave
$("#eyePass").click(function () {
  var password = document.getElementById("password");
  if (password.type === "password") {
    password.type = "text";
    $("#eyePass").toggleClass("fa-eye fa-eye-slash");
  } else {
    password.type = "password";
    $("#eyePass").toggleClass("fa-eye-slash fa-eye");
  }
});

//Ver contrasena confirmar clave
$("#eyeConfirm").click(function () {
  var password = document.getElementById("confirmPassword");
  if (password.type === "password") {
    password.type = "text";
    $("#eyeConfirm").toggleClass("fa-eye fa-eye-slash");
  } else {
    password.type = "password";
    $("#eyeConfirm").toggleClass("fa-eye-slash fa-eye");
  }
});
