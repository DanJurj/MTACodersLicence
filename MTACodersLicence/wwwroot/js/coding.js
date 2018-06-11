function run() {
    var editor = ace.edit("editor");
    var code = editor.getSession().getValue();
    var code2 = btoa(unescape(encodeURIComponent(code)));
    //var code2 = code.split('"').join("\\\"");
    document.getElementById("code").value = code;
    //var input = document.getElementById("input").value;
    //document.getElementById("input").value = btoa(unescape(encodeURIComponent(input)));
    var language = $("#language option:selected").val();
    document.getElementById("languageCode").value = language;
}

function run2() {
    document.getElementById("performanceTab").style.display = "none";
    var code = editor.getSession().getValue();
    var codeBase64 = btoa(unescape(encodeURIComponent(code)));
    var input = document.getElementById("input").value;
    var inputBase64 = btoa(unescape(encodeURIComponent(input)));
    var languageId = $("#language option:selected").val();
    var data = {
        source_code: codeBase64,
        language_id: languageId,
        stdin: inputBase64
    };
    $.ajax({
        url: "https://api.judge0.com/submissions?base64_encoded=true&wait=true",
        type: "POST",
        async: true,
        contentType: "application/json",
        data: JSON.stringify(data),
        success: function(result) {
            console.log(`Your submission token is: ${result.token}`);
            handleResult(result);
        },
        error: function (xhr, status, error) {
           alert("fail");
        }
});
}

function handleResult(result) {
    var status = result.status;
    var stdout = decodeURIComponent(escape(atob(result.stdout || "")));
    var stderr = decodeURIComponent(escape(atob(result.stderr || "")));
    var compileOutput = decodeURIComponent(escape(atob(result.compile_output || "")));
    var message = decodeURIComponent(escape(atob(result.message || "")));
    var time = (result.time === null ? "-" : result.time + "s");
    var memory = (result.memory === null ? "-" : result.memory + "KB");

    if (status.id === 6) {
        document.getElementById("outputTag").textContent = "Compilation Error";
        document.getElementById("output").value = compileOutput;
    } else if (status.id === 13) {
        document.getElementById("outputTag").textContent = "Internal Error";
        document.getElementById("output").value = message;
    } else if (status.id !== 3 && stderr !== "") {
        stdout += (stdout === "" ? "" : "\n") + stderr;
        document.getElementById("output").value = stdout;
    } else {
        stdout += (stdout === "" ? "" : "\n") + stderr;
        document.getElementById("output").value = stdout;
        document.getElementById("performanceTab").style.display = "block";
        document.getElementById("timeConsumed").innerHTML = time;
        document.getElementById("memory").innerHTML = memory;
    }
}

function hideCerinte() {
    document.getElementById("cerinte").style.display = 'none';
    document.getElementById("hideBtn").style.display = 'none';
    document.getElementById("showBtn").style.display = 'block';
    document.getElementById("codingPart").className = "col-md-11";
}

function showCerinte() {
    document.getElementById("cerinte").style.display = 'block';
    document.getElementById("hideBtn").style.display = 'block';
    document.getElementById("showBtn").style.display = 'none';
    document.getElementById("codingPart").className = "col-md-6";
}

function showHint() {
    document.getElementById("hint").style.display = 'block';
    document.getElementById("hintBtn").style.display = 'none';
}

// functie schimbare tema
function changeTheme() {
    var pos = document.getElementById("theme").options.selectedIndex;
    switch (pos) {
        case 0: editor.setTheme("ace/theme/ambiance");
            break;
        case 1: editor.setTheme("ace/theme/chaos");
            break;
        case 2: editor.setTheme("ace/theme/chrome");
            break;
        case 3: editor.setTheme("ace/theme/clouds");
            break;
        case 4: editor.setTheme("ace/theme/cobalt");
            break;
        case 5: editor.setTheme("ace/theme/dracula");
            break;
        case 6: editor.setTheme("ace/theme/dreamweaver");
            break;
        case 7: editor.setTheme("ace/theme/eclipse");
            break;
        case 8: editor.setTheme("ace/theme/github");
            break;
        case 9: editor.setTheme("ace/theme/sqlserver");
            break;
        case 10: editor.setTheme("ace/theme/terminal");
            break;
        case 11: editor.setTheme("ace/theme/xcode");
            break;
        default:
            break;
    }
}

// functie citire fisier si incarcare in editor
function readSingleFile(evt) {
    var f = evt.target.files[0];
    if (f) {
        var r = new FileReader();
        r.onload = function (e) {
            var contents = e.target.result;
            editor.setValue(contents);
        }
        r.readAsText(f);
    } else {
        alert("Failed to load file");
    }
}

// functie ascundere timp
function toggleTime() {
    $('#time').fadeToggle('fast');
}

// functie salvare cod
function saveCode() {
    var code = editor.getValue();
    document.getElementById("savedCode").value = code;
    var language = $("#language option:selected").text();
    document.getElementById("languageNameSave").value = language;
}

