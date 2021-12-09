// Show/hide password onClick of button using Javascript only

// https://stackoverflow.com/questions/31224651/show-hide-password-onclick-of-button-using-javascript-only
$(document).ready(function () {
    function show() {
        var p = document.getElementById('pwd');
        p.setAttribute('type', 'text');
    }

    function hide() {
        var p = document.getElementById('pwd');
        p.setAttribute('type', 'password');
    }

    var pwShown = 0;
    console.log("eye= " + document.getElementById("eye"))
    document.getElementById("eye").addEventListener("click", function () {
        if (pwShown == 0) {
            pwShown = 1;
            show();
        } else {
            pwShown = 0;
            hide();
        }
    }, false);



    function show2() {
        var p = document.getElementById('pwd2');
        p.setAttribute('type', 'text');
    }

    function hide2() {
        var p = document.getElementById('pwd2');
        p.setAttribute('type', 'password');
    }

    var pwShown2 = 0;
    console.log("eye2= " + document.getElementById("eye2"))
    document.getElementById("eye2").addEventListener("click", function () {
        if (pwShown2 == 0) {
            pwShown2 = 1;
            show2();
        } else {
            pwShown2 = 0;
            hide2();
        }
    }, false);



    function checkPasswordMatch() {
        var password = $("#pwd").val();
        var confirmPassword = $("#pwd2").val();

        if (password != confirmPassword)
            $("#checkPasswordMatch").html("Passwords does not match!").css("color", "red");
        else
            $("#checkPasswordMatch").html("Passwords match.").css("color", "green");
    }

    $("#pwd").keyup(checkPasswordMatch);
    $("#pwd2").keyup(checkPasswordMatch);

    $("#myform").submit(function () {
        var pwdMes = $("#checkPasswordMatch").html();
        var notMatch = pwdMes != "Passwords match." && pwdMes != "";
        if (notMatch) return false;
      
    })
})
